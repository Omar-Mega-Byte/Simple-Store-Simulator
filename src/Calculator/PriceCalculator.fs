module PriceCalculator

open CartTypes
open Product

// Price Calculator Module - عمر أحمد محمد أحمد
// This module provides comprehensive price calculation functions for the store

// ============================================
// BASIC PRICE CALCULATIONS
// ============================================

/// Calculate subtotal for a single cart entry (price × quantity)
let calculateItemSubtotal (entry: CartEntry) : decimal =
    entry.Product.Price * decimal entry.Quantity

/// Calculate subtotal for all items in the cart
let calculateCartSubtotal (cart: Cart) : decimal =
    cart.Items
    |> Map.values
    |> Seq.sumBy calculateItemSubtotal

/// Calculate subtotal from a list of cart entries
let calculateSubtotalFromList (entries: CartEntry list) : decimal =
    entries
    |> List.sumBy calculateItemSubtotal

// ============================================
// TAX CALCULATIONS
// ============================================

/// Calculate tax amount based on subtotal and tax rate
let calculateTax (subtotal: decimal) (taxRate: decimal) : decimal =
    subtotal * taxRate

/// Calculate tax for the entire cart
let calculateCartTax (cart: Cart) (taxRate: decimal) : decimal =
    let subtotal = calculateCartSubtotal cart
    calculateTax subtotal taxRate

/// Calculate tax with validation (tax rate must be between 0 and 1)
let calculateTaxWithValidation (subtotal: decimal) (taxRate: decimal) : Result<decimal, string> =
    if taxRate < 0.0m then
        Error "Tax rate cannot be negative"
    elif taxRate > 1.0m then
        Error "Tax rate cannot exceed 100%"
    else
        Ok (calculateTax subtotal taxRate)

// ============================================
// SHIPPING CALCULATIONS
// ============================================

/// Calculate shipping fee based on item count and tier rates
let calculateShippingFee (itemCount: int) (tier1: decimal) (tier2: decimal) (tier3: decimal) : decimal =
    match itemCount with
    | 0 -> 0m
    | n when n <= 5 -> tier1
    | n when n <= 10 -> tier2
    | _ -> tier3

/// Calculate shipping fee for a cart using shipping rates tuple
let calculateCartShipping (cart: Cart) (shippingRates: decimal * decimal * decimal) : decimal =
    let (tier1, tier2, tier3) = shippingRates
    let itemCount = 
        cart.Items 
        |> Map.values 
        |> Seq.sumBy (fun entry -> entry.Quantity)
    calculateShippingFee itemCount tier1 tier2 tier3

/// Calculate shipping fee with free shipping threshold
let calculateShippingWithFreeThreshold (subtotal: decimal) (itemCount: int) (freeShippingThreshold: decimal) 
    (tier1: decimal) (tier2: decimal) (tier3: decimal) : decimal =
    if subtotal >= freeShippingThreshold then
        0m
    else
        calculateShippingFee itemCount tier1 tier2 tier3

// ============================================
// TOTAL CALCULATIONS
// ============================================

/// Calculate grand total (subtotal + tax + shipping)
let calculateTotal (subtotal: decimal) (tax: decimal) (shipping: decimal) : decimal =
    subtotal + tax + shipping

/// Calculate complete cart total with all fees
let calculateCartTotal (cart: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) : decimal =
    let subtotal = calculateCartSubtotal cart
    let tax = calculateTax subtotal taxRate
    let shipping = calculateCartShipping cart shippingRates
    calculateTotal subtotal tax shipping

/// Calculate cart total with breakdown information
let calculateCartTotalWithBreakdown (cart: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) 
    : decimal * decimal * decimal * decimal =
    let subtotal = calculateCartSubtotal cart
    let tax = calculateTax subtotal taxRate
    let shipping = calculateCartShipping cart shippingRates
    let total = calculateTotal subtotal tax shipping
    (subtotal, tax, shipping, total)

// ============================================
// ORDER SUMMARY CREATION
// ============================================

/// Create an order summary from a cart with discount applied
let createOrderSummary (cart: Cart) (config: CheckoutConfig) (discount: decimal) : Order =
    let (tier1, tier2, tier3) = config.ShippingRates
    let subtotal = calculateCartSubtotal cart
    let discountedSubtotal = subtotal - discount
    let tax = calculateTax discountedSubtotal config.TaxRate
    let shipping = calculateCartShipping cart config.ShippingRates
    let total = calculateTotal discountedSubtotal tax shipping
    
    {
        OrderId = System.Guid.NewGuid().ToString()
        Items = cart.Items |> Map.toList |> List.map snd
        Subtotal = subtotal
        Discount = discount
        Tax = tax
        Shipping = shipping
        Total = total
        Timestamp = System.DateTime.UtcNow
    }

