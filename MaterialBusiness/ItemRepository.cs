using System;
using System.Collections.Generic;
using System.Linq;
using MaterialBusiness;

namespace MaterialBusiness
{
    public class ItemRepository
    {
        private readonly BusinessDbContext _context;

        // Constructor now takes DbContext instead of creating a Dictionary
        public ItemRepository(BusinessDbContext context)
        {
            _context = context;
        }

        // Add: Saves to database instead of Dictionary
        public void Add(Fabric item)
        {
            _context.Fabrics.Add(item);  // Tell EF to track this item
            _context.SaveChanges();       // Actually write to database file
        }

        // Get: Queries database instead of Dictionary lookup
        public Fabric? Get(Guid id)
        {
            return _context.Fabrics.Find(id);  // SELECT * FROM Fabrics WHERE Id = @id
        }

        // GetAll: Reads all records from database
        public IEnumerable<Fabric> GetAll()
        {
            return _context.Fabrics.ToList();  // SELECT * FROM Fabrics
        }

        // Remove: Deletes from database
        public void Remove(Guid id)
        {
            var item = Get(id);
            if (item != null)
            {
                _context.Fabrics.Remove(item);  // Mark for deletion
                _context.SaveChanges();          // Actually delete from file
            }
        }

        public IEnumerable<Fabric> GetAllFabrics()
        {
            return _context.Fabrics.ToList();
        }
    }
}