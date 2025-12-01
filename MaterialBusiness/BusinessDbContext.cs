using Microsoft.EntityFrameworkCore;
using MaterialBusiness;

public class BusinessDbContext : DbContext
{
    public DbSet<Fabric> Fabrics { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    // Parameterless constructor for migrations
    public BusinessDbContext()
    {
    }

    public BusinessDbContext(DbContextOptions<BusinessDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This ensures SQLite is configured even when using parameterless constructor
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=business.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Item inheritance (TPH - Table Per Hierarchy)
        modelBuilder.Entity<Item>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<Fabric>("Fabric");

        // Configure decimal precision
        modelBuilder.Entity<Fabric>()
            .Property(f => f.PricePerUnit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Fabric>()
            .Property(f => f.StockQuantity)
            .HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }
}