using MaterialBusiness;
using System;
using System.Collections.Generic;

public class DataSeeder
{
    private readonly Business _business;
    private Random _random = new Random();

    public DataSeeder(Business business)
    {
        _business = business;
    }

    public void SeedAll()
    {
        Console.WriteLine("Seeding database with dummy data...\n");

        var fabrics = SeedFabrics();
        SeedPromotions();
        SeedOrders(fabrics);
        SeedStockMovements(fabrics);

        Console.WriteLine("\nDatabase seeding complete!");
    }

    private List<Fabric> SeedFabrics()
    {
        Console.WriteLine("Creating fabrics...");

        var fabrics = new List<Fabric>();

        // Cotton fabrics
        var cottons = new[]
        {
            ("Premium Egyptian Cotton", "White", 120m, 450m),
            ("Organic Cotton Blend", "Beige", 95m, 380m),
            ("Cotton Poplin", "Blue", 85m, 320m),
            ("Heavy Cotton Canvas", "Natural", 110m, 500m),
            ("Soft Cotton Jersey", "Grey", 75m, 280m)
        };

        foreach (var (name, color, price, stock) in cottons)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.Roll);
            fabric.MaterialType = "Cotton";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 1.5m;
            fabric.GSM = 150m;
            fabric.SetMetadata("Supplier", GetRandomSupplier());
            fabric.SetMetadata("Origin", GetRandomOrigin());
            fabric.SetMetadata("CareInstructions", "Machine wash cold, tumble dry low");

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        // Satin fabrics
        var satins = new[]
        {
            ("Luxe Silk Satin", "Ivory", 180m, 200m),
            ("Polyester Satin", "Black", 65m, 350m),
            ("Bridal Satin", "White", 145m, 180m),
            ("Duchess Satin", "Royal Blue", 160m, 220m)
        };

