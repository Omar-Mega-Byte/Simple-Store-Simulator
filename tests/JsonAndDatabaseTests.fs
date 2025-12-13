module JsonAndDatabaseTests

open Expecto
open Product
open ProductDatabase
open JsonSerializer
open FileIO.FileOperations
open CartTypes
open System
open System.IO

// ============================================
// DATABASE TESTS
// ============================================

[<Tests>]
let databaseTests =
    testList "Database Tests" [
        
        testCase "Database initializes successfully and creates products table" <| fun _ ->
            // Initialize database (reuses existing if present)
            initializeDatabase()
            
            // Verify database file was created
            Expect.isTrue (File.Exists(Path.Combine("data", "products.db"))) "Database file should be created"
    ]

// ============================================
// JSON SERIALIZATION TESTS
// ============================================

[<Tests>]
let jsonTests =
    testList "JSON Serialization Tests" []
