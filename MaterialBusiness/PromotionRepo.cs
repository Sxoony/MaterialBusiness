using System;
using MaterialBusiness;

public class PromotionRepository
{
    private Dictionary<Guid, Promotion> _promotions = new();

    public void Add(Promotion promo)
    {
        _promotions[promo.Id] = promo;
    }

    public IEnumerable<Promotion> GetActivePromotions()
    {
        return _promotions.Values.Where(p => p.IsActive());
    }

    public Promotion? GetById(Guid id)
    {
        _promotions.TryGetValue(id, out var promo);
        return promo;
    }
}

