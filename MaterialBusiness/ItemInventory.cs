using System;
namespace MaterialBusiness
{
        public class InventoryItem
        {
            public Guid Id { get; private set; }
            public Guid ItemId { get; private set; }
            public Fabric ItemRef { get; private set; }

            // Could be: meters, sheets, rolls, kg, pieces
            public decimal Quantity { get; set; }

            public InventoryItem(Fabric item, decimal quantity)
            {
                Id = Guid.NewGuid();
                ItemId = item.Id;
                ItemRef = item;
                Quantity = quantity;
            }

            public override string ToString()
            {
                return $"{ItemRef.Name} — Qty: {Quantity}";
            }
        }
    }

