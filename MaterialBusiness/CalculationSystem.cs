using MaterialBusiness;
using System;
using System.Linq;

public class CalculationSystem
{
    private readonly PromotionRepository _promotionRepo;
    private readonly InventoryRepository _inventoryRepo;

    public CalculationSystem(PromotionRepository promoRepo, InventoryRepository inventoryRepo)
    {
        _promotionRepo = promoRepo;
        _inventoryRepo = inventoryRepo;
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

            // Inventory update (when you want it active)
            // _inventoryRepo.ReduceStock(line.Item.Id, line.Quantity);
        }

        return finalTotal;
    }

    // BASE PRICE (fabric handles quantities + width + bulk)
    private decimal ComputeBasePrice(OrderLine line)
    {
        return line.Item.CalculatePrice(line.Quantity, line.Item.WidthInMeters);
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
