using MaterialBusiness;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

public class Order
{
    [Key]
    public string Id { get; private set; }
    public DateTime Created { get; set; }
    public List<OrderLine> Lines { get; set; }

    public Order()
    {
        Id = Guid.NewGuid().ToString();
        Created = DateTime.Now;
        Lines = new List<OrderLine>();
    }

    public void AddItem(Fabric item, decimal quantity)
    {
        Lines.Add(new OrderLine(item, quantity));
    }
}

public class OrderLine
{
    [Key]
    public Guid Id { get; private set; }

    public Guid ItemId { get; private set; }

    [ForeignKey("ItemId")]
    public Fabric Item { get; set; }

    public decimal Quantity { get;  set; }

    public OrderLine()
    {
        Id = Guid.NewGuid();
    }

    
    public OrderLine(Fabric item, decimal quantity)
    {
        Id = Guid.NewGuid();
        ItemId = item.Id;
        Item = item;
        Quantity = quantity;
    }
}

public class OrderRepository
{
    private readonly BusinessDbContext _context;

    public OrderRepository(BusinessDbContext context)
    {
        _context = context;
    }

    // Add order with its line items
    public void Add(List<OrderLine> lines)
    {
        Order order = new Order();
        foreach (var line in lines)
        {
            order.AddItem(line.Item, line.Quantity);
        }

        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    // Get with related data (Include loads OrderLines too)
    public Order? Get(string id)
    {
        var order = _context.Orders
            .Include(o => o.Lines)      // This loads the OrderLines too
                .ThenInclude(l => l.Item)  // And the Fabric for each line
            .FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Console.WriteLine("Order not found!\n");
            return null;
        }
        else
        {
            Console.WriteLine("Order found!\n");
            return order;
        }
    }

    public IEnumerable<Order> GetAll()
    {
        return _context.Orders
            .Include(o => o.Lines)
                .ThenInclude(l => l.Item)
            .ToList();
    }

