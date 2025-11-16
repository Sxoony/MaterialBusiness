using System;
namespace MaterialBusiness
{
	public class Business
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public ItemRepository Items { get; private set; }
		public AuditRepository AuditLog { get; private set; }
		public PromotionRepository Promotions { get; private set; }


        //TO DO
        public object Calculator { get; set; }
		public List<Order> Orders { get; set; }


		public Business(string name, string address)
		{
            Name = name;
            Address = address;
            Items = new ItemRepository();
            AuditLog = new AuditRepository();
            Promotions = new PromotionRepository();
        }
        // Helper method to reduce stock with logging
        public void ReduceStock(Guid itemId, decimal quantity, string reason = "Sale")
        {
            var item = Items.Get(itemId);
            if (item == null) return;

            decimal oldQty = item.StockQuantity;
            item.StockQuantity -= quantity;

            AuditLog.Log("StockReduced", itemId, item.Name,
                $"{reason}: Reduced from {oldQty} to {item.StockQuantity} (-{quantity})");
        }

        public void AddStock(Guid itemId, decimal quantity, string reason = "Restock")
        {
            var item = Items.Get(itemId);
            if (item == null) return;

            decimal oldQty = item.StockQuantity;
            item.StockQuantity += quantity;

            AuditLog.Log("StockAdded", itemId, item.Name,
                $"{reason}: Increased from {oldQty} to {item.StockQuantity} (+{quantity})");
        }
    }
}