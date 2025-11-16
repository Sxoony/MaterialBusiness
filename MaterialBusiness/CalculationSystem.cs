using MaterialBusiness;
using System;
using System.Linq;

public class CalculationSystem
{
    private readonly PromotionRepository _promotionRepo;
    private readonly ItemRepository _itemRepo;

    public CalculationSystem(PromotionRepository promoRepo, ItemRepository itemRepo)
    {
        _promotionRepo = promoRepo;
        _itemRepo = itemRepo;
    }

    // MAIN ORDER CALCULATION ENTRY
    public decimal ProcessOrder(Order order, bool onCredit = false)
    {
        decimal finalTotal = 0;

        foreach (var line in order.Lines)
        {
            decimal lineBase = ComputeBasePrice(line);
            decimal lineFinal = ApplyAdjustments(line, lineBase, onCredit);

            finalTotal += lineFinal;

            var item = _itemRepo.Get(line.Item.Id);
            if (item != null)
            {
                item.StockQuantity -= line.Quantity;
            }
        }

        return finalTotal;
    }

    // BASE PRICE (fabric handles quantities + width + bulk)
    private decimal ComputeBasePrice(OrderLine line)
    {
        return line.Item.CalculatePrice(line.Quantity, line.Item.LengthInMeters);
    }

    // DISCOUNTS, PROMOTIONS, CREDIT, ETC.
    private decimal ApplyAdjustments(OrderLine line, decimal basePrice, bool onCredit)
    {
        decimal final = basePrice;

        // Apply promotions
        var promos = _promotionRepo.GetActivePromotions();
        foreach (var promo in promos)
        {
            if (promo.AppliesTo(line.Item, line.Quantity, basePrice))
            {
                final = promo.Apply(final);
            }
        }

        // Apply credit payment adjustment (if needed)
        if (onCredit)
        {
            final = final / 6;  // default 6 months
        }

        return final;
    }
}
