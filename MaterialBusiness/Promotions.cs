using MaterialBusiness;
using static MaterialBusiness.Fabric;
using System;
using System.Collections.Generic;
using System.Linq;
public class Promotion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public decimal DiscountPercent { get; set; } // e.g. 10 = 10% off
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public enum PromotionConditionType
{
    Storewide,
    ProductSpecific,
    CategorySpecific,
    MinimumQuantity,
    MinimumOrderAmount
}

    // Conditions
    public PromotionConditionType ConditionType { get; set; }
    public List<Guid>? ProductIds { get; set; } = new();      // For product-specific promos
    public FabricTypeEnum? Category { get; set; }             // For category promotions
    public int? MinimumQuantity { get; set; }                 // For bulk-type promos
    public decimal? MinimumOrderAmount { get; set; }          // Order-total conditions

    public bool IsActive()
    {
        var now = DateTime.Now;
        return now >= StartDate && now <= EndDate;
    }

    public bool AppliesTo(Fabric fabric, decimal quantity, decimal orderTotal)
    {
        if (!IsActive())
            return false;
        switch (ConditionType)
        {
            case PromotionConditionType.Storewide: return true; break;
            case PromotionConditionType.ProductSpecific: return ProductIds != null && ProductIds.Contains(fabric.Id);break;
            case PromotionConditionType.CategorySpecific: return Category != null && fabric.FabricType == Category; break;
            case PromotionConditionType.MinimumQuantity: return MinimumQuantity != null && quantity >= MinimumQuantity; break;
            case PromotionConditionType.MinimumOrderAmount: return MinimumOrderAmount != null && orderTotal >= MinimumOrderAmount; break;
                default: return false; break;
        }
    }

    public decimal Apply(decimal price)
    {
        return price - (price * (DiscountPercent / 100m));
    }
}