        foreach (var (name, color, price, stock) in satins)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.Roll);
            fabric.MaterialType = "Satin";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 1.4m;
            fabric.GSM = 120m;
            fabric.SetMetadata("Supplier", GetRandomSupplier());
            fabric.SetMetadata("Origin", GetRandomOrigin());
            fabric.SetMetadata("CareInstructions", "Dry clean only");

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        // Linen fabrics
        var linens = new[]
        {
            ("Pure Linen", "Natural", 135m, 300m),
            ("Linen Blend", "Stone", 95m, 280m),
            ("Heavy Linen", "Charcoal", 155m, 190m)
        };

        foreach (var (name, color, price, stock) in linens)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.Roll);
            fabric.MaterialType = "Linen";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 1.5m;
            fabric.GSM = 180m;
            fabric.SetMetadata("Supplier", GetRandomSupplier());
            fabric.SetMetadata("Origin", "Europe");
            fabric.SetMetadata("CareInstructions", "Machine wash gentle, air dry");

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        // Sheet fabrics
        var sheets = new[]
        {
            ("Cotton Sheet 2m x 2m", "White", 45m, 150m),
            ("Felt Sheet 1m x 1m", "Red", 12m, 300m),
            ("Canvas Sheet 1.5m x 1.5m", "Natural", 28m, 200m)
        };

        foreach (var (name, color, price, stock) in sheets)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.Sheet);
            fabric.MaterialType = "Various";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 2m;
            fabric.SetMetadata("Supplier", GetRandomSupplier());

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        // Trims (LinearTrim)
        var trims = new[]
        {
            ("Satin Ribbon", "Gold", 8m, 500m),
            ("Lace Trim", "White", 15m, 400m),
            ("Grosgrain Ribbon", "Navy", 6m, 600m),
            ("Elastic Band", "Black", 4m, 800m)
        };

        foreach (var (name, color, price, stock) in trims)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.LinearTrim);
            fabric.MaterialType = "Trim";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 0.05m; // 5cm width
            fabric.SetMetadata("Supplier", GetRandomSupplier());

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        // Bulk items
        var bulkItems = new[]
        {
            ("Thread Spools (pack of 10)", "Assorted", 25m, 100m),
            ("Buttons Mixed Box", "Assorted", 18m, 150m),
            ("Zippers Bulk Pack (50pc)", "Black", 35m, 80m),
            ("Fabric Scissors", "N/A", 45m, 60m)
        };

        foreach (var (name, color, price, stock) in bulkItems)
        {
            var fabric = new Fabric(name, Fabric.FabricTypeEnum.Bulk);
            fabric.MaterialType = "Supplies";
            fabric.Color = color;
            fabric.PricePerUnit = price;
            fabric.StockQuantity = stock;
            fabric.LengthInMeters = 1m;
            fabric.SetMetadata("Supplier", GetRandomSupplier());

            _business.Items.Add(fabric);
            fabrics.Add(fabric);
            Console.WriteLine($"  ✓ {name}");
        }

        return fabrics;
    }

    private void SeedPromotions()
    {
        Console.WriteLine("\nCreating promotions...");

        // Store-wide sale
        var storewidePromo = new Promotion
        {
            Name = "New Year Sale",
            DiscountPercent = 15,
            StartDate = DateTime.Now.AddDays(-30),
            EndDate = DateTime.Now.AddDays(30),
            ConditionType = Promotion.PromotionConditionType.Storewide
        };
        _business.Promotions.Add(storewidePromo);
        Console.WriteLine($"  ✓ {storewidePromo.Name}");

        // Category-specific
        var satinPromo = new Promotion
        {
            Name = "Satin Special",
            DiscountPercent = 25,
            StartDate = DateTime.Now.AddDays(-15),
            EndDate = DateTime.Now.AddDays(15),
            ConditionType = Promotion.PromotionConditionType.CategorySpecific,
            Category = Fabric.FabricTypeEnum.Roll
        };
        _business.Promotions.Add(satinPromo);
        Console.WriteLine($"  ✓ {satinPromo.Name}");

        // Bulk discount
        var bulkPromo = new Promotion
        {
            Name = "Buy More Save More",
            DiscountPercent = 20,
            StartDate = DateTime.Now.AddDays(-20),
            EndDate = DateTime.Now.AddDays(40),
            ConditionType = Promotion.PromotionConditionType.MinimumQuantity,
            MinimumQuantity = 50
        };
        _business.Promotions.Add(bulkPromo);
        Console.WriteLine($"  ✓ {bulkPromo.Name}");

        // Minimum order amount
        var orderPromo = new Promotion
        {
            Name = "Spend $500 Get 10% Off",
            DiscountPercent = 10,
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(50),
            ConditionType = Promotion.PromotionConditionType.MinimumOrderAmount,
            MinimumOrderAmount = 500m
        };
        _business.Promotions.Add(orderPromo);
        Console.WriteLine($"  ✓ {orderPromo.Name}");

        // Expired promotion (for historical data)
        var expiredPromo = new Promotion
        {
            Name = "Black Friday Blowout",
            DiscountPercent = 30,
            StartDate = DateTime.Now.AddDays(-90),
            EndDate = DateTime.Now.AddDays(-60),
            ConditionType = Promotion.PromotionConditionType.Storewide
        };
        _business.Promotions.Add(expiredPromo);
        Console.WriteLine($"  ✓ {expiredPromo.Name} (expired)");
    }

    private void SeedOrders(List<Fabric> fabrics)
    {
        Console.WriteLine("\nCreating orders...");

        // Create 20 orders with random dates over the past 3 months
        for (int i = 1; i <= 20; i++)
        {
            var order = new Order();

            // Random number of items per order (1-5)
            int itemCount = _random.Next(1, 6);

            for (int j = 0; j < itemCount; j++)
            {
                var randomFabric = fabrics[_random.Next(fabrics.Count)];
                decimal quantity = _random.Next(5, 51); // 5-50 units

                order.AddItem(randomFabric, quantity);

                // Reduce stock
                _business.ReduceStock(randomFabric.Id, quantity, $"Order #{i}");
            }

            _business.Orders.Add(order);
            Console.WriteLine($"  ✓ Order #{i} with {itemCount} items");
        }
    }

    private void SeedStockMovements(List<Fabric> fabrics)
    {
        Console.WriteLine("\nCreating stock movements...");

        // Simulate restocks over the past month
        for (int i = 0; i < 15; i++)
        {
            var randomFabric = fabrics[_random.Next(fabrics.Count)];
            decimal quantity = _random.Next(50, 201); // 50-200 units

            _business.AddStock(randomFabric.Id, quantity, "Supplier Delivery");
            Console.WriteLine($"  ✓ Restocked {randomFabric.Name}: +{quantity}");
        }

        // Simulate some returns
        for (int i = 0; i < 5; i++)
        {
            var randomFabric = fabrics[_random.Next(fabrics.Count)];
            decimal quantity = _random.Next(5, 21); // 5-20 units

            _business.AddStock(randomFabric.Id, quantity, "Customer Return");
            Console.WriteLine($"  ✓ Return processed for {randomFabric.Name}: +{quantity}");
        }

        // Simulate some damaged goods
        for (int i = 0; i < 3; i++)
        {
            var randomFabric = fabrics[_random.Next(fabrics.Count)];
            decimal quantity = _random.Next(1, 11); // 1-10 units

            _business.ReduceStock(randomFabric.Id, quantity, "Damaged/Defective");
            Console.WriteLine($"  ✓ Damaged goods removed for {randomFabric.Name}: -{quantity}");
        }
    }

    private string GetRandomSupplier()
    {
        string[] suppliers =
        {
            "Acme Textiles",
            "Global Fabrics Co",
            "Premier Materials Ltd",
            "Textile World Inc",
            "Quality Fabrics Direct",
            "Continental Textiles"
        };
        return suppliers[_random.Next(suppliers.Length)];
    }

    private string GetRandomOrigin()
    {
        string[] origins =
        {
            "Egypt",
            "Turkey",
            "India",
            "China",
            "Italy",
            "USA",
            "Portugal"
        };
        return origins[_random.Next(origins.Length)];
    }
}