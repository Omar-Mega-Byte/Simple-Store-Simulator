// Simple Store Simulator - Main Program
// Programming Languages - 3 Course Project
// Team Leader: عمر أحمد الرفاعي طليس
// Catalog Developer: عمر أحمد محمود عواد
//IMPORATANT!
//TEMPORARY TILL WE ADD UI
open Product
open SearchTypes
open SearchOperations
open System

// ============================================
// MAIN MENU SYSTEM
// ============================================

let displayMainMenu () =
    printfn ""
    printfn "╔══════════════════════════════════════════╗"
    printfn "║     🛒 SIMPLE STORE SIMULATOR 🛒        ║"
    printfn "║        TEMPORARY TILL WE ADD UI          ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    printfn "1. 📦 View All Products"
    printfn "2. 🔍 Search Products"
    printfn "3. 🏷️  Browse by Category"
    printfn "4. 💰 Filter by Price Range"
    printfn "5. 📊 View Product Statistics"
    printfn "6. ⭐ Best Deals"
    printfn "7. 🧪 Run Demo Examples"
    printfn "0. 🚪 Exit"
    printfn ""
    printf "Select an option: "

let getUserInput () : string =
    Console.ReadLine()

let getUserInputInt () : int option =
    match Int32.TryParse(getUserInput()) with
    | (true, value) -> Some value
    | _ -> None

let waitForUser () =
    printfn ""
    printf "Press Enter to continue..."
    Console.ReadLine() |> ignore
    Console.Clear()

// ============================================
// MENU HANDLERS
// ============================================

let handleViewAllProducts (catalog: ProductCatalog) =
    Console.Clear()
    let products = getAllProducts catalog
    let sortConfig = { SortBy = ByName; Order = Ascending }
    let sorted = sortProducts products sortConfig
    
    printfn "╔══════════════════════════════════════════╗"
    printfn "║          📦 PRODUCT CATALOG             ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    sorted |> List.iter displayProduct
    printfn "Total products: %d" (List.length sorted)
    waitForUser()

let handleSearchProducts (catalog: ProductCatalog) =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║          🔍 SEARCH PRODUCTS             ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    printf "Enter search term (name or description): "
    let searchTerm = getUserInput()
    
    if String.IsNullOrWhiteSpace(searchTerm) then
        printfn "No search term entered."
        waitForUser()
    else
        let products = getAllProducts catalog
        let criteria = {
            SearchTerm = Some searchTerm
            Category = None
            PriceRange = None
            StockRange = None
            InStockOnly = true
        }
        let sortConfig = Some { SortBy = ByPrice; Order = Ascending }
        let result = searchWithMetadata products criteria sortConfig
        
        printfn ""
        displaySearchResults result
        waitForUser()

let handleBrowseByCategory (catalog: ProductCatalog) =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║       🏷️  BROWSE BY CATEGORY           ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    let products = getAllProducts catalog
    let categories = getCategories products
    
    printfn "Available Categories:"
    categories |> List.iteri (fun i cat -> printfn "  %d. %s" (i + 1) cat)
    printfn ""
    printf "Select category number: "
    
    match getUserInputInt() with
    | Some num when num > 0 && num <= List.length categories ->
        let selectedCategory = categories.[num - 1]
        Console.Clear()
        printfn "╔══════════════════════════════════════════╗"
        printfn "║     Category: %-26s ║" selectedCategory
        printfn "╚══════════════════════════════════════════╝"
        printfn ""
        
        let filtered = filterByCategory products selectedCategory
        let sorted = sortByPrice filtered true
        
        sorted |> List.iter displayProduct
        printfn "Products in this category: %d" (List.length sorted)
        waitForUser()
    | _ ->
        printfn "Invalid selection."
        waitForUser()

let handleFilterByPrice (catalog: ProductCatalog) =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║      💰 FILTER BY PRICE RANGE           ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    let products = getAllProducts catalog
    
    match getPriceRange products with
    | Some (minPrice, maxPrice) ->
        printfn "Price range in catalog: EGP %.2f - EGP %.2f" minPrice maxPrice
        printfn ""
        
        printf "Enter minimum price (EGP): "
        let minInput = getUserInput()
        printf "Enter maximum price (EGP): "
        let maxInput = getUserInput()
        
        match Decimal.TryParse(minInput), Decimal.TryParse(maxInput) with
        | (true, min), (true, max) when min >= 0m && max >= min ->
            Console.Clear()
            printfn "╔══════════════════════════════════════════╗"
            printfn "║   Products: EGP %.2f - EGP %.2f     ║" min max
            printfn "╚══════════════════════════════════════════╝"
            printfn ""
            
            let filtered = filterByPriceRange products min max
            let sorted = sortByPrice (filterInStockOnly filtered) true
            
            sorted |> List.iter displayProduct
            printfn "Products found: %d" (List.length sorted)
            waitForUser()
        | _ ->
            printfn "Invalid price range."
            waitForUser()
    | None ->
        printfn "No products in catalog."
        waitForUser()

