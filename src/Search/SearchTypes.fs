module SearchTypes

// Search & Filter Module - عمر أحمد الرفاعي طليس (Team Leader)
// This module defines types for search criteria and filter options

/// Sort order enumeration
type SortOrder = 
    | Ascending 
    | Descending

/// Sort field options
type SortBy =
    | ByName
    | ByPrice
    | ByStock
    | ByCategory

/// Price range for filtering
type PriceRange = {
    MinPrice: decimal
    MaxPrice: decimal
}

/// Stock range for filtering
type StockRange = {
    MinStock: int
    MaxStock: int
}

/// Comprehensive search criteria
type SearchCriteria = {
    SearchTerm: string option          // Search by name/description
    Category: string option            // Filter by category
    PriceRange: PriceRange option      // Filter by price range
    StockRange: StockRange option      // Filter by stock level
    InStockOnly: bool                  // Show only in-stock items
}

/// Sort configuration
type SortConfig = {
    SortBy: SortBy
    Order: SortOrder
}

/// Search result with metadata
type SearchResult<'T> = {
    Results: 'T list
    TotalFound: int
    SearchTime: System.TimeSpan
    AppliedFilters: string list
}
/// Utility functions [لسه مستخدمتهمش]
/// Create empty search criteria (no filters)
let createEmptySearchCriteria () : SearchCriteria =
    {
        SearchTerm = None
        Category = None
        PriceRange = None
        StockRange = None
        InStockOnly = false
    }

/// Create default sort configuration (by name, ascending)
let createDefaultSortConfig () : SortConfig =
    {
        SortBy = ByName
        Order = Ascending
    }

/// Create price range
let createPriceRange (min: decimal) (max: decimal) : PriceRange =
    {
        MinPrice = min
        MaxPrice = max
    }

/// Create stock range
let createStockRange (min: int) (max: int) : StockRange =
    {
        MinStock = min
        MaxStock = max
    }
