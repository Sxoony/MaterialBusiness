using System;
using MaterialBusiness;
using System.ComponentModel.DataAnnotations;
public class AuditLog
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    [Required]
    [MaxLength(100)]
    public string Action { get; set; }  // "StockAdded", "StockReduced", "ItemCreated"
    public Guid ItemId { get; set; }
    [MaxLength(200)]
    public string ItemName { get; set; }
    [MaxLength(500)]
    public string Details { get; set; }  // JSON or description of what changed
    public AuditLog() {
        Id = Guid.NewGuid();
        Timestamp = DateTime.Now;
    }
    public AuditLog(string action, Guid itemId, string itemName, string details)
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.Now;
        Action = action;
        ItemId = itemId;
        ItemName = itemName;
        Details = details;
    }
}
public class AuditRepository
{
    private readonly BusinessDbContext _context;

    public AuditRepository(BusinessDbContext context)
    {
        _context = context;
    }

    // Log an action
    public void Log(string action, Guid itemId, string itemName, string details)
    {
        var log = new AuditLog(action, itemId, itemName, details);
        _context.AuditLogs.Add(log);
        _context.SaveChanges();
    }

    // Get all logs for a specific item
    public IEnumerable<AuditLog> GetLogsByItem(Guid itemId)
    {
        return _context.AuditLogs
            .Where(l => l.ItemId == itemId)
            .OrderByDescending(l => l.Timestamp)
            .ToList();
    }

    public IEnumerable<AuditLog> GetAll()
    {
        return _context.AuditLogs
            .OrderByDescending(l => l.Timestamp)
            .ToList();
    }

    // Get recent audit logs
    public IEnumerable<AuditLog> GetRecent(int count = 50)
    {
        return _context.AuditLogs
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToList();
    }

    public IEnumerable<AuditLog> GetLogsByDateRange (DateTime start, DateTime end)
    {
        return _context.AuditLogs.
            Where(l=>l.Timestamp>=start&&l.Timestamp<=end).
            OrderByDescending(l=>l.Timestamp).
            ToList();
    }
}
