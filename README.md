## MaterialBusiness
A fabric and materials business management system for inventory, orders, pricing, and promotions.


## Description

MaterialBusiness is a C# console application designed to manage a fabric retail business. It handles product catalogs (fabrics with various types like rolls, sheets, and bulk items), inventory tracking with stock quantities, customer orders with dynamic pricing calculations including bulk discounts, and promotional campaigns. The system also includes audit logging to track all inventory changes and business transactions.
Features

Multi-type fabric management: Support for rolls, sheets, linear trims, and bulk fabric items
Dynamic pricing: Automated bulk discount calculations using exponential decay formulas (up to 35% off)
Inventory tracking: Real-time stock management with automatic updates on sales
Promotion system: Flexible discount engine supporting store-wide, product-specific, category-specific, minimum quantity, and minimum order amount promotions
Order processing: Complete order management with line items and price calculations
Audit logging: Comprehensive tracking of all inventory changes with timestamps and reasons
Flexible metadata: Key-value metadata storage on all items for custom properties

## Technologies

Language/Framework: C# / .NET 8
Libraries: System.Collections.Generic, System.Linq
Future: Entity Framework Core (planned for database integration)

## How to run (local)

Clone: git clone https://github.com/Sxoony/MaterialBusiness.git
Navigate: cd MaterialBusiness
Setup:

Open MaterialBusiness.sln in Visual Studio 2022 or later


## Run:

Press F5 or click "Start" in Visual Studio
Or run dotnet run from the project directory



## To Do

 
 Build **user interface** (console menu, later down the line transitioning to GUI)
 Add **input validation** (negative quantities, empty names, etc.)
 Implement **database integration** (SQLite + Entity Framework Core)
 Create sample data (if needed) and **test cases** (and possibly tests that verify correct business operations)
 Add reporting features (sales reports, low stock alerts) to create **business data**
 Manipulate business data to create **business intelligence** (from reports and logs as well as patterns or trends in that data)

 

## Author
Zamir Kruger

