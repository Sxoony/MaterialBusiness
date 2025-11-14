using System;
namespace MaterialBusiness
{
    public class ItemRepository
    {
        private Dictionary<Guid, Fabric> _items = new(); //Dictionary of a collection of Item objects, which contain name, description, unit of measure,
                                                               //and other metadata (such as price per UOM)
        public void Add(Fabric item)
        {
            _items[item.Id] = item;
        }

        public Fabric? Get(Guid id)
        {
            return _items.TryGetValue(id, out var i) ? i : null;
        }

        public IEnumerable<Fabric> GetAll()
        {
            return _items.Values;
        }

        public void Remove(Guid id)
        {
            _items.Remove(id);
        }
    }
}

