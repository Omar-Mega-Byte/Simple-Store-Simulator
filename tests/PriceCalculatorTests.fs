module PriceCalculatorTests

open Expecto
open Product
open CartTypes
open CartOperations
open PriceCalculator

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()

let createTestCart () =
    let cart1Result = addItem sampleCatalog 1 2 empty  // Chocolate: 15.00 x 2 = 30.00
    match cart1Result with
    | Ok cart1 ->
        let cart2Result = addItem sampleCatalog 2 3 cart1  // Biscuits: 10.00 x 3 = 30.00
        match cart2Result with
        | Ok cart2 -> cart2
        | Error _ -> empty
    | Error _ -> empty

let createSingleItemCart () =
    match addItem sampleCatalog 1 5 empty with  // Chocolate: 15.00 x 5 = 75.00
    | Ok cart -> cart
    | Error _ -> empty

// ============================================
// PRICE CALCULATOR TESTS
// ============================================

[<Tests>]
let priceCalculatorTests =
    testList "Price Calculator Tests" [
        
        testCase "Calculate cart subtotal with multiple items" <| fun _ ->
            let cart = createTestCart()
            let subtotal = calculateCartSubtotal cart
            Expect.equal subtotal 60.00m "Subtotal should be 30.00 + 30.00 = 60.00"
        
        testCase "Calculate tax from subtotal" <| fun _ ->
            let tax = calculateTax 100m 0.14m
            Expect.equal tax 14m "Tax should be 100 x 0.14 = 14"
        
        testCase "Calculate shipping fee for tier 1 (1-5 items)" <| fun _ ->
            let fee = calculateShippingFee 3 10m 20m 30m
            Expect.equal fee 10m "Shipping for 3 items should be tier 1"
        
        testCase "Calculate shipping fee for tier 3 (11+ items)" <| fun _ ->
            let fee = calculateShippingFee 15 10m 20m 30m
            Expect.equal fee 30m "Shipping for 15 items should be tier 3"
        
        testCase "Calculate cart total with all fees" <| fun _ ->
            let cart = createTestCart()  // Subtotal: 60.00
            let total = calculateCartTotal cart 0.14m (10m, 20m, 30m)
            // Subtotal: 60.00, Tax: 8.40, Shipping: 10.00
            Expect.equal total 78.40m "Total should be 60.00 + 8.40 + 10.00 = 78.40"
    ]