    public void Remove(string id)
    {
        var order = Get(id);
        if (order == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        if (order.Lines != null)
        {
            foreach (var item in order.Lines)
            {
                Fabric fabric = item.Item;
                if (fabric != null)
                {
                    fabric.StockQuantity += item.Quantity;
                    _context.Fabrics.Update(fabric);
                }
            }
        }

        _context.Orders.Remove(order);
        _context.SaveChanges();
    }

    // Query methods
    public IEnumerable<Order> GetOrdersByDateRange(DateTime start, DateTime end)
    {
        return _context.Orders
            .Include(o => o.Lines)
            .Where(o => o.Created >= start && o.Created <= end)
            .ToList();
    }

    public void Update(string orderId)
    {
        //since this is an order, and the item properties cannot change, the only properties of an order we can change are the date created, lines (fabric in its entirety), quantity.

        Order order = Get(orderId);
        if (order == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        List<OrderLine> lines = order.Lines;

        bool finished = false;

        while (!finished)
        {
            Console.WriteLine($"What would you like to change?\n1. Order Creation Date\n2. Order Items\n3. Item Quantity\n4. Exit");
            Console.Write("Choice: ");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1": // Change creation date
                    Console.Write("Enter new creation date (yyyy-MM-dd): ");
                    string dateInput = Console.ReadLine() ?? "";
                    if (DateTime.TryParse(dateInput, out DateTime newDate))
                    {
                        order.Created = newDate;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format.");
                    }
                    break;
                case "2": // Change order items
                    Console.WriteLine($"What item would you like to change? Enter the corresponding number.");
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {lines[i].Item.Name} - Quantity: {lines[i].Quantity}");
                    }
                    Console.WriteLine($"{lines.Count + 1}. Exit");
                    Console.Write("Choice: ");
                    string itemChoice = Console.ReadLine() ?? "";
                    if (int.TryParse(itemChoice, out int inty))
                    {
                        if (inty == lines.Count + 1)
                        {
                            break;
                        }
                    }

                    if (int.TryParse(itemChoice, out int itemIndex) && itemIndex >= 1 && itemIndex <= lines.Count)
                    {
                        OrderLine selectedLine = lines[itemIndex - 1];
                        Console.WriteLine($"Selected Item: {selectedLine.Item.Name}");
                        Console.WriteLine("1. Remove Item from Order");
                        Console.WriteLine("2. Replace Item with another Fabric");
                        Console.WriteLine("3. Exit");
                        Console.Write("Choice: ");
                        string actionChoice = Console.ReadLine() ?? "";
                        switch (actionChoice)
                        {
                            case "1": //in order items, removes item
                                lines.RemoveAt(itemIndex - 1);
                                Console.WriteLine("Item removed from order.");

                                selectedLine.Item.StockQuantity += selectedLine.Quantity;
                                _context.Update(selectedLine.Item);
                                _context.SaveChanges();

                                _context.AuditLogs.Add(new AuditLog("Remove item from order", selectedLine.Item.Id, selectedLine.Item.Name, $"Removed {selectedLine.Quantity} {selectedLine.Item.UnitOfMeasure}(s) from order"));
                                break;
                            case "2": // Replace item
                                Console.Write("Enter new Fabric ID to replace with: ");
                                string newFabricIdInput = Console.ReadLine() ?? "";
                                if (Guid.TryParse(newFabricIdInput, out Guid newFabricId))
                                {
                                    Fabric? newFabric = _context.Fabrics.Find(newFabricId);
                                    if (newFabric == null)
                                    {
                                        Console.WriteLine("Fabric not found.");
                                        break;
                                    }

                                    Console.WriteLine($"Enter Quantity ({newFabric.StockQuantity} available)");
                                    string qtyInput = Console.ReadLine() ?? "";
                                    if (int.TryParse(qtyInput, out int result))
                                    {
                                      
                                        if (result < 1 || newFabric.StockQuantity < result)
                                        {
                                            Console.WriteLine("Invalid quantity or insufficient stock.");
                                            break;
                                        }
                                        
                                            Fabric itemToReturn = selectedLine.Item;

                                            // Return old quantity to stock and subtract new quantity from newFabric
                                            itemToReturn.StockQuantity += selectedLine.Quantity;
                                            newFabric.StockQuantity -= result;
                                        
                                        // replace line with new fabric & requested quantity
                                        OrderLine changedLine = new OrderLine(newFabric, result);
                                        
                                        selectedLine.Item = newFabric;
                                        _context.Fabrics.Update(itemToReturn);
                                        _context.Update(selectedLine);
                                        _context.SaveChanges();
                                        _context.AuditLogs.Add(new AuditLog("Replaced item from order", newFabric.Id, newFabric.Name, $"Replaced {itemToReturn.Name} in order with {newFabric.Name}, {result} {newFabric.UnitOfMeasure}(s)"));

                                    
                                        Console.WriteLine("Item replaced successfully.");
                                        
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid quantity format.");
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Fabric ID format.");
                                }
                                break;
                            case "3"://exit
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid item choice.");
                    }
                    break;
                case "3": // this changes the item quantity, outside of lines again now.
                    Console.WriteLine($"What item would you like to change? Enter the corresponding number.");
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {lines[i].Item.Name} - Quantity: {lines[i].Quantity}");
                    }
                    Console.Write("Choice: ");
                    itemChoice = Console.ReadLine() ?? "";
                    if (int.TryParse(itemChoice, out int itemIndexQ) && itemIndexQ >= 1 && itemIndexQ <= lines.Count) //Q is for quantity
                    {
                        OrderLine selectedLine = lines[itemIndexQ - 1];
                        int originalStock = (int)selectedLine.Item.StockQuantity;
                        Console.WriteLine($"Selected Item: {selectedLine.Item.Name} - Quantity {selectedLine.Quantity}");
                        int originalQuantity = (int)selectedLine.Quantity;
                        Console.Write($"Quantity Changing to ({selectedLine.Item.StockQuantity + selectedLine.Quantity} units max): ");
                        int maxQty = (int)(selectedLine.Item.StockQuantity+selectedLine.Quantity);
                        string qtyInput = Console.ReadLine() ?? "";
                        if (decimal.TryParse(qtyInput, out decimal newQty) && newQty <= (selectedLine.Item.StockQuantity + selectedLine.Quantity))
                        {
                            selectedLine.Quantity= newQty;
                           
                            Console.WriteLine("Quantity updated successfully.");

                            maxQty -= (int)newQty;
                            selectedLine.Item.StockQuantity = maxQty;
                           
                            _context.Update(selectedLine);
                            _context.SaveChanges();
                            _context.AuditLogs.Add(new AuditLog("Updated item Quantity", selectedLine.Item.Id, selectedLine.Item.Name, $"from {originalQuantity} {selectedLine.Item.UnitOfMeasure} to {newQty} {selectedLine.Item.UnitOfMeasure}"));
                            _context.AuditLogs.Add(new AuditLog("Stock Changed ", selectedLine.Item.Id, selectedLine.Item.Name, $"from {originalStock} {selectedLine.Item.UnitOfMeasure} to {selectedLine.Item.StockQuantity} {selectedLine.Item.UnitOfMeasure}"));


                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity or exceeds stock.");
                        }

                    }
                    break;
                case "4":
                    finished = true;
                    _context.SaveChanges();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    public IEnumerable<Order> GetRecentOrders(int count)
    {
        if (count <= _context.Orders.Count())
        {
            return _context.Orders
                .Include(o => o.Lines)
                .OrderByDescending(o => o.Created)
                .Take(count)
                .ToList();
        }
        else
        {
            return _context.Orders
                .Include(o => o.Lines)
                .OrderByDescending(o => o.Created)
                .ToList();
        }
    }

}
