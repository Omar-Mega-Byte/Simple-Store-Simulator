module SearchOperations

open SearchTypes
open Product
open System

// This module provides comprehensive search, filter, and sort operations

// ============================================
// CORE SEARCH FUNCTIONS
// ============================================

/// Search products by name (case-insensitive)
let searchByName (products: Product list) (searchTerm: string) : Product list =
    if String.IsNullOrWhiteSpace(searchTerm) then
        products
    else
        let term = searchTerm.ToLower().Trim()
        products
        |> List.filter (fun p -> 
            p.Name.ToLower().Contains(term))

/// Search products by description (case-insensitive)
let searchByDescription (products: Product list) (searchTerm: string) : Product list =
    if String.IsNullOrWhiteSpace(searchTerm) then
        products
    else
        let term = searchTerm.ToLower().Trim()
        products
        |> List.filter (fun p -> 
            p.Description.ToLower().Contains(term))

/// Search products by name OR description (case-insensitive)
let searchByNameOrDescription (products: Product list) (searchTerm: string) : Product list =
    if String.IsNullOrWhiteSpace(searchTerm) then
        products
    else
        let term = searchTerm.ToLower().Trim()
        products
        |> List.filter (fun p -> 
            p.Name.ToLower().Contains(term) || 
            p.Description.ToLower().Contains(term))

// ============================================
// FILTER FUNCTIONS
// ============================================

/// Filter products by exact category (case-insensitive)
let filterByCategory (products: Product list) (category: string) : Product list =
    if String.IsNullOrWhiteSpace(category) then
        products
    else
        let cat = category.ToLower().Trim()
        products
        |> List.filter (fun p -> 
            p.Category.ToLower() = cat)

/// Filter products by price range (inclusive)
let filterByPriceRange (products: Product list) (minPrice: decimal) (maxPrice: decimal) : Product list =
    products
    |> List.filter (fun p -> 
        p.Price >= minPrice && p.Price <= maxPrice)

/// Filter products by price range using PriceRange type
let filterByPriceRangeType (products: Product list) (priceRange: PriceRange) : Product list =
    filterByPriceRange products priceRange.MinPrice priceRange.MaxPrice

/// Filter products by minimum price
let filterByMinPrice (products: Product list) (minPrice: decimal) : Product list =
    products
    |> List.filter (fun p -> p.Price >= minPrice)

/// Filter products by maximum price
let filterByMaxPrice (products: Product list) (maxPrice: decimal) : Product list =
    products
    |> List.filter (fun p -> p.Price <= maxPrice)

/// Filter products by stock range (inclusive)
let filterByStockRange (products: Product list) (minStock: int) (maxStock: int) : Product list =
    products
    |> List.filter (fun p -> 
        p.Stock >= minStock && p.Stock <= maxStock)

/// Filter products by stock range using StockRange type
let filterByStockRangeType (products: Product list) (stockRange: StockRange) : Product list =
    filterByStockRange products stockRange.MinStock stockRange.MaxStock

/// Filter products that are in stock (stock > 0)
let filterInStockOnly (products: Product list) : Product list =
    products
    |> List.filter (fun p -> p.Stock > 0)

/// Filter products that are out of stock (stock = 0)
let filterOutOfStock (products: Product list) : Product list =
    products
    |> List.filter (fun p -> p.Stock = 0)

/// Filter products with low stock (below threshold)
let filterLowStock (products: Product list) (threshold: int) : Product list =
    products
    |> List.filter (fun p -> p.Stock > 0 && p.Stock <= threshold)

// ============================================
// SORT FUNCTIONS
// ============================================

/// Sort products by name
let sortByName (products: Product list) (ascending: bool) : Product list =
    if ascending then
        products |> List.sortBy (fun p -> p.Name)
    else
        products |> List.sortByDescending (fun p -> p.Name)

/// Sort products by price
let sortByPrice (products: Product list) (ascending: bool) : Product list =
    if ascending then
        products |> List.sortBy (fun p -> p.Price)
    else
        products |> List.sortByDescending (fun p -> p.Price)

