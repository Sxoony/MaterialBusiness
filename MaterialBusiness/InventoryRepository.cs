using System;
namespace MaterialBusiness
{
    public class InventoryRepository
    {
        private Dictionary<Guid, InventoryItem> _inventory = new();
        private Dictionary<Guid, InventoryItem> _byItemId = new();

        public void Add(InventoryItem inv)
        {
            _inventory[inv.Id] = inv;
            _byItemId[inv.ItemId] = inv;
        }

        public InventoryItem? Get(Guid id)
        {
            return _inventory.TryGetValue(id, out var item) ? item : null;
        }

        public InventoryItem? GetByItemId(Guid itemId)
        {
            return _byItemId.TryGetValue(itemId, out var inv) ? inv : null;
        }

        public IEnumerable<InventoryItem> GetAll()
        {
            return _inventory.Values;
        }

        public void Remove(Guid id)
        {
            if (_inventory.TryGetValue(id, out var inv))
            {
                _inventory.Remove(id);
                _byItemId.Remove(inv.ItemId);
            }
        }
    }
}
