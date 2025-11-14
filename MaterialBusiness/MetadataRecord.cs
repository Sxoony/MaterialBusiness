using System;
namespace MaterialBusiness
{
    public class MetadataRecord
    {
        public Guid Id { get; private set; }
        public Guid ItemId { get; private set; } //Links to fabric
        public string Key { get; private set; } // e.g., "Width", "Color"
        public string Value { get; private set; } 

        public MetadataRecord(Guid itemId, string key, string value)
        {
            Id = Guid.NewGuid();
            ItemId = itemId;
            Key = key;
            Value = value;
        }
    }
}
