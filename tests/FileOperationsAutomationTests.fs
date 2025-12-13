module FileOperationsAutomationTests

open Expecto
open System
open System.IO
open Product
open CartTypes
open CartOperations
open FileIO.FileOperations

// ============================================
// AUTOMATION TEST SUITE FOR FILE I/O OPERATIONS
// ============================================
// Streamlined tests for main file operations features

// ============================================
// TEST DATA SETUP
// ============================================

let sampleCatalog = initializeCatalog()

let createTestOrder () : Order =
    let cart = 
        match addItem sampleCatalog 1 2 empty with
        | Ok c1 -> 
            match addItem sampleCatalog 2 3 c1 with
            | Ok c2 -> c2
            | Error _ -> c1
        | Error _ -> empty
    
    let items = getItems cart
    {
        OrderId = Guid.NewGuid().ToString()
        Items = items
        Subtotal = 60.00m
        Tax = 8.40m
        Shipping = 10.00m
        Discount = 0m
        Total = 78.40m
        Timestamp = DateTime.Now
    }

let createTestCart () : Cart =
    match addItem sampleCatalog 1 2 empty with
    | Ok c1 -> 
        match addItem sampleCatalog 2 3 c1 with
        | Ok c2 -> c2
        | Error _ -> c1
    | Error _ -> empty

let cleanupTestFiles () =
    try
        let dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "data")
        if Directory.Exists(dataFolder) then
            let ordersFolder = Path.Combine(dataFolder, "orders")
            if Directory.Exists(ordersFolder) then
                let files = Directory.GetFiles(ordersFolder, "order_*.json")
                for file in files do
                    try File.Delete(file) with | _ -> ()
                let summaryFiles = Directory.GetFiles(ordersFolder, "order_summary_*.txt")
                for file in summaryFiles do
                    try File.Delete(file) with | _ -> ()
    with
    | _ -> ()

// ============================================
// MAIN FILE OPERATIONS TESTS
// ============================================

[<Tests>]
let fileOperationsTests =
    testList "File Operations Main Tests" [
        
        testCase "Save order creates JSON file successfully" <| fun _ ->
            let order = createTestOrder()
            let saveResult = saveOrder order
            match saveResult with
            | Ok filePath ->
                Expect.isTrue (File.Exists filePath) "Order file should be created"
                Expect.stringContains filePath order.OrderId "Filename should contain order ID"
                Expect.stringEnds filePath ".json" "File should be JSON"
                let fileContent = File.ReadAllText(filePath)
                Expect.isNotEmpty fileContent "File should have content"
                try File.Delete(filePath) with | _ -> ()
            | Error msg ->
                failtest $"Order save should succeed: {msg}"
    ]
