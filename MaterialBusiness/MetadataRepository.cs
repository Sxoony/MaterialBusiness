using System;
namespace MaterialBusiness
{
    public class MetadataRepository
    {
        // Key = MetadataRecord.Id
        private Dictionary<Guid, MetadataRecord> _metadata = new();

        // Secondary index: ItemId → List<MetadataRecord>
        private Dictionary<Guid, List<MetadataRecord>> _byItem = new();

        public void Add(MetadataRecord record)
        {
            _metadata[record.Id] = record;

            if (!_byItem.ContainsKey(record.ItemId))
                _byItem[record.ItemId] = new List<MetadataRecord>();

            _byItem[record.ItemId].Add(record);
        }

        public IEnumerable<MetadataRecord> GetByItem(Guid itemId)
        {
            if (_byItem.TryGetValue(itemId, out var list))
                return list;

            return Enumerable.Empty<MetadataRecord>();
        }

        public MetadataRecord? Get(Guid id)
        {
            return _metadata.TryGetValue(id, out var m) ? m : null;
        }

        public void Remove(Guid id)
        {
            if (_metadata.TryGetValue(id, out var record))
            {
                _metadata.Remove(id);

                if (_byItem.ContainsKey(record.ItemId))
                {
                    _byItem[record.ItemId].Remove(record);
                }
            }
        }

        public IEnumerable<MetadataRecord> GetAll()
        {
            return _metadata.Values;
        }
    }
}

