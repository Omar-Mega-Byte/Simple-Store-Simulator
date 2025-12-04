module DiscountEngine

open CartTypes
open Product
open PriceCalculator

// Discount Engine Module - عمر أحمد محمد أحمد
// This module implements discount logic and promotional pricing

// ============================================
// DISCOUNT TYPES
// ============================================

/// Discount type definition
type DiscountType =
    | Percentage of decimal  // Percentage discount (0.0 to 1.0)
    | FixedAmount of decimal // Fixed amount discount
    | BuyXGetY of int * int  // Buy X get Y free (e.g., Buy 2 Get 1)

/// Discount rule definition
type DiscountRule = {
    RuleId: string
    Name: string
    Description: string
    DiscountType: DiscountType
    MinimumPurchase: decimal option
    MinimumQuantity: int option
    ApplicableCategories: string list option
    ApplicableProductIds: int list option
    IsActive: bool
}

/// Discount application result
type DiscountResult = {
    OriginalPrice: decimal
    DiscountAmount: decimal
    FinalPrice: decimal
    DiscountDescription: string
}

// ============================================
// BASIC DISCOUNT CALCULATIONS
// ============================================

/// Apply percentage discount to a price
let applyPercentageDiscount (price: decimal) (percentage: decimal) : decimal =
    if percentage < 0m || percentage > 1m then
        price // Invalid percentage, return original
    else
        price * (1m - percentage)

/// Calculate percentage discount amount
let calculatePercentageDiscountAmount (price: decimal) (percentage: decimal) : decimal =
    if percentage < 0m || percentage > 1m then
        0m
    else
        price * percentage

/// Apply fixed amount discount to a price
let applyFixedDiscount (price: decimal) (amount: decimal) : decimal =
    let discounted = price - amount
    if discounted < 0m then 0m else discounted

/// Calculate discount amount from discount type
let calculateDiscountAmount (price: decimal) (discountType: DiscountType) : decimal =
    match discountType with
    | Percentage pct -> calculatePercentageDiscountAmount price pct
    | FixedAmount amt -> if amt > price then price else amt
    | BuyXGetY _ -> 0m // Special handling needed for BuyXGetY

// ============================================
// DISCOUNT RULE VALIDATION
// ============================================

/// Check if minimum purchase requirement is met
let checkMinimumPurchase (subtotal: decimal) (rule: DiscountRule) : bool =
    match rule.MinimumPurchase with
    | Some minPurchase -> subtotal >= minPurchase
    | None -> true

/// Check if minimum quantity requirement is met
let checkMinimumQuantity (quantity: int) (rule: DiscountRule) : bool =
    match rule.MinimumQuantity with
    | Some minQty -> quantity >= minQty
    | None -> true

/// Check if product is eligible for discount based on category
let checkCategoryEligibility (product: Product) (rule: DiscountRule) : bool =
    match rule.ApplicableCategories with
    | Some categories -> 
        categories 
        |> List.exists (fun cat -> cat.ToLower() = product.Category.ToLower())
    | None -> true

/// Check if product is eligible based on product ID
let checkProductEligibility (productId: int) (rule: DiscountRule) : bool =
    match rule.ApplicableProductIds with
    | Some ids -> ids |> List.contains productId
    | None -> true

/// Check if discount rule is applicable to a cart entry
let isRuleApplicable (entry: CartEntry) (cartSubtotal: decimal) (rule: DiscountRule) : bool =
    rule.IsActive &&
    checkMinimumPurchase cartSubtotal rule &&
    checkMinimumQuantity entry.Quantity rule &&
    checkCategoryEligibility entry.Product rule &&
    checkProductEligibility entry.Product.Id rule

// ============================================
// APPLY DISCOUNTS TO ITEMS
// ============================================

/// Apply discount to a single cart entry
let applyDiscountToEntry (entry: CartEntry) (rule: DiscountRule) : decimal =
    let originalPrice = calculateItemSubtotal entry
    
    match rule.DiscountType with
    | Percentage pct ->
        let discount = calculatePercentageDiscountAmount originalPrice pct
        discount
    
    | FixedAmount amt ->
        if amt > originalPrice then originalPrice else amt
    
    | BuyXGetY (buyCount, freeCount) ->
        if entry.Quantity >= buyCount then
            let sets = entry.Quantity / buyCount
            let freeItems = sets * freeCount
            let discountAmount = decimal freeItems * entry.Product.Price
            if discountAmount > originalPrice then originalPrice else discountAmount
        else
            0m

