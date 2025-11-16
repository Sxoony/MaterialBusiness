using System;
using MaterialBusiness;
public class AuditLog
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }  // "StockAdded", "StockReduced", "ItemCreated"
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public string Details { get; set; }  // JSON or description of what changed

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
    private List<AuditLog> _logs = new();

    public void Log(string action, Guid itemId, string itemName, string details)
    {
        var log = new AuditLog(action, itemId, itemName, details);
        _logs.Add(log);

        // Optionally: write to file
        // File.AppendAllText("audit.log", $"{log.Timestamp}: {action} - {details}\n");
    }

    public IEnumerable<AuditLog> GetLogsByItem(Guid itemId)
    {
        return _logs.Where(l => l.ItemId == itemId);
    }

    public IEnumerable<AuditLog> GetAll()
    {
        return _logs;
    }
}