/// Create order summary with custom order ID
let createOrderSummaryWithId (orderId: string) (cart: Cart) (config: CheckoutConfig) (discount: decimal) : Order =
    let order = createOrderSummary cart config discount
    { order with OrderId = orderId }

// ============================================
// PRICE FORMATTING HELPERS
// ============================================

/// Format decimal amount as currency string
let formatPrice (amount: decimal) : string =
    sprintf "EGP %.2f" amount

/// Format price with symbol
let formatPriceWithSymbol (amount: decimal) (symbol: string) : string =
    sprintf "%s %.2f" symbol amount

/// Format all cart totals as strings
let formatCartTotals (cart: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) 
    : string * string * string * string =
    let (subtotal, tax, shipping, total) = calculateCartTotalWithBreakdown cart taxRate shippingRates
    (formatPrice subtotal, formatPrice tax, formatPrice shipping, formatPrice total)

// ============================================
// PRICE STATISTICS
// ============================================

/// Calculate average item price in cart
let calculateAverageItemPrice (cart: Cart) : decimal option =
    let items = cart.Items |> Map.values |> Seq.toList
    if List.isEmpty items then
        None
    else
        let totalPrice = items |> List.sumBy (fun e -> e.Product.Price)
        let itemCount = List.length items
        Some (totalPrice / decimal itemCount)

/// Find the most expensive item in cart
let findMostExpensiveItem (cart: Cart) : CartEntry option =
    cart.Items
    |> Map.values
    |> Seq.sortByDescending (fun e -> e.Product.Price)
    |> Seq.tryHead

/// Find the cheapest item in cart
let findCheapestItem (cart: Cart) : CartEntry option =
    cart.Items
    |> Map.values
    |> Seq.sortBy (fun e -> e.Product.Price)
    |> Seq.tryHead

/// Calculate total quantity of items in cart
let calculateTotalQuantity (cart: Cart) : int =
    cart.Items
    |> Map.values
    |> Seq.sumBy (fun entry -> entry.Quantity)

/// Calculate average price per unit (total / total quantity)
let calculateAveragePricePerUnit (cart: Cart) : decimal option =
    let totalQuantity = calculateTotalQuantity cart
    if totalQuantity = 0 then
        None
    else
        let subtotal = calculateCartSubtotal cart
        Some (subtotal / decimal totalQuantity)

// ============================================
// PRICE COMPARISON UTILITIES
// ============================================

/// Compare two carts and return price difference
let compareCarts (cart1: Cart) (cart2: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) : decimal =
    let total1 = calculateCartTotal cart1 taxRate shippingRates
    let total2 = calculateCartTotal cart2 taxRate shippingRates
    total1 - total2

/// Calculate price per item for bulk purchases
let calculatePricePerItem (totalPrice: decimal) (quantity: int) : decimal option =
    if quantity <= 0 then
        None
    else
        Some (totalPrice / decimal quantity)

// ============================================
// DISPLAY FUNCTIONS
// ============================================

/// Display formatted price breakdown
let displayPriceBreakdown (cart: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) =
    let (subtotal, tax, shipping, total) = calculateCartTotalWithBreakdown cart taxRate shippingRates
    
    printfn "=========================================="
    printfn "PRICE BREAKDOWN"
    printfn "=========================================="
    printfn "Subtotal:  %s" (formatPrice subtotal)
    printfn "Tax (%d%%): %s" (int (taxRate * 100m)) (formatPrice tax)
    printfn "Shipping:  %s" (formatPrice shipping)
    printfn "----------------------------------------"
    printfn "TOTAL:     %s" (formatPrice total)
    printfn "=========================================="

/// Display cart statistics
let displayCartStatistics (cart: Cart) =
    printfn "=========================================="
    printfn "CART STATISTICS"
    printfn "=========================================="
    printfn "Total Items:        %d" (Map.count cart.Items)
    printfn "Total Quantity:     %d" (calculateTotalQuantity cart)
    printfn "Cart Subtotal:      %s" (formatPrice (calculateCartSubtotal cart))
    
    match calculateAverageItemPrice cart with
    | Some avg -> printfn "Avg Item Price:     %s" (formatPrice avg)
    | None -> printfn "Avg Item Price:     N/A"
    
    match calculateAveragePricePerUnit cart with
    | Some avg -> printfn "Avg Price/Unit:     %s" (formatPrice avg)
    | None -> printfn "Avg Price/Unit:     N/A"
    
    match findMostExpensiveItem cart with
    | Some entry -> printfn "Most Expensive:     %s (%s)" entry.Product.Name (formatPrice entry.Product.Price)
    | None -> printfn "Most Expensive:     N/A"
    
    match findCheapestItem cart with
    | Some entry -> printfn "Cheapest Item:      %s (%s)" entry.Product.Name (formatPrice entry.Product.Price)
    | None -> printfn "Cheapest Item:      N/A"
    
    printfn "=========================================="