/// Apply discount rule to cart entry and return result
let applyDiscountToEntryWithResult (entry: CartEntry) (rule: DiscountRule) (cartSubtotal: decimal) : DiscountResult option =
    if not (isRuleApplicable entry cartSubtotal rule) then
        None
    else
        let originalPrice = calculateItemSubtotal entry
        let discountAmount = applyDiscountToEntry entry rule
        let finalPrice = originalPrice - discountAmount
        
        Some {
            OriginalPrice = originalPrice
            DiscountAmount = discountAmount
            FinalPrice = finalPrice
            DiscountDescription = sprintf "%s: %s" rule.Name rule.Description
        }

// ============================================
// APPLY DISCOUNTS TO CART
// ============================================

/// Apply discount rule to entire cart
let applyDiscountToCart (cart: Cart) (rule: DiscountRule) : decimal =
    let cartSubtotal = calculateCartSubtotal cart
    
    cart.Items
    |> Map.values
    |> Seq.filter (fun entry -> isRuleApplicable entry cartSubtotal rule)
    |> Seq.sumBy (fun entry -> applyDiscountToEntry entry rule)

/// Calculate cart total with discount applied
let calculateCartTotalWithDiscount (cart: Cart) (rule: DiscountRule) (taxRate: decimal) 
    (shippingRates: decimal * decimal * decimal) : decimal =
    let subtotal = calculateCartSubtotal cart
    let discount = applyDiscountToCart cart rule
    let discountedSubtotal = subtotal - discount
    let tax = calculateTax discountedSubtotal taxRate
    let shipping = calculateCartShipping cart shippingRates
    calculateTotal discountedSubtotal tax shipping

/// Calculate cart total with multiple discounts
let calculateCartTotalWithMultipleDiscounts (cart: Cart) (rules: DiscountRule list) (taxRate: decimal) 
    (shippingRates: decimal * decimal * decimal) : decimal =
    let subtotal = calculateCartSubtotal cart
    
    let totalDiscount = 
        rules
        |> List.filter (fun rule -> rule.IsActive)
        |> List.sumBy (fun rule -> applyDiscountToCart cart rule)
    
    let discountedSubtotal = max 0m (subtotal - totalDiscount)
    let tax = calculateTax discountedSubtotal taxRate
    let shipping = calculateCartShipping cart shippingRates
    calculateTotal discountedSubtotal tax shipping

// ============================================
// CART TOTAL WITH DISCOUNT BREAKDOWN
// ============================================

/// Calculate cart breakdown with discount information
let calculateCartBreakdownWithDiscount (cart: Cart) (rule: DiscountRule) (taxRate: decimal) 
    (shippingRates: decimal * decimal * decimal) 
    : decimal * decimal * decimal * decimal * decimal =
    
    let subtotal = calculateCartSubtotal cart
    let discount = applyDiscountToCart cart rule
    let discountedSubtotal = subtotal - discount
    let tax = calculateTax discountedSubtotal taxRate
    let shipping = calculateCartShipping cart shippingRates
    let total = calculateTotal discountedSubtotal tax shipping
    
    (subtotal, discount, tax, shipping, total)

// ============================================
// PREDEFINED DISCOUNT RULES
// ============================================

