module ProductTests

open Expecto
open Product

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()

// ============================================
// PRODUCT CATALOG TESTS
// ============================================

[<Tests>]
let productTests =
    testList "Product Module Tests" [
        
        testCase "Initialize catalog should contain 35 products" <| fun _ ->
            let catalog = initializeCatalog()
            let productCount = catalog |> Map.count
            Expect.equal productCount 35 "Catalog should have exactly 35 products"
        
        testCase "Get existing product by ID returns Some" <| fun _ ->
            let result = getProduct sampleCatalog 1
            Expect.isSome result "Product with ID 1 should exist"
        
        testCase "Get existing product returns correct data" <| fun _ ->
            let result = getProduct sampleCatalog 1
            match result with
            | Some product ->
                Expect.equal product.Name "Chocolate" "Product name should be Chocolate"
                Expect.equal product.Price 15.00m "Product price should be 15.00"
                Expect.equal product.Category "Sweets" "Product category should be Sweets"
            | None ->
                failtest "Product should exist"
        
        testCase "Get non-existing product returns None" <| fun _ ->
            let result = getProduct sampleCatalog 999
            Expect.isNone result "Product with ID 999 should not exist"
        
        testCase "Get all products returns correct count" <| fun _ ->
            let allProducts = getAllProducts sampleCatalog
            Expect.equal (List.length allProducts) 35 "Should return all 35 products"
        
        testCase "Get products by category filters correctly" <| fun _ ->
            let snacks = getProductsByCategory sampleCatalog "Snacks"
            Expect.isNonEmpty snacks "Should find products in Snacks category"
            Expect.all snacks (fun p -> p.Category = "Snacks") "All products should be in Snacks category"
        
        testCase "Get products by category is case insensitive" <| fun _ ->
            let snacks1 = getProductsByCategory sampleCatalog "Snacks"
            let snacks2 = getProductsByCategory sampleCatalog "SNACKS"
            let snacks3 = getProductsByCategory sampleCatalog "snacks"
            Expect.equal (List.length snacks1) (List.length snacks2) "Case should not matter"
            Expect.equal (List.length snacks2) (List.length snacks3) "Case should not matter"
        
        testCase "Is in stock returns true when sufficient quantity" <| fun _ ->
            match getProduct sampleCatalog 1 with
            | Some product ->
                let result = isInStock product 5
                Expect.isTrue result "Should be in stock with sufficient quantity"
            | None ->
                failtest "Product should exist"
        
        testCase "Is in stock returns false when insufficient quantity" <| fun _ ->
            match getProduct sampleCatalog 1 with
            | Some product ->
                let result = isInStock product 100
                Expect.isFalse result "Should not be in stock with insufficient quantity"
            | None ->
                failtest "Product should exist"
        
        testCase "Is in stock returns true when exact quantity" <| fun _ ->
            match getProduct sampleCatalog 1 with
            | Some product ->
                let result = isInStock product product.Stock
                Expect.isTrue result "Should be in stock with exact quantity"
            | None ->
                failtest "Product should exist"
        
        testCase "Update stock changes product stock correctly" <| fun _ ->
            let updatedCatalog = updateStock sampleCatalog 1 50
            match getProduct updatedCatalog 1 with
            | Some product ->
                Expect.equal product.Stock 50 "Stock should be updated to 50"
            | None ->
                failtest "Product should exist"
        
        testCase "Update stock for non-existing product returns unchanged catalog" <| fun _ ->
            let updatedCatalog = updateStock sampleCatalog 999 100
            Expect.equal updatedCatalog sampleCatalog "Catalog should remain unchanged"
        
        testCase "Format price displays correct currency format" <| fun _ ->
            let formatted = formatPrice 15.50m
            Expect.equal formatted "EGP 15.50" "Price should be formatted as EGP currency"
        
        testCase "Format price handles whole numbers correctly" <| fun _ ->
            let formatted = formatPrice 20m
            Expect.equal formatted "EGP 20.00" "Price should show two decimal places"
    ]
