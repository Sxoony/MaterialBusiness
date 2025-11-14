# MaterialBusiness


A C# console-based prototype for a fully-fledged material shop management system.


## Description
This project models real-world fabric/material stores where items are sold by meter, sheet, roll, or unit, and pricing depends on:

width
unit of measurement
quantity
bulk discounts (economies of scale)
promotions
payment method (cash / store credit)
inventory updates

The aim is to create a production-ready foundation that can later be extended into a Windows Forms or web application.

## Project Goals
Build a generic, extensible domain model for a fabric/material shop

Create a calculation engine capable of computing:
base fabric price
bulk discounts
promotions
store credit adjustments
final order totals

Implement a clean repository pattern for:
inventory
metadata
promotions
Support full customer orders consisting of multiple items

Keep everything decoupled, testable, and ready for expansion

## Architecture Overview
1. Domain Layer
Contains the core models:
Fabric
  Base item of the shop
  Contains width, price-per-unit, unit of measure
  Implements bulk pricing via exponential discount model
  Self-contained CalculatePrice() method
Order
  Represents a customer order
  Contains multiple OrderLine items
  OrderLine
Represents a specific fabric + quantity combination

2. Repository Layer
InventoryRepository
  Tracks current stock levels
  Allows stock reduction on sales
PromotionRepository
  Holds active promotions
  Evaluates whether a promotion applies to the item
MetadataRepository
  Stores descriptive metadata (color, pattern, brand, etc.)
  Fully decoupled from pricing logic

4. Calculation Engine
CalculationSystem processes a full order:
  Computes base price from each Fabric
    Applies:
      bulk discounts
      promotions
      store credit adjustments (default 6 months)
  Returns final order total
  Optionally updates inventory
This engine is completely detached from UI concerns.
## Technologies
- Language / Framework: C#
- Libraries: list any major ones


## How to run (local)
1. Clone: `git clone https://github.com/Sxoony/MaterialBusiness.git`
2. Navigate: `cd MaterialBusiness`
3. Setup
- Open in Visual Studio (C#)
4. Run

## Author
Zamir Kruger
