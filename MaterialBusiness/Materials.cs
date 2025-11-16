using System;
namespace MaterialBusiness
{
    public class Fabric : Item
    {
        // Essential metadata
        public string MaterialType { get; set; }
        public decimal LengthInMeters { get; set; }
        public string Color { get; set; }
        public decimal GSM { get; set; }
        public string UnitOfMeasure { get; set; }
       
        public enum FabricTypeEnum
        {
            Roll,
            Sheet,
            LinearTrim,
            Bulk
        }
        public FabricTypeEnum FabricType { get; set; }
        public decimal PricePerUnit { get; set; }

        public Fabric(string name, FabricTypeEnum type) : base(name, "")
        {
            switch (type)
            {
                case FabricTypeEnum.Roll:
                    UnitOfMeasure = "roll";
                    FabricType = FabricTypeEnum.Roll;
                    break;
                case FabricTypeEnum.Sheet:
                    UnitOfMeasure = "sheet";
                    FabricType = FabricTypeEnum.Sheet;
                    break;
                case FabricTypeEnum.LinearTrim:
                    UnitOfMeasure = "meter";
                    FabricType = FabricTypeEnum.LinearTrim;
                    break;
                case FabricTypeEnum.Bulk:
                    UnitOfMeasure = "unit";
                    FabricType = FabricTypeEnum.Bulk;
                    break;
                default:
                    UnitOfMeasure = "unit";
                    break;
            }
        }

        public decimal CalculatePrice(decimal quantity, decimal additionalParam = 1)
        {
            if (FabricType == FabricTypeEnum.Bulk)
            {
                // Your new economies of scale formula here
                decimal maxDiscountPercent = 0.35m;
                decimal minPriceMultiplier = 1m - maxDiscountPercent;
                decimal discountRate = 0.02m;

                double quantityDouble = (double)quantity;
                double discountRateDouble = (double)discountRate;

                double discountFactor = 1.0 - Math.Exp(-discountRateDouble * (quantityDouble - 1));
                decimal discountPercent = Math.Min((decimal)discountFactor, maxDiscountPercent);

                decimal discountedUnitPrice = PricePerUnit * (1m - discountPercent);
                discountedUnitPrice = Math.Max(discountedUnitPrice, PricePerUnit * minPriceMultiplier);

                return discountedUnitPrice * quantity;
            }

            return PricePerUnit * quantity * LengthInMeters;
        }
    }


}
