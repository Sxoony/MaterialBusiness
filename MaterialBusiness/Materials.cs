using System;
namespace MaterialBusiness
{
    public class Fabric
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Essential metadata
        public string MaterialType { get; set; }   // Cotton, Satin, Linen
        public decimal WidthInMeters { get; set; } // Applies to sheets, rolls, trims
        public string Color { get; set; }
        public decimal GSM { get; set; }           // optional, for fabrics
        public string UnitOfMeasure { get; set; }  // meter, sheet, roll, unit
        public enum FabricTypeEnum
        {
            Roll,
            Sheet,
            LinearTrim,
            Bulk
        }
        public FabricTypeEnum FabricType { get; set; }
        // Pricing
        public decimal PricePerUnit { get; set; }

        public Fabric(string name, FabricTypeEnum type)
        {
            Id = Guid.NewGuid();
            Name = name;
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

        public decimal CalculatePrice(decimal quantity, decimal width)
        {
            if (FabricType == FabricTypeEnum.Bulk)
            {
                decimal k = 0.02m;
                double q = (double)quantity;
                double kk =(double)k;
                decimal minMultiplier = 0.65m;
                decimal basePricePerUnit = PricePerUnit;
                double effectiveUnitPriceDouble = (double) basePricePerUnit * Math.Exp(-kk * (q - 1));

                decimal effectiveUnitPrice = (decimal)effectiveUnitPriceDouble;
                    
                effectiveUnitPrice= Math.Max(effectiveUnitPrice, basePricePerUnit * minMultiplier); ;
                decimal total = effectiveUnitPrice * width * quantity;
                return total;
            }
            return PricePerUnit * quantity * width;

        }
    }


}
