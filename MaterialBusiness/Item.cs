using System;
namespace MaterialBusiness
{
    public abstract class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StockQuantity { get; set; }

        // Simple key-value metadata
        public Dictionary<string, string> Metadata { get; private set; } = new();

        protected Item(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StockQuantity = 0;
        }


        // Metadata helpers
        public void SetMetadata(string key, string value)
        {
            Metadata[key] = value;
        }

        public string? GetMetadata(string key)
        {
           return Metadata.TryGetValue(key, out var value) ? value : null;
        }

        public override string ToString()
        {
            return $"{Name} (ID: {Id})";
        }
    }
}
