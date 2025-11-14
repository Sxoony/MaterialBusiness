using System;
namespace MaterialBusiness
{
	public abstract class Item
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public abstract decimal CalculateBasePrice();
		

		protected Item(string name, string description)
		{
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return $"{Name} (ID: {Id})";
		}
	}
}
