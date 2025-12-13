module ProductTests

open Expecto
open Product
open PriceCalculator

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
        
        testCase "Initialize catalog should contain products from database" <| fun _ ->
            let catalog = initializeCatalog()
            let productCount = catalog |> Map.count
            Expect.isGreaterThanOrEqual productCount 33 "Catalog should have at least 33 products from database"
        
        testCase "Get existing product returns correct data" <| fun _ ->
            let result = getProduct sampleCatalog 1
            match result with
            | Some product ->
                Expect.equal product.Name "Chocolate" "Product name should be Chocolate"
                Expect.equal product.Price 15.00m "Product price should be 15.00"
                Expect.equal product.Category "Sweets" "Product category should be Sweets"
            | None ->
                failtest "Product should exist"
        
        testCase "Update stock changes product stock correctly" <| fun _ ->
            let updatedCatalog = updateStock sampleCatalog 1 50
            match getProduct updatedCatalog 1 with
            | Some product ->
                Expect.equal product.Stock 50 "Stock should be updated to 50"
            | None ->
                failtest "Product should exist"
    ]
