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

/// Search products by description (case-insensitive) TEMPORARY
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

/// Filter products that are out of stock (stock = 0) I DONT USE THIS ONE
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


