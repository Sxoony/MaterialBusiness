using Microsoft.EntityFrameworkCore;
using System;
namespace MaterialBusiness
{
	public class Business
    {
        private readonly BusinessDbContext _context;
        public string Name { get; set; }
		public string Address { get; set; }
		public ItemRepository Items { get; private set; }
		public AuditRepository AuditLog { get; private set; }
		public PromotionRepository Promotions { get; private set; }
        public OrderRepository Orders { get; private set; }

        public CalculationSystem Calculator { get; set; }




        public Business(string name, string address)
		{
            Name = name;
            Address = address;


            var optionsBuilder = new DbContextOptionsBuilder<BusinessDbContext>();
            optionsBuilder.UseSqlite("Data Source=business.db");
            _context = new BusinessDbContext(optionsBuilder.Options);

            // Ensure database exists (creates if missing)
            _context.Database.EnsureCreated();

            // Pass the SAME context to all repositories
            Items = new ItemRepository(_context);
            AuditLog = new AuditRepository(_context);
            Promotions = new PromotionRepository(_context);
            Orders = new OrderRepository(_context);
            Calculator = new CalculationSystem(Promotions,Items);
        }
        // Helper method to reduce stock with logging
        public void ReduceStock(Guid itemId, decimal quantity, string reason = "Sale")
        {
            var item = Items.Get(itemId);
            if (item == null) return;

            decimal oldQty = item.StockQuantity;
            item.StockQuantity -= quantity;

            // Save the change to database
            _context.SaveChanges();

            // Log it
            AuditLog.Log("StockReduced", itemId, item.Name,
                $"{reason}: Reduced from {oldQty} to {item.StockQuantity} (-{quantity})");
        }

        public void AddStock(Guid itemId, decimal quantity, string reason = "Restock")
        {
            var item = Items.Get(itemId);
            if (item == null) return;

            decimal oldQty = item.StockQuantity;
            item.StockQuantity += quantity;

            // Save the change
            _context.SaveChanges();

            // Log it
            AuditLog.Log("StockAdded", itemId, item.Name,
                $"{reason}: Increased from {oldQty} to {item.StockQuantity} (+{quantity})");
        }
      
    }
}