let handleStatistics (catalog: ProductCatalog) =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║       📊 PRODUCT STATISTICS             ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    let products = getAllProducts catalog
    
    printfn "📦 Total Products: %d" (List.length products)
    printfn "✅ In Stock: %d" (filterInStockOnly products |> List.length)
    printfn "❌ Out of Stock: %d" (filterOutOfStock products |> List.length)
    printfn ""
    
    match getPriceRange products with
    | Some (minPrice, maxPrice) ->
        printfn "💰 Price Range: EGP %.2f - EGP %.2f" minPrice maxPrice
        let avgPrice = products |> List.averageBy (fun p -> float p.Price) |> decimal
        printfn "💵 Average Price: EGP %.2f" avgPrice
    | None -> ()
    
    printfn ""
    printfn "📂 Products by Category:"
    let categoryCounts = countByCategory products
    categoryCounts |> Map.iter (fun cat count -> printfn "   %s: %d" cat count)
    
    printfn ""
    printfn "⚠️  Low Stock Items (≤ 5):"
    let lowStock = findRestockNeeded products 5
    if List.isEmpty lowStock then
        printfn "   None"
    else
        lowStock |> List.iter (fun p -> printfn "   - %s: %d units" p.Name p.Stock)
    
    waitForUser()

let handleBestDeals (catalog: ProductCatalog) =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║          ⭐ BEST DEALS ⭐               ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    let products = getAllProducts catalog
    let topDeals = findBestDeals products 5
    
    printfn "🏆 Top 5 Cheapest Products (In Stock):"
    printfn ""
    
    topDeals |> List.iteri (fun i p ->
        printfn "  %d. %s - %s" (i + 1) p.Name (formatPrice p.Price)
        printfn "     %s" p.Description
        printfn "     Stock: %d units" p.Stock
        printfn "")
    
    waitForUser()

let handleRunExamples () =
    Console.Clear()
    printfn "╔══════════════════════════════════════════╗"
    printfn "║      🧪 RUNNING DEMO EXAMPLES           ║"
    printfn "╚══════════════════════════════════════════╝"
    printfn ""
    
    runExamples()
    waitForUser()

// ============================================
// MAIN PROGRAM LOOP
// ============================================

let rec mainLoop (catalog: ProductCatalog) =
    displayMainMenu()
    
    match getUserInput() with
    | "1" -> 
        handleViewAllProducts catalog
        mainLoop catalog
    | "2" -> 
        handleSearchProducts catalog
        mainLoop catalog
    | "3" -> 
        handleBrowseByCategory catalog
        mainLoop catalog
    | "4" -> 
        handleFilterByPrice catalog
        mainLoop catalog
    | "5" -> 
        handleStatistics catalog
        mainLoop catalog
    | "6" -> 
        handleBestDeals catalog
        mainLoop catalog
    | "7" -> 
        handleRunExamples()
        mainLoop catalog
    | "0" -> 
        Console.Clear()
        printfn ""
        printfn "╔══════════════════════════════════════════╗"
        printfn "║   Thank you for using our store!         ║"
        printfn "║                                          ║"
        printfn "║   Team: Omar Ahmed Elrfaat & Team        ║"
        printfn "╚══════════════════════════════════════════╝"
        printfn ""
    | _ -> 
        printfn "Invalid option. Please try again."
        System.Threading.Thread.Sleep(1000)
        mainLoop catalog

// ============================================
// PROGRAM ENTRY POINT
// ============================================

[<EntryPoint>]
let main argv =
    Console.Clear()
    Console.OutputEncoding <- System.Text.Encoding.UTF8
    
    printfn ""
    printfn "╔════════════════════════════════════════════════════╗"
    printfn "║                                                    ║"
    printfn "║        🛒 SIMPLE STORE SIMULATOR 🛒               ║"
    printfn "║                                                    ║"
    printfn "║     F# Functional Programming Project              ║"
    printfn "║     Programming Languages - 3 Course               ║"
    printfn "║                                                    ║"
    printfn "║     Team Leader: Omar Ahmed Elrfaay                ║"
    printfn "║     Catalog Dev: Omar Ahmed Mahmoud Awad           ║"
    printfn "║                                                    ║"
    printfn "╚════════════════════════════════════════════════════╝"
    printfn ""
    printfn "Initializing catalog..."
    
    let catalog = initializeCatalog()
    let productCount = Map.count catalog
    
    printfn "✅ Loaded %d products" productCount
    printfn ""
    printf "Press Enter to start..."
    Console.ReadLine() |> ignore
    
    Console.Clear()
    mainLoop catalog
    
    0 // Return success exit code
