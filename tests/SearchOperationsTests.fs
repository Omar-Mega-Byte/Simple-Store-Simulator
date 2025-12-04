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
        
        testCase "Search by name is case insensitive" <| fun _ ->
            let results1 = searchByName allProducts "chocolate"
            let results2 = searchByName allProducts "CHOCOLATE"
            let results3 = searchByName allProducts "ChOcOlAtE"
            Expect.equal (List.length results1) (List.length results2) "Case should not matter"
            Expect.equal (List.length results2) (List.length results3) "Case should not matter"
        
        testCase "Search by name with empty string returns all products" <| fun _ ->
            let results = searchByName allProducts ""
            Expect.equal (List.length results) (List.length allProducts) 
                "Empty search should return all products"
        
        testCase "Search by name with no matches returns empty list" <| fun _ ->
            let results = searchByName allProducts "NonExistentProduct123"
            Expect.isEmpty results "Should return empty list for no matches"
        
        testCase "Search by name finds partial matches" <| fun _ ->
            let results = searchByName allProducts "Choc"
            Expect.isNonEmpty results "Should find products with 'Choc' in name"
        
        testCase "Search by description finds matching products" <| fun _ ->
            let results = searchByDescription allProducts "fresh"
            Expect.isNonEmpty results "Should find products with 'fresh' in description"
            Expect.all results (fun p -> p.Description.ToLower().Contains("fresh")) 
                "All results should contain 'fresh' in description"
        
        testCase "Search by name or description finds more results" <| fun _ ->
            let nameResults = searchByName allProducts "milk"
            let descResults = searchByDescription allProducts "milk"
            let combinedResults = searchByNameOrDescription allProducts "milk"
            Expect.isGreaterThanOrEqual (List.length combinedResults) (List.length nameResults) 
                "Combined search should find at least as many as name search"
            Expect.isGreaterThanOrEqual (List.length combinedResults) (List.length descResults) 
                "Combined search should find at least as many as description search"
        
        testCase "Filter by category returns only matching category" <| fun _ ->
            let results = filterByCategory allProducts "Snacks"
            Expect.isNonEmpty results "Should find Snacks"
            Expect.all results (fun p -> p.Category = "Snacks") 
                "All results should be in Snacks category"
        
        testCase "Filter by category is case insensitive" <| fun _ ->
            let results1 = filterByCategory allProducts "snacks"
            let results2 = filterByCategory allProducts "SNACKS"
            Expect.equal (List.length results1) (List.length results2) 
                "Category filter should be case insensitive"
        
        testCase "Filter by category with empty string returns all" <| fun _ ->
            let results = filterByCategory allProducts ""
            Expect.equal (List.length results) (List.length allProducts) 
                "Empty category should return all"
        
        testCase "Filter by price range returns products in range" <| fun _ ->
            let results = filterByPriceRange allProducts 10m 20m
            Expect.isNonEmpty results "Should find products in range 10-20"
            Expect.all results (fun p -> p.Price >= 10m && p.Price <= 20m) 
                "All products should be in price range"
        
        testCase "Filter by price range includes boundaries" <| fun _ ->
            let results = filterByPriceRange allProducts 15m 15m
            Expect.isNonEmpty results "Should find products at exact price"
            Expect.all results (fun p -> p.Price = 15m) 
                "All products should be exactly 15"
        
        testCase "Filter by price range type works correctly" <| fun _ ->
            let priceRange = { MinPrice = 10m; MaxPrice = 30m }
            let results = filterByPriceRangeType allProducts priceRange
            Expect.all results (fun p -> p.Price >= 10m && p.Price <= 30m) 
                "All products should be in range"
        
        testCase "Filter by minimum price excludes lower prices" <| fun _ ->
            let results = filterByMinPrice allProducts 50m
            Expect.all results (fun p -> p.Price >= 50m) 
                "All products should be >= 50"
        
        testCase "Filter by maximum price excludes higher prices" <| fun _ ->
            let results = filterByMaxPrice allProducts 20m
            Expect.all results (fun p -> p.Price <= 20m) 
                "All products should be <= 20"
        
        testCase "Filter by stock range returns products in range" <| fun _ ->
            let results = filterByStockRange allProducts 10 20
            Expect.all results (fun p -> p.Stock >= 10 && p.Stock <= 20) 
                "All products should have stock in range"
        
        testCase "Filter by stock range type works correctly" <| fun _ ->
            let stockRange = { MinStock = 15; MaxStock = 30 }
            let results = filterByStockRangeType allProducts stockRange
            Expect.all results (fun p -> p.Stock >= 15 && p.Stock <= 30) 
                "All products should have stock in range"
        
        testCase "Filter in stock only excludes out of stock" <| fun _ ->
            let results = filterInStockOnly allProducts
            Expect.all results (fun p -> p.Stock > 0) 
                "All products should have stock > 0"
        
        testCase "Sort by price ascending orders correctly" <| fun _ ->
            let sorted = sortByPrice allProducts true
            let prices = sorted |> List.map (fun p -> p.Price)
            let sortedPrices = prices |> List.sort
            Expect.equal prices sortedPrices "Prices should be in ascending order"
        
        testCase "Sort by price descending orders correctly" <| fun _ ->
            let sorted = sortByPrice allProducts false
            let prices = sorted |> List.map (fun p -> p.Price)
            let sortedPrices = prices |> List.sortDescending
            Expect.equal prices sortedPrices "Prices should be in descending order"
        
        testCase "Sort by name ascending orders alphabetically" <| fun _ ->
            let sorted = sortByName allProducts true
            let names = sorted |> List.map (fun p -> p.Name)
            let sortedNames = names |> List.sort
            Expect.equal names sortedNames "Names should be in alphabetical order"
        
        testCase "Sort by name descending orders reverse alphabetically" <| fun _ ->
            let sorted = sortByName allProducts false
            let names = sorted |> List.map (fun p -> p.Name)
            let sortedNames = names |> List.sortDescending
            Expect.equal names sortedNames "Names should be in reverse alphabetical order"
        
        testCase "Get categories returns unique categories" <| fun _ ->
            let categories = getCategories allProducts
            let uniqueCategories = categories |> List.distinct
            Expect.equal (List.length categories) (List.length uniqueCategories) 
                "Categories should be unique"
        
        testCase "Get categories includes known categories" <| fun _ ->
            let categories = getCategories allProducts
            Expect.contains categories "Snacks" "Should include Snacks category"
            Expect.contains categories "Dairy" "Should include Dairy category"
            Expect.contains categories "Sweets" "Should include Sweets category"
        
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
        
        testCase "Apply search criteria with no filters returns all" <| fun _ ->
            let criteria = {
                SearchTerm = None
                Category = None
                PriceRange = None
                StockRange = None
                InStockOnly = false
            }
            let results = applySearchCriteria allProducts criteria
            Expect.equal (List.length results) (List.length allProducts) 
                "Should return all products with no filters"
        
        testCase "Apply search criteria filters and searches" <| fun _ ->
            let criteria = {
                SearchTerm = Some "banana"
                Category = None
                PriceRange = None
                StockRange = None
                InStockOnly = true
            }
            let results = applySearchCriteria allProducts criteria
            Expect.isNonEmpty results "Should find bananas"
            Expect.all results (fun p -> p.Stock > 0) "All should be in stock"
    ]
