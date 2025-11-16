using System;
namespace MaterialBusiness
{
    public class ItemRepository
    {
        private Dictionary<Guid, Item> _items = new(); // Changed from Fabric to Item

        public void Add(Item item)
        {
            _items[item.Id] = item;
        }

        public Item? Get(Guid id)
        {
            return _items.TryGetValue(id, out var i) ? i : null;
        }

        public IEnumerable<Item> GetAll()
        {
            return _items.Values;
        }

        // Get specific types
        public IEnumerable<Fabric> GetAllFabrics()
        {
            return _items.Values.OfType<Fabric>();
        }
    }
}