/// Sort products by stock level
let sortByStock (products: Product list) (ascending: bool) : Product list =
    if ascending then
        products |> List.sortBy (fun p -> p.Stock)
    else
        products |> List.sortByDescending (fun p -> p.Stock)

/// Sort products by category, then by name
let sortByCategory (products: Product list) (ascending: bool) : Product list =
    if ascending then
        products 
        |> List.sortBy (fun p -> p.Category)
        |> List.sortBy (fun p -> p.Name)
    else
        products 
        |> List.sortByDescending (fun p -> p.Category)
        |> List.sortByDescending (fun p -> p.Name)

/// Sort products using SortConfig
let sortProducts (products: Product list) (sortConfig: SortConfig) : Product list =
    let isAscending = sortConfig.Order = Ascending
    match sortConfig.SortBy with
    | ByName -> sortByName products isAscending
    | ByPrice -> sortByPrice products isAscending
    | ByStock -> sortByStock products isAscending
    | ByCategory -> sortByCategory products isAscending

// ============================================
// UTILITY FUNCTIONS
// ============================================

/// Get all unique categories from product list
let getCategories (products: Product list) : string list =
    products
    |> List.map (fun p -> p.Category)
    |> List.distinct
    |> List.sort

/// Get price range from product list (min, max)
let getPriceRange (products: Product list) : (decimal * decimal) option =
    if List.isEmpty products then
        None
    else
        let prices = products |> List.map (fun p -> p.Price)
        Some (List.min prices, List.max prices)

/// Get stock range from product list (min, max)
let getStockRange (products: Product list) : (int * int) option =
    if List.isEmpty products then
        None
    else
        let stocks = products |> List.map (fun p -> p.Stock)
        Some (List.min stocks, List.max stocks)

/// Count products by category
let countByCategory (products: Product list) : Map<string, int> =
    products
    |> List.groupBy (fun p -> p.Category)
    |> List.map (fun (category, prods) -> (category, List.length prods))
    |> Map.ofList

/// Get products with highest price (top N)
let getTopPriced (products: Product list) (count: int) : Product list =
    products
    |> List.sortByDescending (fun p -> p.Price)
    |> List.truncate count

/// Get products with lowest price (top N)
let getCheapest (products: Product list) (count: int) : Product list =
    products
    |> List.sortBy (fun p -> p.Price)
    |> List.truncate count

/// Get products with most stock (top N)
let getMostStocked (products: Product list) (count: int) : Product list =
    products
    |> List.sortByDescending (fun p -> p.Stock)
    |> List.truncate count

// ============================================
// ADVANCED SEARCH - QUERY COMPOSITION
// ============================================

/// Apply search criteria to product list (functional pipeline approach)
let applySearchCriteria (products: Product list) (criteria: SearchCriteria) : Product list =
    products
    // Apply search term filter
    |> fun prods ->
        match criteria.SearchTerm with
        | Some term when not (String.IsNullOrWhiteSpace(term)) -> 
            searchByNameOrDescription prods term
        | _ -> prods
    
    // Apply category filter
    |> fun prods ->
        match criteria.Category with
        | Some cat when not (String.IsNullOrWhiteSpace(cat)) -> 
            filterByCategory prods cat
        | _ -> prods
    
    // Apply price range filter
    |> fun prods ->
        match criteria.PriceRange with
        | Some range -> 
            filterByPriceRangeType prods range
        | None -> prods
    
    // Apply stock range filter
    |> fun prods ->
        match criteria.StockRange with
        | Some range -> 
            filterByStockRangeType prods range
        | None -> prods
    
    // Apply in-stock filter
    |> fun prods ->
        if criteria.InStockOnly then
            filterInStockOnly prods
        else
            prods

