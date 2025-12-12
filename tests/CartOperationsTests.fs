module CartOperationsTests

open Expecto
open Product
open CartTypes
open CartOperations

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()

let createTestConfig () =
    {
        TaxRate = 0.14m
        ShippingRates = (10m, 20m, 30m)
    }

// ============================================
// CART OPERATIONS TESTS
// ============================================

[<Tests>]
let cartOperationsTests =
    testList "Cart Operations Tests" [
        
        testCase "Add item to empty cart succeeds" <| fun _ ->
            let result = addItem sampleCatalog 1 2 empty
            match result with
            | Ok cart ->
                Expect.isFalse (isEmpty cart) "Cart should not be empty after adding item"
            | Error msg ->
                failtest $"Adding item should succeed: {msg}"
        
        testCase "Add same item twice accumulates quantity" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 2 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2Result = addItem sampleCatalog 1 3 cart1
                match cart2Result with
                | Ok cart2 ->
                    let items = getItems cart2
                    Expect.equal items.[0].Quantity 5 "Quantity should accumulate to 5"
                | Error msg ->
                    failtest $"Second add should succeed: {msg}"
            | Error msg ->
                failtest $"First add should succeed: {msg}"
        
        testCase "Add item exceeding stock fails" <| fun _ ->
            let result = addItem sampleCatalog 1 1000 empty
            match result with
            | Ok _ ->
                failtest "Adding more than stock should fail"
            | Error msg ->
                Expect.stringContains msg "Insufficient stock" "Error should mention insufficient stock"
        
        testCase "Remove item from cart succeeds" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 5 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2Result = removeItem 1 2 cart1
                match cart2Result with
                | Ok cart2 ->
                    let items = getItems cart2
                    Expect.equal items.[0].Quantity 3 "Remaining quantity should be 3"
                | Error msg ->
                    failtest $"Remove should succeed: {msg}"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Checkout updates catalog stock" <| fun _ ->
            let config = createTestConfig()
            let cart1Result = addItem sampleCatalog 1 2 empty
            match cart1Result with
            | Ok cart1 ->
                let originalStock = 
                    match getProduct sampleCatalog 1 with
                    | Some p -> p.Stock
                    | None -> 0
                
                let checkoutResult = checkout config sampleCatalog cart1 0m
                match checkoutResult with
                | Ok (order, updatedCatalog) ->
                    let newStock = 
                        match getProduct updatedCatalog 1 with
                        | Some p -> p.Stock
                        | None -> 0
                    Expect.equal newStock (originalStock - 2) "Stock should be reduced by 2"
                | Error msg ->
                    failtest $"Checkout should succeed: {msg}"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
    ]