// ============================================
// VALIDATION FUNCTIONS
// ============================================

/// Validate that cart has items before calculating
let validateCartNotEmpty (cart: Cart) : Result<Cart, string> =
    if Map.isEmpty cart.Items then
        Error "Cart is empty - cannot calculate prices"
    else
        Ok cart

/// Validate tax rate is within acceptable range
let validateTaxRate (taxRate: decimal) : Result<decimal, string> =
    if taxRate < 0m then
        Error "Tax rate cannot be negative"
    elif taxRate > 1m then
        Error "Tax rate cannot exceed 100%"
    else
        Ok taxRate

/// Validate shipping rates are positive
let validateShippingRates (tier1: decimal) (tier2: decimal) (tier3: decimal) : Result<decimal * decimal * decimal, string> =
    if tier1 < 0m || tier2 < 0m || tier3 < 0m then
        Error "Shipping rates cannot be negative"
    else
        Ok (tier1, tier2, tier3)

// ============================================
// ADVANCED CALCULATION WITH VALIDATION
// ============================================

/// Calculate cart total with full validation
let calculateCartTotalValidated (cart: Cart) (taxRate: decimal) (shippingRates: decimal * decimal * decimal) 
    : Result<decimal, string> =
    match validateCartNotEmpty cart with
    | Error err -> Error err
    | Ok validCart ->
        match validateTaxRate taxRate with
        | Error err -> Error err
        | Ok validTaxRate ->
            let (tier1, tier2, tier3) = shippingRates
            match validateShippingRates tier1 tier2 tier3 with
            | Error err -> Error err
            | Ok validShipping ->
                let subtotal = calculateCartSubtotal validCart
                let tax = calculateTax subtotal validTaxRate
                let shipping = calculateCartShipping validCart validShipping
                Ok (calculateTotal subtotal tax shipping)

// ============================================
// EXAMPLES AND TESTING
// ============================================

/// Example usage of price calculator functions
let runExamples () =
    printfn "=========================================="
    printfn "PRICE CALCULATOR MODULE EXAMPLES"
    printfn "by عمر أحمد محمد أحمد"
    printfn "=========================================="
    printfn ""
    
    // Create sample products
    let product1 = { Id = 1; Name = "Chocolate"; Price = 15.00m; Description = "Milk chocolate"; Category = "Sweets"; Stock = 10 }
    let product2 = { Id = 2; Name = "Cookies"; Price = 8.00m; Description = "Chocolate chip"; Category = "Snacks"; Stock = 15 }
    
    // Create sample cart
    let entry1 = { Product = product1; Quantity = 2 }
    let entry2 = { Product = product2; Quantity = 3 }
    let cart = { Items = Map.ofList [(1, entry1); (2, entry2)] }
    
    // Example calculations
    printfn "Example 1: Item Subtotals"
    printfn "  %s x %d = %s" entry1.Product.Name entry1.Quantity (formatPrice (calculateItemSubtotal entry1))
    printfn "  %s x %d = %s" entry2.Product.Name entry2.Quantity (formatPrice (calculateItemSubtotal entry2))
    printfn ""
    
    printfn "Example 2: Cart Subtotal"
    let subtotal = calculateCartSubtotal cart
    printfn "  Cart Subtotal: %s" (formatPrice subtotal)
    printfn ""
    
    printfn "Example 3: Tax Calculation (14%%)"
    let taxRate = 0.14m
    let tax = calculateCartTax cart taxRate
    printfn "  Tax Amount: %s" (formatPrice tax)
    printfn ""
    
    printfn "Example 4: Shipping Calculation"
    let shippingRates = (10m, 20m, 30m)
    let shipping = calculateCartShipping cart shippingRates
    printfn "  Shipping Fee: %s" (formatPrice shipping)
    printfn ""
    
    printfn "Example 5: Complete Price Breakdown"
    displayPriceBreakdown cart taxRate shippingRates
    printfn ""
    
    printfn "Example 6: Cart Statistics"
    displayCartStatistics cart
    printfn ""

// Uncomment to run examples
// runExamples()