/// Build a list of applied filters for display
let getAppliedFilters (criteria: SearchCriteria) : string list =
    let filters = System.Collections.Generic.List<string>()
    
    match criteria.SearchTerm with
    | Some term when not (String.IsNullOrWhiteSpace(term)) -> 
        filters.Add($"Search: '{term}'")
    | _ -> ()
    
    match criteria.Category with
    | Some cat when not (String.IsNullOrWhiteSpace(cat)) -> 
        filters.Add($"Category: {cat}")
    | _ -> ()
    
    match criteria.PriceRange with
    | Some range -> 
        filters.Add($"Price: EGP {range.MinPrice:F2} - EGP {range.MaxPrice:F2}")
    | None -> ()
    
    match criteria.StockRange with
    | Some range -> 
        filters.Add($"Stock: {range.MinStock} - {range.MaxStock} units")
    | None -> ()
    
    if criteria.InStockOnly then
        filters.Add("In Stock Only")
    
    filters |> List.ofSeq

/// Advanced search with result metadata
let searchWithMetadata (products: Product list) (criteria: SearchCriteria) (sortConfig: SortConfig option) : SearchResult<Product> =
    let startTime = DateTime.Now
    
    // Apply filters
    let filtered = applySearchCriteria products criteria
    
    // Apply sorting if provided
    let sorted = 
        match sortConfig with
        | Some config -> sortProducts filtered config
        | None -> filtered
    
    let endTime = DateTime.Now
    let searchTime = endTime - startTime
    
    {
        Results = sorted
        TotalFound = List.length sorted
        SearchTime = searchTime
        AppliedFilters = getAppliedFilters criteria
    }

// ============================================
// PREDEFINED QUERIES
// ============================================

/// Find affordable products (below a price threshold)
let findAffordableProducts (products: Product list) (maxPrice: decimal) : Product list =
    products
    |> (fun ps -> filterByMaxPrice ps maxPrice)
    |> filterInStockOnly
    |> (fun ps -> sortByPrice ps true)

/// Find premium products (above a price threshold)
let findPremiumProducts (products: Product list) (minPrice: decimal) : Product list =
    products
    |> (fun ps -> filterByMinPrice ps minPrice)
    |> (fun ps -> sortByPrice ps false)

/// Find best deals (in stock, sorted by lowest price)
let findBestDeals (products: Product list) (count: int) : Product list =
    products
    |> filterInStockOnly
    |> getCheapest <| count

/// Find products needing restock (low stock items)
let findRestockNeeded (products: Product list) (threshold: int) : Product list =
    products
    |> filterLowStock <| threshold
    |> sortByStock <| true

/// Search products by multiple categories
let searchMultipleCategories (products: Product list) (categories: string list) : Product list =
    let categoriesLower = categories |> List.map (fun c -> c.ToLower().Trim())
    products
    |> List.filter (fun p -> 
        categoriesLower |> List.contains (p.Category.ToLower()))

// ============================================
// DISPLAY HELPERS
// ============================================

/// Format search result summary
let formatSearchSummary (result: SearchResult<Product>) : string =
    let filtersText = 
        if List.isEmpty result.AppliedFilters then
            "No filters applied"
        else
            String.Join(", ", result.AppliedFilters)
    
    sprintf "Found %d product(s) in %.2f ms\nFilters: %s" 
        result.TotalFound 
        result.SearchTime.TotalMilliseconds
        filtersText

/// Display search results
let displaySearchResults (result: SearchResult<Product>) =
    printfn "=========================================="
    printfn "SEARCH RESULTS"
    printfn "=========================================="
    printfn "%s" (formatSearchSummary result)
    printfn ""
    
    if List.isEmpty result.Results then
        printfn "No products found matching your criteria."
    else
        result.Results |> List.iteri (fun i p ->
            printfn "[%d] %s - EGP %.2f" (i + 1) p.Name p.Price
            printfn "    %s" p.Description
            printfn "    Category: %s | Stock: %d units" p.Category p.Stock
            printfn "")
    
    printfn "=========================================="

// ============================================
// EXAMPLES AND TESTING
// ============================================

