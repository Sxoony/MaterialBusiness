using MaterialBusiness;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Linq;
class Program
{
    static Business localBusiness = new Business("Local Fabrics", "123 Main St");
    static void FabricQueries()
    {
        bool finished = false;
        while (!finished)
        {
            Console.WriteLine("1. List all fabrics");
            Console.WriteLine("2. Add stock to a fabric");
            Console.WriteLine("3. Reduce stock of a fabric");
            Console.WriteLine("4. Find a specific fabric");
            Console.WriteLine("5. View audit log");
            Console.WriteLine("6. Exit\n");
            Console.Write("Choice: ");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    var fabrics = localBusiness.Items.GetAllFabrics();
                    Console.WriteLine("\n--- Fabrics ---");
                    Console.WriteLine("Name\tID\tStock");
                    foreach (var fabric in fabrics)
                    {
                        Console.WriteLine($"{fabric.Name}\t(ID: {fabric.Id})\tStock: {fabric.StockQuantity}");
                    }
                    Console.WriteLine();
                    break;
                case "2":
                    Console.Write("Enter Fabric ID to add stock: ");
                    Guid addId = Guid.Parse(Console.ReadLine() ?? "");
                    Console.Write("Enter quantity to add: ");
                    decimal addQty = decimal.Parse(Console.ReadLine() ?? "0");
                    localBusiness.AddStock(addId, addQty, "Manual Restock");
                    Console.WriteLine("Stock added.\n");
                    break;
                case "3":
                    Console.Write("Enter Fabric ID to reduce stock: ");
                    Guid reduceId = Guid.Parse(Console.ReadLine() ?? "");
                    Console.Write("Enter quantity to reduce: ");
                    decimal reduceQty = decimal.Parse(Console.ReadLine() ?? "0");
                    localBusiness.ReduceStock(reduceId, reduceQty, "Manual Sale");
                    Console.WriteLine("Stock reduced.\n");
                    break;
                case "4":
                    Console.Write("Enter Fabric ID to find: ");
                    Guid findId = Guid.Parse(Console.ReadLine() ?? "");
                    var fabricFound = localBusiness.Items.Get(findId);
                    if (fabricFound != null)
                    {
                        Console.WriteLine($"\nFound Fabric: {fabricFound.Name} (ID: {fabricFound.Id}) - Stock: {fabricFound.StockQuantity}\n");
                    }
                    else
                    {
                        Console.WriteLine("Fabric not found.\n");
                    }
                    break;
                case "5":
                    var logs = localBusiness.AuditLog.GetAll();
                    Console.WriteLine("\n--- Audit Log ---");
                    foreach (var log in logs)
                    {
                        Console.WriteLine($"{log.Timestamp}: {log.Action} on Item {log.ItemName} - {log.Details}");
                    }
                    Console.WriteLine();
                    break;
                case "6":
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }
    }
    static void OrderQueries()
    {
        bool finished = false;
        while (!finished)
        {
            Console.WriteLine("1. View Recent Orders");
            Console.WriteLine("2. Get Orders by Date Range");
            Console.WriteLine("3. Add Order");
            Console.WriteLine("4. Find a Specific Order");
            Console.WriteLine("5. Update an Order");
            Console.WriteLine("6. Remove an Order");
            Console.WriteLine("7. Exit\n");

            Console.Write("Choice: ");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    Console.WriteLine("How many of the most recent orders would you like to see?");
                    Console.Write("Number: ");
                    int numberOfOrders = int.Parse(Console.ReadLine() ?? "5");
                    var recentOrders = localBusiness.Orders.GetRecentOrders(numberOfOrders);
                    Console.WriteLine("\n--- Recent Orders ---");
                    foreach (var order in recentOrders)
                    {
                        Console.WriteLine($"Order ID: {order.Id}, Created: {order.Created}, Items: {order.Lines.Count}");
                    }
                    Console.WriteLine();
                    break;
                case "2":
                    Console.Write("Enter start date (yyyy-MM-dd): ");
                    DateTime startDate = DateTime.Parse(Console.ReadLine() ?? "");
                    Console.Write("Enter end date (yyyy-MM-dd): ");
                    DateTime endDate = DateTime.Parse(Console.ReadLine() ?? "");
                    var ordersInRange = localBusiness.Orders.GetOrdersByDateRange(startDate, endDate);
                    Console.WriteLine($"\n--- Orders from {startDate.ToShortDateString()} to {endDate.ToShortDateString()} ---");
                    foreach (var order in ordersInRange)
                    {
                        Console.WriteLine($"Order ID: {order.Id}, Created: {order.Created}, Items: {order.Lines.Count}");
                    }
                    Console.WriteLine();
                    break;
                case "3":
                    List<OrderLine> lines = new List<OrderLine>();
                    string Item;
                    int Quantity;
                    Console.WriteLine("Adding a new order. Enter the following details:");
                    var fabrics = localBusiness.Items.GetAllFabrics().ToList();
                    foreach (var fabric in fabrics)
                    {
                        Console.WriteLine($"{fabric.Name} (ID: {fabric.Id}) - {fabric.Name} - Stock: {fabric.StockQuantity}");
                    }
                    Console.Write("Item ID: ");
                    Item = Console.ReadLine() ?? "";
                    if (Guid.TryParse(Item, out Guid itemId))
                    {
                        var fabricItem = localBusiness.Items.Get(itemId);
                        if (fabricItem != null)
                        {
                            Console.Write("Quantity: ");
                            Quantity = int.Parse(Console.ReadLine() ?? "0");

                            lines.Add(new OrderLine(fabricItem, Quantity));
                              
                      
                            localBusiness.Orders.Add(lines);
                            Console.WriteLine("Order added successfully.\n");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Item ID.\n");
                        }
                    }

                    break;
                case "4":
                    Console.WriteLine("Enter OrderID: ");
                    string orderID = Console.ReadLine();
                    if (Guid.TryParse(orderID, out Guid orderId))
                    {
                        var order = localBusiness.Orders.Get(orderID);
                        if (order != null)
                        {
                            Console.WriteLine($"Date Created: {order.Created}\nItems:");
                            foreach (var item in order.Lines)
                            {
                                Console.WriteLine($"{item.Item.Id} - {item.Item.Name} - {item.Quantity}");
                            }
                            Console.WriteLine();
                        }
                    }
                    break;
                case "5":
                    Console.WriteLine("Enter OrderID: ");
                     orderID = Console.ReadLine();
                    if (Guid.TryParse(orderID, out Guid orderId2))
                    {
                        
                            localBusiness.Orders.Update(orderID);
                        

                    }
                        break;
                case "6":
                    Console.WriteLine("Enter OrderID: ");
                    orderID = Console.ReadLine();
                    if (Guid.TryParse(orderID, out Guid orderId3))
                    {
                        localBusiness.Orders.Remove(orderID);
                        Console.WriteLine("Order successfully removed!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                    }
                        break;
                case "7":
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }

    }
    static void Main(string[] args)
    {

        Console.WriteLine($"===Welcome to {localBusiness.Name}, on {localBusiness.Address}!===\n");
        bool finished = false;
        while (!finished)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Fabric Queries");
            Console.WriteLine("2. Order Queries");
            Console.WriteLine("3. Promotion Queries");
            Console.WriteLine("4. Audit Log Queries");
            Console.WriteLine("5. Exit\n");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    FabricQueries();
                    break;
                case "2":
                    OrderQueries();
                    break;
                case "3":
                    // Implement PromotionQueries();
                    break;
                case "4":
                    // Implement AuditLogQueries();
                    break;
                case "5":
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }

            //ORDER QUERIES




            //PROMOTION QUERIES



            //AUDIT LOG QUERIES



        }
      
    }
}
