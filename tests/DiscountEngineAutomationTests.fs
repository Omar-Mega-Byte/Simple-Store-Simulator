module DiscountEngineAutomationTests

open Expecto
open Product
open CartTypes
open CartOperations
open PriceCalculator
open DiscountEngine

// ============================================
// AUTOMATION TEST SUITE FOR DISCOUNT ENGINE
// ============================================
// Streamlined tests for main discount features

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()

let createTestCart () : Cart =
    let cart1Result = addItem sampleCatalog 1 3 empty
    match cart1Result with
    | Ok cart1 ->
        let cart2Result = addItem sampleCatalog 2 2 cart1
        match cart2Result with
        | Ok cart2 -> cart2
        | Error _ -> cart1
    | Error _ -> empty

let createPercentageDiscountRule () : DiscountRule =
    {
        RuleId = "SAVE20"
        Name = "20% Off"
        Description = "Get 20% off your purchase"
        DiscountType = Percentage 0.20m
        MinimumPurchase = None
        MinimumQuantity = None
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

let createFixedDiscountRule () : DiscountRule =
    {
        RuleId = "SAVE10"
        Name = "$10 Off"
        Description = "Get $10 off your purchase"
        DiscountType = FixedAmount 10m
        MinimumPurchase = Some 50m
        MinimumQuantity = None
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

let createBuyXGetYRule () : DiscountRule =
    {
        RuleId = "BUY2GET1"
        Name = "Buy 2 Get 1 Free"
        Description = "Buy 2 items, get 1 free"
        DiscountType = BuyXGetY (2, 1)
        MinimumPurchase = None
        MinimumQuantity = Some 2
        ApplicableCategories = None
        ApplicableProductIds = None
        IsActive = true
    }

// ============================================
// MAIN DISCOUNT TESTS
// ============================================

[<Tests>]
let discountEngineTests =
    testList "Discount Engine Main Tests" [
        
        testCase "Apply percentage discount to cart" <| fun _ ->
            let cart = createTestCart()
            let rule = createPercentageDiscountRule()
            let discount = applyDiscountToCart cart rule
            Expect.equal discount 13.00m "20% of 65.00 should be 13.00"
        
        testCase "Calculate cart total with percentage discount" <| fun _ ->
            let cart = createTestCart()
            let rule = createPercentageDiscountRule()
            let config = { TaxRate = 0.14m; ShippingRates = (10m, 20m, 30m) }
            let discountedTotal = calculateCartTotalWithDiscount cart rule config.TaxRate config.ShippingRates
            let originalTotal = calculateCartTotal cart config.TaxRate config.ShippingRates
            Expect.isLessThan discountedTotal originalTotal "Discounted total should be less"
        
        testCase "Apply fixed discount with minimum purchase" <| fun _ ->
            let cart = createTestCart()
            let rule = createFixedDiscountRule()
            let discount = applyDiscountToCart cart rule
            Expect.isGreaterThan discount 0m "Should get discount when meeting minimum"
        
        testCase "Buy X Get Y discount calculation" <| fun _ ->
            let cart = empty
            match addItem sampleCatalog 1 3 cart with
            | Ok testCart ->
                let rule = createBuyXGetYRule()
                let discount = applyDiscountToCart testCart rule
                Expect.equal discount 15.00m "Should get 1 free item worth 15.00"
            | Error _ -> failtest "Cart creation failed"
    ]