/// Example: Create sample products for testing
let createSampleProducts () : Product list = [
    { Id = 1; Name = "Chocolate"; Price = 15.00m; Description = "Delicious milk chocolate bar"; Category = "Sweets"; Stock = 10 }
    { Id = 2; Name = "Biscuits"; Price = 10.00m; Description = "Crunchy butter biscuits"; Category = "Snacks"; Stock = 20 }
    { Id = 3; Name = "Ice Cream"; Price = 12.50m; Description = "Vanilla ice cream tub"; Category = "Frozen"; Stock = 5 }
    { Id = 4; Name = "Cookies"; Price = 8.00m; Description = "Chocolate chip cookies"; Category = "Snacks"; Stock = 15 }
    { Id = 5; Name = "Candy"; Price = 5.00m; Description = "Assorted fruit candies"; Category = "Sweets"; Stock = 30 }
    { Id = 6; Name = "Chips"; Price = 7.50m; Description = "Crispy potato chips"; Category = "Snacks"; Stock = 0 }
    { Id = 7; Name = "Juice"; Price = 18.00m; Description = "Fresh orange juice"; Category = "Beverages"; Stock = 12 }
    { Id = 8; Name = "Cake"; Price = 25.00m; Description = "Chocolate cake"; Category = "Sweets"; Stock = 3 }
]

/// Run example searches
let runExamples () =
    printfn "=========================================="
    printfn "SEARCH & FILTER MODULE EXAMPLES"
    printfn "by عمر أحمد الرفاعي طليس"
    printfn "=========================================="
    printfn ""
    
    let products = createSampleProducts()
    
    // Example 1: Search by name
    printfn "Example 1: Search for 'chocolate'"
    let result1 = searchByName products "chocolate"
    printfn "Found %d product(s)" (List.length result1)
    result1 |> List.iter (fun p -> printfn "  - %s" p.Name)
    printfn ""
    
    // Example 2: Filter by category
    printfn "Example 2: Filter by 'Snacks' category"
    let result2 = filterByCategory products "Snacks"
    printfn "Found %d product(s)" (List.length result2)
    result2 |> List.iter (fun p -> printfn "  - %s (EGP %.2f)" p.Name p.Price)
    printfn ""
    
    // Example 3: Filter by price range
    printfn "Example 3: Products between EGP 5 and EGP 15"
    let result3 = filterByPriceRange products 5.00m 15.00m
    printfn "Found %d product(s)" (List.length result3)
    result3 |> List.iter (fun p -> printfn "  - %s at EGP %.2f" p.Name p.Price)
    printfn ""
    
    // Example 4: Sort by price
    printfn "Example 4: All products sorted by price (ascending)"
    let result4 = sortByPrice products true
    result4 |> List.iter (fun p -> printfn "  - %s: EGP %.2f" p.Name p.Price)
    printfn ""
    
    // Example 5: Advanced search with criteria
    printfn "Example 5: Advanced search - In stock Sweets under EGP 20"
    let criteria = {
        SearchTerm = None
        Category = Some "Sweets"
        PriceRange = Some { MinPrice = 0m; MaxPrice = 20m }
        StockRange = None
        InStockOnly = true
    }
    let sortConfig = Some { SortBy = ByPrice; Order = Ascending }
    let result5 = searchWithMetadata products criteria sortConfig
    displaySearchResults result5
    
    // Example 6: Get categories
    printfn "Example 6: All available categories"
    let categories = getCategories products
    categories |> List.iter (fun c -> printfn "  - %s" c)
    printfn ""
    
    // Example 7: Find best deals
    printfn "Example 7: Top 3 cheapest products in stock"
    let result7 = findBestDeals products 3
    result7 |> List.iter (fun p -> printfn "  - %s: EGP %.2f (Stock: %d)" p.Name p.Price p.Stock)
    printfn ""
    
    // Example 8: Products needing restock
    printfn "Example 8: Products with low stock (≤ 5 units)"
    let result8 = findRestockNeeded products 5
    result8 |> List.iter (fun p -> printfn "  - %s: %d units remaining" p.Name p.Stock)
    printfn ""

// Uncomment to run examples
// runExamples()
