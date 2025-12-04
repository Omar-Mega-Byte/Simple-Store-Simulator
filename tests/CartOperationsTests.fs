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
        
        testCase "Empty cart has no items" <| fun _ ->
            let cart = empty
            Expect.isTrue (isEmpty cart) "Empty cart should be empty"
        
        testCase "Add item to empty cart succeeds" <| fun _ ->
            let result = addItem sampleCatalog 1 2 empty
            match result with
            | Ok cart ->
                Expect.isFalse (isEmpty cart) "Cart should not be empty after adding item"
            | Error msg ->
                failtest $"Adding item should succeed: {msg}"
        
        testCase "Add item with correct quantity" <| fun _ ->
            let result = addItem sampleCatalog 1 3 empty
            match result with
            | Ok cart ->
                let items = getItems cart
                Expect.equal (List.length items) 1 "Cart should have 1 item"
                Expect.equal items.[0].Quantity 3 "Item should have quantity 3"
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
        
        testCase "Add item with zero quantity fails" <| fun _ ->
            let result = addItem sampleCatalog 1 0 empty
            match result with
            | Ok _ ->
                failtest "Adding zero quantity should fail"
            | Error msg ->
                Expect.isNotNull msg "Should return error message"
        
        testCase "Add item with negative quantity fails" <| fun _ ->
            let result = addItem sampleCatalog 1 -5 empty
            match result with
            | Ok _ ->
                failtest "Adding negative quantity should fail"
            | Error msg ->
                Expect.isNotNull msg "Should return error message"
        
        testCase "Add non-existing product fails" <| fun _ ->
            let result = addItem sampleCatalog 999 1 empty
            match result with
            | Ok _ ->
                failtest "Adding non-existing product should fail"
            | Error msg ->
                Expect.stringContains msg "not found" "Error should mention product not found"
        
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
        
        testCase "Remove all quantity removes item from cart" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 3 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2Result = removeItem 1 3 cart1
                match cart2Result with
                | Ok cart2 ->
                    Expect.isTrue (isEmpty cart2) "Cart should be empty after removing all"
                | Error msg ->
                    failtest $"Remove should succeed: {msg}"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Remove more than available quantity removes item" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 3 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2Result = removeItem 1 5 cart1
                match cart2Result with
                | Ok cart2 ->
                    Expect.isTrue (isEmpty cart2) "Cart should be empty"
                | Error msg ->
                    failtest $"Remove should succeed: {msg}"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Remove item not in cart fails" <| fun _ ->
            let result = removeItem 1 1 empty
            match result with
            | Ok _ ->
                failtest "Removing non-existing item should fail"
            | Error msg ->
                Expect.stringContains msg "not in cart" "Error should mention item not in cart"
        
        testCase "Remove zero quantity fails" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 5 empty
            match cart1Result with
            | Ok cart1 ->
                let result = removeItem 1 0 cart1
                match result with
                | Ok _ ->
                    failtest "Removing zero quantity should fail"
                | Error msg ->
                    Expect.isNotNull msg "Should return error message"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Remove item completely removes from cart" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 5 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2 = removeItemCompletely 1 cart1
                Expect.isTrue (isEmpty cart2) "Cart should be empty"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Get items returns all cart items" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 2 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2Result = addItem sampleCatalog 2 3 cart1
                match cart2Result with
                | Ok cart2 ->
                    let items = getItems cart2
                    Expect.equal (List.length items) 2 "Should have 2 items"
                | Error msg ->
                    failtest $"Second add should succeed: {msg}"
            | Error msg ->
                failtest $"First add should succeed: {msg}"
        
        testCase "Clear cart makes it empty" <| fun _ ->
            let cart1Result = addItem sampleCatalog 1 5 empty
            match cart1Result with
            | Ok cart1 ->
                let cart2 = clear cart1
                Expect.isTrue (isEmpty cart2) "Cleared cart should be empty"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
        
        testCase "Checkout empty cart fails" <| fun _ ->
            let config = createTestConfig()
            let result = checkout config sampleCatalog empty
            match result with
            | Ok _ ->
                failtest "Checkout empty cart should fail"
            | Error msg ->
                Expect.stringContains msg "empty" "Error should mention empty cart"
        
        testCase "Checkout updates catalog stock" <| fun _ ->
            let config = createTestConfig()
            let cart1Result = addItem sampleCatalog 1 2 empty
            match cart1Result with
            | Ok cart1 ->
                let originalStock = 
                    match getProduct sampleCatalog 1 with
                    | Some p -> p.Stock
                    | None -> 0
                
                let checkoutResult = checkout config sampleCatalog cart1
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
        
        testCase "Checkout creates order with correct data" <| fun _ ->
            let config = createTestConfig()
            let cart1Result = addItem sampleCatalog 1 2 empty
            match cart1Result with
            | Ok cart1 ->
                let checkoutResult = checkout config sampleCatalog cart1
                match checkoutResult with
                | Ok (order, _) ->
                    Expect.isNotNull order.OrderId "Order should have an ID"
                    Expect.isNonEmpty order.Items "Order should have items"
                    Expect.isGreaterThan order.Total 0m "Order total should be positive"
                | Error msg ->
                    failtest $"Checkout should succeed: {msg}"
            | Error msg ->
                failtest $"Add should succeed: {msg}"
    ]
