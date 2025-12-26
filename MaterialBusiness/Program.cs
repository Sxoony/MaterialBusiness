using MaterialBusiness;
using System;
using System.Linq;
class Program {
  static  Business localBusiness = new Business("Local Fabrics", "123 Main St");

class Program
{
    static void Main(string[] args)
    {
       
        Console.WriteLine($"===Welcome to {localBusiness.Name}, on {localBusiness.Address}!===\n");
       bool finished = false;
        while (!finished)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Fabric Queries");
            Console.WriteLine("2. Order Queries");
            Console.WriteLine("3. Promotion Queries");
            Console.WriteLine("4. Audit Log Queries");
            Console.WriteLine("5. Exit");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    FabricQueries();
                    break;
                case "2":
                    // Implement OrderQueries();
                    break;
                case "3":
                    // Implement PromotionQueries();
                    break;
                case "4":
                    // Implement AuditLogQueries();
                    break;
                case "5":
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }

            //ORDER QUERIES




            //PROMOTION QUERIES



            //AUDIT LOG QUERIES



        }

        static void FabricQueries(){
           bool finished = false;
            while (!finished)
            {
                Console.WriteLine("1. List all fabrics");
                Console.WriteLine("2. Add stock to a fabric");
                Console.WriteLine("3. Reduce stock of a fabric");
                Console.WriteLine("4. Find a specific fabric");
                Console.WriteLine("5. View audit log");
                Console.WriteLine("6. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        var fabrics = localBusiness.Items.GetAllFabrics();
                        Console.WriteLine("\n--- Fabrics ---");
                        Console.WriteLine("Name\tID\tStock");
                        foreach (var fabric in fabrics)
                        {
                            Console.WriteLine($"{fabric.Name}\t(ID: {fabric.Id})\tStock: {fabric.StockQuantity}");
                        }
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.Write("Enter Fabric ID to add stock: ");
                        Guid addId = Guid.Parse(Console.ReadLine() ?? "");
                        Console.Write("Enter quantity to add: ");
                        decimal addQty = decimal.Parse(Console.ReadLine() ?? "0");
                        localBusiness.AddStock(addId, addQty, "Manual Restock");
                        Console.WriteLine("Stock added.\n");
                        break;
                    case "3":
                        Console.Write("Enter Fabric ID to reduce stock: ");
                        Guid reduceId = Guid.Parse(Console.ReadLine() ?? "");
                        Console.Write("Enter quantity to reduce: ");
                        decimal reduceQty = decimal.Parse(Console.ReadLine() ?? "0");
                        localBusiness.ReduceStock(reduceId, reduceQty, "Manual Sale");
                        Console.WriteLine("Stock reduced.\n");
                        break;
                    case "4":
                        Console.Write("Enter Fabric ID to find: ");
                        Guid findId = Guid.Parse(Console.ReadLine() ?? "");
                        var fabricFound = localBusiness.Items.Get(findId);
                        if (fabricFound != null)
                        {
                            Console.WriteLine($"\nFound Fabric: {fabricFound.Name} (ID: {fabricFound.Id}) - Stock: {fabricFound.StockQuantity}\n");
                        }
                        else
                        {
                            Console.WriteLine("Fabric not found.\n");
                        }
                        break;
                    case "5":
                        var logs = localBusiness.AuditLog.GetAll();
                        Console.WriteLine("\n--- Audit Log ---");
                        foreach (var log in logs)
                        {
                            Console.WriteLine($"{log.Timestamp}: {log.Action} on Item {log.ItemName} - {log.Details}");
                        }
                        Console.WriteLine();
                        break;
                    case "6":
                        finished = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }
    }
        Business business = new Business("Fabric Emporium", "123 Main St");
       
            Console.WriteLine("=== MaterialBusiness - Database Setup ===\n");

            // Check if database is already seeded
            var existingItems = business.Items.GetAll();

            if (!existingItems.Any())
            {
                Console.WriteLine("Database is empty. Seeding with dummy data...\n");

                var seeder = new DataSeeder(business);
                seeder.SeedAll();
            }
            else
            {
                Console.WriteLine($"Database already contains {existingItems.Count()} items.");
                Console.WriteLine("Do you want to clear and reseed? (y/n)");

                var response = Console.ReadLine()?.ToLower();

                if (response == "y")
                {
                    Console.WriteLine("\nClearing existing data...");
                    // You'd need to add methods to clear data, or just delete business.db and restart
                    Console.WriteLine("Please delete business.db manually and restart the application.");
                    return;
                }
            }
        }

    }
