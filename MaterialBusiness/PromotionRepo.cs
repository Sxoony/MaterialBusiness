using System;
using MaterialBusiness;

public class PromotionRepository
{
    private readonly BusinessDbContext _context;

    public PromotionRepository(BusinessDbContext context)
    {
        _context = context;
    }

    public void Add(Promotion promo)
    {
        _context.Promotions.Add(promo);
        _context.SaveChanges();
    }

    // Only returns promotions that are currently active (date range check)
    public IEnumerable<Promotion> GetActivePromotions()
    {
        var now = DateTime.Now;
        return _context.Promotions
            .Where(p => p.StartDate <= now && p.EndDate >= now)
            .ToList();
    }

    public Promotion? GetById(Guid id)
    {
        return _context.Promotions.Find(id);
    }

    public IEnumerable<Promotion> GetAll()
    {
        return _context.Promotions.ToList();
    }

    public void Remove(Guid id)
    {
        var promo = GetById(id);
        if (promo != null)
        {
            _context.Promotions.Remove(promo);
            _context.SaveChanges();
        }
    }
}