/// Create a percentage discount rule
let createPercentageDiscount (ruleId: string) (name: string) (percentage: decimal) : DiscountRule =
    {
        RuleId = ruleId
        Name = name
        Description = sprintf "%.0f%% off" (percentage * 100m)
        DiscountType = Percentage percentage
        MinimumPurchase = None
        MinimumQuantity = None
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

/// Create a fixed amount discount rule
let createFixedDiscount (ruleId: string) (name: string) (amount: decimal) : DiscountRule =
    {
        RuleId = ruleId
        Name = name
        Description = sprintf "EGP %.2f off" amount
        DiscountType = FixedAmount amount
        MinimumPurchase = None
        MinimumQuantity = None
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

/// Create a Buy X Get Y discount rule
let createBuyXGetYDiscount (ruleId: string) (name: string) (buyCount: int) (freeCount: int) : DiscountRule =
    {
        RuleId = ruleId
        Name = name
        Description = sprintf "Buy %d Get %d Free" buyCount freeCount
        DiscountType = BuyXGetY (buyCount, freeCount)
        MinimumPurchase = None
        MinimumQuantity = Some buyCount
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

/// Create discount with minimum purchase requirement
let withMinimumPurchase (minPurchase: decimal) (rule: DiscountRule) : DiscountRule =
    { rule with MinimumPurchase = Some minPurchase }

/// Apply discount to specific categories
let forCategories (categories: string list) (rule: DiscountRule) : DiscountRule =
    { rule with ApplicableCategories = Some categories }

/// Apply discount to specific products
let forProducts (productIds: int list) (rule: DiscountRule) : DiscountRule =
    { rule with ApplicableProductIds = Some productIds }

/// Deactivate a discount rule
let deactivateRule (rule: DiscountRule) : DiscountRule =
    { rule with IsActive = false }

// ============================================
// COMMON PROMOTIONAL DISCOUNTS
// ============================================

/// 10% off entire purchase
let discount10Percent = createPercentageDiscount "10PCT" "10% Off" 0.10m

/// 20% off entire purchase
let discount20Percent = createPercentageDiscount "20PCT" "20% Off" 0.20m

/// EGP 50 off on orders over EGP 300
let discount50OffOver300 = 
    createFixedDiscount "50OFF300" "EGP 50 Off" 50m
    |> withMinimumPurchase 300m

/// Buy 2 Get 1 Free
let buy2Get1Free = createBuyXGetYDiscount "B2G1" "Buy 2 Get 1 Free" 2 1

/// Buy 3 Get 2 Free
let buy3Get2Free = createBuyXGetYDiscount "B3G2" "Buy 3 Get 2 Free" 3 2

/// 15% off on Sweets category
let sweetsCategoryDiscount = 
    createPercentageDiscount "SWEETS15" "Sweets Sale" 0.15m
    |> forCategories ["Sweets"]

/// 25% off on Snacks category
let snacksCategoryDiscount = 
    createPercentageDiscount "SNACKS25" "Snacks Promo" 0.25m
    |> forCategories ["Snacks"]

// ============================================
// DISCOUNT FINDER
// ============================================

/// Find best discount rule for a cart from available rules
let findBestDiscount (cart: Cart) (rules: DiscountRule list) : DiscountRule option =
    let cartSubtotal = calculateCartSubtotal cart
    
    rules
    |> List.filter (fun rule -> rule.IsActive)
    |> List.choose (fun rule ->
        let discount = applyDiscountToCart cart rule
        if discount > 0m then Some (rule, discount) else None
    )
    |> List.sortByDescending snd
    |> List.tryHead
    |> Option.map fst

/// Get all applicable discounts for a cart
let getApplicableDiscounts (cart: Cart) (rules: DiscountRule list) : (DiscountRule * decimal) list =
    let cartSubtotal = calculateCartSubtotal cart
    
    rules
    |> List.filter (fun rule -> rule.IsActive)
    |> List.choose (fun rule ->
        let discount = applyDiscountToCart cart rule
        if discount > 0m then Some (rule, discount) else None
    )
    |> List.sortByDescending snd

// ============================================
// DISPLAY FUNCTIONS
// ============================================

/// Display discount information
let displayDiscountInfo (rule: DiscountRule) =
    printfn "Discount: %s" rule.Name
    printfn "  %s" rule.Description
    match rule.MinimumPurchase with
    | Some min -> printfn "  Minimum Purchase: EGP %.2f" min
    | None -> ()
    match rule.MinimumQuantity with
    | Some qty -> printfn "  Minimum Quantity: %d" qty
    | None -> ()
    match rule.ApplicableCategories with
    | Some cats -> printfn "  Applicable Categories: %s" (String.concat ", " cats)
    | None -> ()
    printfn "  Status: %s" (if rule.IsActive then "Active" else "Inactive")

/// Display cart breakdown with discount
let displayCartBreakdownWithDiscount (cart: Cart) (rule: DiscountRule) (taxRate: decimal) 
    (shippingRates: decimal * decimal * decimal) =
    
    let (subtotal, discount, tax, shipping, total) = 
        calculateCartBreakdownWithDiscount cart rule taxRate shippingRates
    
    printfn "=========================================="
    printfn "PRICE BREAKDOWN WITH DISCOUNT"
    printfn "=========================================="
    printfn "Subtotal:         %s" (formatPrice subtotal)
    printfn "Discount (%s):  -%s" rule.Name (formatPrice discount)
    printfn "After Discount:   %s" (formatPrice (subtotal - discount))
    printfn "Tax (%d%%):        %s" (int (taxRate * 100m)) (formatPrice tax)
    printfn "Shipping:         %s" (formatPrice shipping)
    printfn "----------------------------------------"
    printfn "TOTAL:            %s" (formatPrice total)
    printfn "=========================================="
    printfn "You saved:        %s" (formatPrice discount)
    printfn "=========================================="

/// Display all applicable discounts
let displayApplicableDiscounts (cart: Cart) (rules: DiscountRule list) =
    let applicable = getApplicableDiscounts cart rules
    
    if List.isEmpty applicable then
        printfn "No discounts currently applicable to your cart."
    else
        printfn "=========================================="
        printfn "APPLICABLE DISCOUNTS"
        printfn "=========================================="
        applicable |> List.iteri (fun i (rule, amount) ->
            printfn "[%d] %s" (i + 1) rule.Name
            printfn "    %s" rule.Description
            printfn "    Savings: %s" (formatPrice amount)
            printfn ""
        )
        printfn "=========================================="

// ============================================
// EXAMPLES AND TESTING
// ============================================

/// Example usage of discount engine
let runExamples () =
    printfn "=========================================="
    printfn "DISCOUNT ENGINE MODULE EXAMPLES"
    printfn "by عمر أحمد محمد أحمد"
    printfn "=========================================="
    printfn ""
    
    // Create sample products
    let product1 = { Id = 1; Name = "Chocolate"; Price = 15.00m; Description = "Milk chocolate"; Category = "Sweets"; Stock = 10 }
    let product2 = { Id = 2; Name = "Cookies"; Price = 8.00m; Description = "Chocolate chip"; Category = "Snacks"; Stock = 15 }
    
    // Create sample cart
    let entry1 = { Product = product1; Quantity = 3 }
    let entry2 = { Product = product2; Quantity = 2 }
    let cart = { Items = Map.ofList [(1, entry1); (2, entry2)] }
    
    let taxRate = 0.14m
    let shippingRates = (10m, 20m, 30m)
    
    printfn "Cart Contents:"
    printfn "  3x Chocolate @ EGP 15.00 = EGP 45.00"
    printfn "  2x Cookies @ EGP 8.00 = EGP 16.00"
    printfn "  Subtotal: EGP 61.00"
    printfn ""
    
    printfn "Example 1: 10%% Discount"
    displayCartBreakdownWithDiscount cart discount10Percent taxRate shippingRates
    printfn ""
    
    printfn "Example 2: Sweets Category 15%% Off"
    displayCartBreakdownWithDiscount cart sweetsCategoryDiscount taxRate shippingRates
    printfn ""
    
    printfn "Example 3: Buy 2 Get 1 Free on Chocolate"
    let b2g1 = buy2Get1Free |> forProducts [1]
    displayCartBreakdownWithDiscount cart b2g1 taxRate shippingRates
    printfn ""
    
    printfn "Example 4: All Applicable Discounts"
    let allRules = [discount10Percent; discount20Percent; sweetsCategoryDiscount; buy2Get1Free]
    displayApplicableDiscounts cart allRules
    printfn ""
    
    printfn "Example 5: Best Discount"
    match findBestDiscount cart allRules with
    | Some rule -> 
        printfn "Best discount for your cart: %s" rule.Name
        displayDiscountInfo rule
    | None -> 
        printfn "No discounts available"
    printfn ""

// Uncomment to run examples
// runExamples()
