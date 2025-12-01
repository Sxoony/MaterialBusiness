using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
namespace MaterialBusiness
{
    public abstract class Item
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public decimal StockQuantity { get; set; }
        public string MetadataJson { get; set; } = "{}";

        // Simple key-value metadata, not mapped to DB directly
        [NotMapped]
        public Dictionary<string, string> Metadata
        {
            get => string.IsNullOrEmpty(MetadataJson)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(MetadataJson) ?? new();
            set => MetadataJson = JsonSerializer.Serialize(value); }

        protected Item(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StockQuantity = 0;
        }
        protected Item()
        {
            Id = Guid.NewGuid();
        }

        // Metadata helpers
        public void SetMetadata(string key, string value)
        {
            var meta = Metadata;
            meta[key] = value;
            Metadata= meta;
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
