using MaterialBusiness;
using System;
using System.Linq;

    static void Main(string[] args)
    {
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
