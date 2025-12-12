module JsonAndDatabaseTests

open Expecto
open Product
open ProductDatabase
open JsonSerializer
open FileIO.FileOperations
open CartTypes
open System
open System.IO

// ============================================
// DATABASE TESTS
// ============================================

[<Tests>]
let databaseTests =
    testList "Database Tests" [
        
        testCase "Database initializes successfully and creates products table" <| fun _ ->
            // Initialize database (reuses existing if present)
            initializeDatabase()
            
            // Verify database file was created
            Expect.isTrue (File.Exists(Path.Combine("data", "products.db"))) "Database file should be created"
        
        testCase "Products loaded from database have correct data structure" <| fun _ ->
            // Load products from existing database
            let catalog = initializeCatalog()
            
            // Get first product
            match Map.tryFind 1 catalog with
            | Some product ->
                Expect.equal product.Id 1 "Product ID should be 1"
                Expect.equal product.Name "Chocolate" "Product name should be Chocolate"
                Expect.equal product.Price 15.00m "Product price should be 15.00"
                Expect.equal product.Category "Sweets" "Product category should be Sweets"
                Expect.equal product.Stock 10 "Product stock should be 10"
            | None ->
                failtest "Product with ID 1 should exist in database"
    ]

// ============================================
// JSON SERIALIZATION TESTS
// ============================================

[<Tests>]
let jsonTests =
    testList "JSON Serialization Tests" [
        
        testCase "Serialize product catalog to JSON" <| fun _ ->
            let catalog = initializeCatalog()
            
            let json = serializeProductCatalog catalog
            
            Expect.isNotEmpty json "JSON should not be empty"
            Expect.stringContains json "Chocolate" "JSON should contain product names"
            Expect.stringContains json "Biscuits" "JSON should contain multiple products"
        
        testCase "Save order to file creates JSON file successfully" <| fun _ ->
            let testProduct = {
                Id = 1
                Name = "Test Product"
                Price = 15.00m
                Description = "Test"
                Category = "Test"
                Stock = 10
            }
            
            let testEntry = {
                Product = testProduct
                Quantity = 3
            }
            
            let testOrder: Order = {
                OrderId = Guid.NewGuid().ToString()
                Items = [testEntry]
                Subtotal = 45.00m
                Discount = 0.00m
                Tax = 6.30m
                Shipping = 10.00m
                Total = 61.30m
                Timestamp = DateTime.Now
            }
            
            match saveOrder testOrder with
            | Ok filePath ->
                Expect.isTrue (File.Exists(filePath)) "Order file should be created"
                Expect.stringContains filePath ".json" "File should have .json extension"
                
                // Clean up test file
                try File.Delete(filePath) with | _ -> ()
            | Error msg ->
                failtest (sprintf "Should save order successfully: %s" msg)
    ]
