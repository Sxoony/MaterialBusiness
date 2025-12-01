using MaterialBusiness;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Order
{
    [Key]
    public string Id { get; private set; }
    public DateTime Created { get; private set; }
    public List<OrderLine> Lines { get; private set; }
    
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
    public Fabric Item { get; private set; }
    public decimal Quantity { get; private set; }
    public OrderLine()
    {
        Id = Guid.NewGuid();
    }
    public OrderLine(Fabric item, decimal quantity)
    {
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
    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    // Get with related data (Include loads OrderLines too)
    public Order? Get(string id)
    {
        return _context.Orders
            .Include(o => o.Lines)      // This loads the OrderLines too
                .ThenInclude(l => l.Item)  // And the Fabric for each line
            .FirstOrDefault(o => o.Id == id);
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
        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }

    // Query methods
    public IEnumerable<Order> GetOrdersByDateRange(DateTime start, DateTime end)
    {
        return _context.Orders
            .Include(o => o.Lines)
            .Where(o => o.Created >= start && o.Created <= end)
            .ToList();
    }

    public IEnumerable<Order> GetRecentOrders(int count = 10)
    {
        return _context.Orders
            .Include(o => o.Lines)
            .OrderByDescending(o => o.Created)
            .Take(count)
            .ToList();
    }
}
