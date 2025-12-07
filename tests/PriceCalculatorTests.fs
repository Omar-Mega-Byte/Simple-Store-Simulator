module PriceCalculatorTests

open Expecto
open Product
open ProductOperations
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
        
        testCase "Calculate item subtotal for single product" <| fun _ ->
            match getProduct sampleCatalog 1 with
            | Some product ->
                let entry = { Product = product; Quantity = 3 }
                let subtotal = calculateItemSubtotal entry
                Expect.equal subtotal 45.00m "Subtotal should be 15.00 x 3 = 45.00"
            | None ->
                failtest "Product should exist"
        
        testCase "Calculate item subtotal with quantity 1" <| fun _ ->
            match getProduct sampleCatalog 2 with
            | Some product ->
                let entry = { Product = product; Quantity = 1 }
                let subtotal = calculateItemSubtotal entry
                Expect.equal subtotal 10.00m "Subtotal should equal product price"
            | None ->
                failtest "Product should exist"
        
        testCase "Calculate cart subtotal with multiple items" <| fun _ ->
            let cart = createTestCart()
            let subtotal = calculateCartSubtotal cart
            Expect.equal subtotal 60.00m "Subtotal should be 30.00 + 30.00 = 60.00"
        
        testCase "Calculate cart subtotal for empty cart" <| fun _ ->
            let subtotal = calculateCartSubtotal empty
            Expect.equal subtotal 0m "Empty cart subtotal should be 0"
        
        testCase "Calculate cart subtotal with single item" <| fun _ ->
            let cart = createSingleItemCart()
            let subtotal = calculateCartSubtotal cart
            Expect.equal subtotal 75.00m "Subtotal should be 15.00 x 5 = 75.00"
        
        testCase "Calculate tax from subtotal" <| fun _ ->
            let tax = calculateTax 100m 0.14m
            Expect.equal tax 14m "Tax should be 100 x 0.14 = 14"
        
        testCase "Calculate tax with zero rate" <| fun _ ->
            let tax = calculateTax 100m 0m
            Expect.equal tax 0m "Tax with 0 rate should be 0"
        
        testCase "Calculate tax with 100% rate" <| fun _ ->
            let tax = calculateTax 50m 1.0m
            Expect.equal tax 50m "Tax with 100% rate should equal subtotal"
        
        testCase "Calculate cart tax" <| fun _ ->
            let cart = createTestCart()
            let tax = calculateCartTax cart 0.14m
            Expect.equal tax 8.40m "Tax should be 60.00 x 0.14 = 8.40"
        
        testCase "Calculate tax with validation accepts valid rate" <| fun _ ->
            let result = calculateTaxWithValidation 100m 0.14m
            match result with
            | Ok tax ->
                Expect.equal tax 14m "Tax should be calculated correctly"
            | Error msg ->
                failtest $"Valid tax rate should succeed: {msg}"
        
        testCase "Calculate tax with validation rejects negative rate" <| fun _ ->
            let result = calculateTaxWithValidation 100m -0.05m
            match result with
            | Ok _ ->
                failtest "Negative tax rate should be rejected"
            | Error msg ->
                Expect.stringContains msg "negative" "Error should mention negative rate"
        
        testCase "Calculate tax with validation rejects rate above 100%" <| fun _ ->
            let result = calculateTaxWithValidation 100m 1.5m
            match result with
            | Ok _ ->
                failtest "Tax rate above 100% should be rejected"
            | Error msg ->
                Expect.stringContains msg "exceed" "Error should mention exceeding 100%"
        
        testCase "Calculate shipping fee for 0 items" <| fun _ ->
            let fee = calculateShippingFee 0 10m 20m 30m
            Expect.equal fee 0m "Shipping for 0 items should be free"
        
        testCase "Calculate shipping fee for tier 1 (1-5 items)" <| fun _ ->
            let fee = calculateShippingFee 3 10m 20m 30m
            Expect.equal fee 10m "Shipping for 3 items should be tier 1"
        
        testCase "Calculate shipping fee for tier 1 boundary (5 items)" <| fun _ ->
            let fee = calculateShippingFee 5 10m 20m 30m
            Expect.equal fee 10m "Shipping for 5 items should be tier 1"
        
        testCase "Calculate shipping fee for tier 2 (6-10 items)" <| fun _ ->
            let fee = calculateShippingFee 7 10m 20m 30m
            Expect.equal fee 20m "Shipping for 7 items should be tier 2"
        
        testCase "Calculate shipping fee for tier 2 boundary (10 items)" <| fun _ ->
            let fee = calculateShippingFee 10 10m 20m 30m
            Expect.equal fee 20m "Shipping for 10 items should be tier 2"
        
        testCase "Calculate shipping fee for tier 3 (11+ items)" <| fun _ ->
            let fee = calculateShippingFee 15 10m 20m 30m
            Expect.equal fee 30m "Shipping for 15 items should be tier 3"
        
        testCase "Calculate cart shipping" <| fun _ ->
            let cart = createTestCart()  // 2 + 3 = 5 items
            let shipping = calculateCartShipping cart (10m, 20m, 30m)
            Expect.equal shipping 10m "Shipping for 5 items should be tier 1"
        
        testCase "Calculate shipping with free threshold applies free shipping" <| fun _ ->
            let fee = calculateShippingWithFreeThreshold 200m 5 150m 10m 20m 30m
            Expect.equal fee 0m "Should be free shipping when above threshold"
        
        testCase "Calculate shipping with free threshold charges when below" <| fun _ ->
            let fee = calculateShippingWithFreeThreshold 100m 3 150m 10m 20m 30m
            Expect.equal fee 10m "Should charge shipping when below threshold"
        
        testCase "Calculate shipping with free threshold at exact threshold" <| fun _ ->
            let fee = calculateShippingWithFreeThreshold 150m 5 150m 10m 20m 30m
            Expect.equal fee 0m "Should be free at exact threshold"
        
        testCase "Calculate total from components" <| fun _ ->
            let total = calculateTotal 100m 14m 10m
            Expect.equal total 124m "Total should be 100 + 14 + 10 = 124"
        
        testCase "Calculate total with zero values" <| fun _ ->
            let total = calculateTotal 50m 0m 0m
            Expect.equal total 50m "Total should equal subtotal when no fees"
        
        testCase "Calculate cart total with all fees" <| fun _ ->
            let cart = createTestCart()  // Subtotal: 60.00
            let total = calculateCartTotal cart 0.14m (10m, 20m, 30m)
            // Subtotal: 60.00, Tax: 8.40, Shipping: 10.00
            Expect.equal total 78.40m "Total should be 60.00 + 8.40 + 10.00 = 78.40"
        
        testCase "Calculate cart total with breakdown" <| fun _ ->
            let cart = createSingleItemCart()  // Subtotal: 75.00, 5 items
            let (subtotal, tax, shipping, total) = 
                calculateCartTotalWithBreakdown cart 0.14m (10m, 20m, 30m)
            Expect.equal subtotal 75.00m "Subtotal should be 75.00"
            Expect.equal tax 10.50m "Tax should be 10.50"
            Expect.equal shipping 10m "Shipping should be 10"
            Expect.equal total 95.50m "Total should be 95.50"
        
        testCase "Calculate subtotal from entry list" <| fun _ ->
            match getProduct sampleCatalog 1, getProduct sampleCatalog 2 with
            | Some p1, Some p2 ->
                let entries = [
                    { Product = p1; Quantity = 2 }
                    { Product = p2; Quantity = 3 }
                ]
                let subtotal = calculateSubtotalFromList entries
                Expect.equal subtotal 60.00m "Subtotal should be 30.00 + 30.00 = 60.00"
            | _ ->
                failtest "Products should exist"
    ]
