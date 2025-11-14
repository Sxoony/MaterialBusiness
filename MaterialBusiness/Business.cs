using System;
namespace MaterialBusiness
{
	public class Business
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public ItemRepository Items { get; private set; }
		public InventoryRepository Inventory { get; private set; }
		public MetadataRepository Metadata { get; private set; }

		//TO DO
		public object Calculator { get; set; }
		public List<Order> Orders { get; set; }


		public Business(string name, string address)
		{
			Name = name;
			Address = address;
			Items = new ItemRepository();
			Inventory = new InventoryRepository();
			Metadata = new MetadataRepository();
		}
	}
}