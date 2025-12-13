module SearchOperationsTests

open Expecto
open Product
open SearchOperations
open SearchTypes

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()
let allProducts = getAllProducts sampleCatalog

// ============================================
// SEARCH OPERATIONS TESTS
// ============================================

[<Tests>]
let searchOperationsTests =
    testList "Search Operations Tests" [
        
        testCase "Search by name finds matching products" <| fun _ ->
            let results = searchByName allProducts "Chocolate"
            Expect.isNonEmpty results "Should find Chocolate"
            Expect.all results (fun p -> p.Name.ToLower().Contains("chocolate")) 
                "All results should contain 'chocolate'"
        
        testCase "Filter by category returns only matching category" <| fun _ ->
            let results = filterByCategory allProducts "Snacks"
            Expect.isNonEmpty results "Should find Snacks"
            Expect.all results (fun p -> p.Category = "Snacks") 
                "All results should be in Snacks category"
        
        testCase "Apply search criteria filters correctly" <| fun _ ->
            let criteria = {
                SearchTerm = Some "cookie"
                Category = Some "Snacks"
                PriceRange = Some { MinPrice = 0m; MaxPrice = 15m }
                StockRange = None
                InStockOnly = true
            }
            let results = applySearchCriteria allProducts criteria
            // Should find Snacks with 'cookie' in name/description, price <= 15, in stock
            Expect.all results (fun p -> p.Category = "Snacks") "All should be Snacks"
            Expect.all results (fun p -> p.Price <= 15m) "All should be <= 15"
            Expect.all results (fun p -> p.Stock > 0) "All should be in stock"
    ]
