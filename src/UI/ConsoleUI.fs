module ConsoleUI

// UI Module - Main Console UI Logic
// Developer: علي محمد جمعة زكي
// Integrates all modules and provides the main user interface

open System
open Product
open SearchTypes
open SearchOperations
open DisplayHelpers
open Menu

// ============================================
// APPLICATION STATE (Placeholder for Cart Module)
// ============================================

/// Application state to track catalog and cart
type AppState = {
    Catalog: ProductCatalog
    Cart: Product list  // Placeholder until Cart module is implemented
    // TODO: Replace with proper Cart type when Cart module is ready
}

/// Create initial application state
let createInitialState (catalog: ProductCatalog) : AppState =
    {
        Catalog = catalog
        Cart = []
    }

/// Add product to cart (placeholder implementation)
let addToCart (state: AppState) (product: Product) (quantity: int) : AppState =
    // Simple implementation - add product to list
    // TODO: Replace with Cart module functionality
    let itemsToAdd = List.replicate quantity product
    { state with Cart = state.Cart @ itemsToAdd }

/// Remove product from cart (placeholder implementation)
let removeFromCart (state: AppState) (productId: int) : AppState =
    // Simple implementation - remove first occurrence
    // TODO: Replace with Cart module functionality
    let newCart = 
        state.Cart 
        |> List.filter (fun p -> p.Id <> productId)
    { state with Cart = newCart }

/// Clear cart
let clearCart (state: AppState) : AppState =
    { state with Cart = [] }

/// Get cart total (placeholder implementation)
let getCartTotal (state: AppState) : decimal =
    state.Cart 
    |> List.sumBy (fun p -> p.Price)

/// Get cart item count
let getCartItemCount (state: AppState) : int =
    List.length state.Cart

// ============================================
// VIEW ALL PRODUCTS HANDLER
// ============================================

let handleViewAllProducts (state: AppState) =
    clearScreen()
    displayHeader "📦 PRODUCT CATALOG"
    
    let products = getAllProducts state.Catalog
    
    if List.isEmpty products then
        printInfo "No products available in the catalog."
    else
        // Show sort menu
        printfn ""
        printfn "How would you like to view the products?"
        printfn "  [1] As Table"
        printfn "  [2] Detailed View"
        printfn "  [3] Compact List"
        printfn ""
        displayPrompt "Select view"
        
        let viewChoice = getUserSelection()
        clearScreen()
        displayHeader "📦 PRODUCT CATALOG"
        
        let sortedProducts = sortByName products true
        
        match viewChoice with
        | "1" -> displayProductTable sortedProducts
        | "2" -> sortedProducts |> List.iter displayProductDetailed
        | "3" -> sortedProducts |> List.iter displayProductCompact
        | _ -> displayProductTable sortedProducts
    
    waitForUser()
    state

// ============================================
// SEARCH PRODUCTS HANDLER
// ============================================

let handleSearchProducts (state: AppState) =
    clearScreen()
    displayHeader "🔍 SEARCH PRODUCTS"
    
    printfn ""
    printfn "Search Options:"
    printfn "  [1] Search by name"
    printfn "  [2] Search by name or description"
    printfn "  [3] Back to main menu"
    printfn ""
    displayPrompt "Select option"
    
    let searchChoice = getUserSelection()
    
    match searchChoice with
    | "1" | "2" ->
        printfn ""
        let searchTerm = promptForString "Enter search term"
        
        if String.IsNullOrWhiteSpace(searchTerm) then
            showInfo "No search term entered."
        else
            clearScreen()
            displayHeader (sprintf "🔍 SEARCH RESULTS: '%s'" searchTerm)
            
            let products = getAllProducts state.Catalog
            let results = 
                if searchChoice = "1" then
                    searchByName products searchTerm
                else
                    searchByNameOrDescription products searchTerm
            
            let inStockResults = filterInStockOnly results
            
            if List.isEmpty inStockResults then
                printWarning "No products found matching your search."
            else
                printfn ""
                printSuccess (sprintf "Found %d product(s):" (List.length inStockResults))
                printfn ""
                displayProductTable inStockResults
            
            waitForUser()
    | _ -> ()
    
    state

// ============================================
// BROWSE BY CATEGORY HANDLER
// ============================================

let handleBrowseByCategory (state: AppState) =
    let products = getAllProducts state.Catalog
    let categories = getCategories products
    
    match displayCategorySelection categories with
    | Some selectedCategory ->
        clearScreen()
        displayHeader (sprintf "🏷️  CATEGORY: %s" selectedCategory)
        
        let categoryProducts = filterByCategory products selectedCategory
        let inStock = filterInStockOnly categoryProducts
        let sorted = sortByPrice inStock true
        
        if List.isEmpty sorted then
            printWarning "No products available in this category."
        else
            printfn ""
            displayProductTable sorted
        
        waitForUser()
    | None -> ()
    
    state

// ============================================
// FILTER BY PRICE HANDLER
// ============================================

let handleFilterByPrice (state: AppState) =
    clearScreen()
    displayHeader "💰 FILTER BY PRICE RANGE"
    
    let products = getAllProducts state.Catalog
    
    match getPriceRange products with
    | Some (minPrice, maxPrice) ->
        printfn ""
        printInfo (sprintf "Available price range: %s - %s" 
            (formatPrice minPrice) (formatPrice maxPrice))
        
        match promptForPriceRange() with
        | Some (min, max) ->
            clearScreen()
            displayHeader (sprintf "💰 PRODUCTS: %s - %s" (formatPrice min) (formatPrice max))
            
            let filtered = filterByPriceRange products min max
            let inStock = filterInStockOnly filtered
            let sorted = sortByPrice inStock true
            
            if List.isEmpty sorted then
                printWarning "No products found in this price range."
            else
                printfn ""
                displayProductTable sorted
            
            waitForUser()
        | None ->
            showError "Invalid price range."
    | None ->
        showInfo "No products available."
    
    state

// ============================================
// VIEW CART HANDLER
// ============================================

let handleViewCart (state: AppState) =
    clearScreen()
    displayHeader "🛒 SHOPPING CART"
    
    if List.isEmpty state.Cart then
        printfn ""
        printInfo "Your cart is empty."
        printfn ""
        printfn "Add products from the main menu to start shopping!"
    else
        printfn ""
        
        // Group cart items by product ID and count quantities
        let cartSummary = 
            state.Cart
            |> List.groupBy (fun p -> p.Id)
            |> List.map (fun (id, items) ->
                let product = List.head items
                let quantity = List.length items
                let subtotal = product.Price * decimal quantity
                (product, quantity, subtotal))
        
        // Display cart items
        printfn "┌──────┬─────────────────────┬────────────┬──────────┬──────────────┐"
        printfn "│  ID  │        Name         │   Price    │ Quantity │   Subtotal   │"
        printfn "├──────┼─────────────────────┼────────────┼──────────┼──────────────┤"
        
        cartSummary |> List.iter (fun (product, qty, subtotal) ->
            printf "│ %-4d │ %-19s │ %10s │ %8d │ %12s │\n"
                product.Id
                (truncate product.Name 19)
                (formatPrice product.Price)
                qty
                (formatPrice subtotal))
        
        printfn "└──────┴─────────────────────┴────────────┴──────────┴──────────────┘"
        
        let total = getCartTotal state
        let itemCount = getCartItemCount state
        
        printfn ""
        printfn "Total Items: %d" itemCount
        printfn "Total Amount: %s" (formatPrice total)
    
    printfn ""
    waitForUser()
    state

// ============================================
// CART MANAGEMENT HANDLER
// ============================================

let handleCartManagement (state: AppState) =
    let rec cartLoop currentState =
        let menu = createCartMenu()
        let selection = showMenuAndGetSelection menu
        
        match selection with
        | "0" -> currentState  // Go back
        | "1" ->  // Add product
            clearScreen()
            displayHeader "➕ ADD PRODUCT TO CART"
            
            let products = getAllProducts currentState.Catalog |> filterInStockOnly
            
            if List.isEmpty products then
                showInfo "No products available."
                cartLoop currentState
            else
                printfn ""
                displayProductTable products
                printfn ""
                
                match promptForPositiveInt "Enter product ID" with
                | Some productId ->
                    match getProduct currentState.Catalog productId with
                    | Some product when product.Stock > 0 ->
                        match promptForQuantity (Some product.Stock) with
                        | Some quantity ->
                            let newState = addToCart currentState product quantity
                            displaySuccessScreen 
                                (sprintf "Added %dx %s to cart!" quantity product.Name)
                                [sprintf "Price: %s each" (formatPrice product.Price);
                                 sprintf "Subtotal: %s" (formatPrice (product.Price * decimal quantity))]
                            cartLoop newState
                        | None ->
                            showError "Invalid quantity."
                            cartLoop currentState
                    | Some _ ->
                        showError "Product is out of stock."
                        cartLoop currentState
                    | None ->
                        showError "Product not found."
                        cartLoop currentState
                | None ->
                    showError "Invalid product ID."
                    cartLoop currentState
        
        | "2" ->  // Remove product
            clearScreen()
            displayHeader "➖ REMOVE PRODUCT FROM CART"
            
            if List.isEmpty currentState.Cart then
                showInfo "Your cart is empty."
                cartLoop currentState
            else
                printfn ""
                
                // Show current cart
                let cartProducts = currentState.Cart |> List.distinctBy (fun p -> p.Id)
                cartProducts |> List.iteri (fun i p ->
                    printfn "  [%d] %s - %s" (i + 1) p.Name (formatPrice p.Price))
                
                printfn ""
                
                match promptForPositiveInt "Enter product ID to remove" with
                | Some productId ->
                    if currentState.Cart |> List.exists (fun p -> p.Id = productId) then
                        let newState = removeFromCart currentState productId
                        printSuccess (sprintf "Removed product ID %d from cart." productId)
                        waitForUser()
                        cartLoop newState
                    else
                        showError "Product not in cart."
                        cartLoop currentState
                | None ->
                    showError "Invalid product ID."
                    cartLoop currentState
        
        | "3" ->  // Update quantity (placeholder)
            showInfo "Update quantity feature coming soon! (Requires Cart module)"
            cartLoop currentState
        
        | "4" ->  // View cart
            let _ = handleViewCart currentState
            cartLoop currentState
        
        | "5" ->  // Clear cart
            if List.isEmpty currentState.Cart then
                showInfo "Your cart is already empty."
                cartLoop currentState
            else
                if confirmAction "Are you sure you want to clear your cart?" then
                    let newState = clearCart currentState
                    printSuccess "Cart cleared successfully!"
                    waitForUser()
                    cartLoop newState
                else
                    cartLoop currentState
        
        | _ ->
            handleInvalidSelection()
            cartLoop currentState
    
    cartLoop state

// ============================================
// CHECKOUT HANDLER
// ============================================

let handleCheckout (state: AppState) =
    clearScreen()
    displayHeader "💳 CHECKOUT"
    
    if List.isEmpty state.Cart then
        printfn ""
        printWarning "Your cart is empty!"
        printfn ""
        printfn "Please add products to your cart before checking out."
        waitForUser()
        state
    else
        printfn ""
        printInfo "Order Summary:"
        printfn ""
        
        // Display cart summary
        let cartSummary = 
            state.Cart
            |> List.groupBy (fun p -> p.Id)
            |> List.map (fun (id, items) ->
                let product = List.head items
                let quantity = List.length items
                (product, quantity))
        
        cartSummary |> List.iter (fun (product, qty) ->
            printfn "  • %s x %d = %s" 
                product.Name 
                qty 
                (formatPrice (product.Price * decimal qty)))
        
        let total = getCartTotal state
        let itemCount = getCartItemCount state
        
        printfn ""
        printThinSeparator()
        printfn "  Total Items: %d" itemCount
        printfn "  Total Amount: %s" (formatPrice total)
        printThinSeparator()
        printfn ""
        
        if confirmAction "Proceed with checkout?" then
            showProcessing "Processing your order"
            
            // Simulate order processing
            System.Threading.Thread.Sleep(1000)
            
            let orderId = System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            let orderDate = DateTime.Now
            
            displaySuccessScreen 
                "✓ ORDER COMPLETED SUCCESSFULLY!"
                [sprintf "Order ID: %s" orderId;
                 sprintf "Date: %s" (formatTimestamp orderDate);
                 sprintf "Total: %s" (formatPrice total);
                 sprintf "Items: %d" itemCount;
                 "";
                 "Thank you for your purchase!"]
            
            // Clear cart after successful checkout
            clearCart state
        else
            printInfo "Checkout cancelled."
            waitForUser()
            state

// ============================================
// STATISTICS HANDLER
// ============================================

let handleStatistics (state: AppState) =
    clearScreen()
    displayHeader "📊 PRODUCT STATISTICS"
    
    let products = getAllProducts state.Catalog
    let inStock = filterInStockOnly products
    let outOfStock = filterOutOfStock products
    let lowStock = filterLowStock products 5
    
    let stats = [
        ("Total Products", formatNumber (List.length products))
        ("In Stock", formatNumber (List.length inStock))
        ("Out of Stock", formatNumber (List.length outOfStock))
        ("Low Stock (≤ 5)", formatNumber (List.length lowStock))
    ]
    
    printfn ""
    displayStatistics stats
    
    printfn ""
    printSectionHeader "PRICE STATISTICS"
    
    match getPriceRange products with
    | Some (minPrice, maxPrice) ->
        let avgPrice = products |> List.averageBy (fun p -> float p.Price) |> decimal
        DisplayHelpers.displayKeyValue "Lowest Price" (formatPrice minPrice)
        DisplayHelpers.displayKeyValue "Highest Price" (formatPrice maxPrice)
        DisplayHelpers.displayKeyValue "Average Price" (formatPrice avgPrice)
    | None ->
        printInfo "No price data available."
    
    printfn ""
    printSectionHeader "CATEGORIES"
    
    let categoryCounts = countByCategory products
    categoryCounts |> Map.iter (fun cat count ->
        DisplayHelpers.displayKeyValue cat (formatNumber count))
    
    if not (List.isEmpty lowStock) then
        printfn ""
        printSectionHeader "⚠️  LOW STOCK ITEMS"
        lowStock |> List.iter (fun p ->
            printfn "  • %s: %d units" p.Name p.Stock)
    
    printfn ""
    waitForUser()
    state

// ============================================
// BEST DEALS HANDLER
// ============================================

let handleBestDeals (state: AppState) =
    clearScreen()
    displayHeader "⭐ BEST DEALS"
    
    let products = getAllProducts state.Catalog
    let topDeals = findBestDeals products 5
    
    if List.isEmpty topDeals then
        printWarning "No deals available at the moment."
    else
        printfn ""
        printSuccess "🏆 Top 5 Cheapest Products (In Stock):"
        printfn ""
        
        topDeals |> List.iteri (fun i p ->
            printfn "  %d. %s - %s" (i + 1) p.Name (formatPrice p.Price)
            printfn "     %s" p.Description
            printfn "     Category: %s | Stock: %d units" p.Category p.Stock
            printfn "")
    
    waitForUser()
    state

// ============================================
// MAIN MENU LOOP
// ============================================

let rec mainMenuLoop (state: AppState) =
    let menu = createMainMenu()
    let selection = showMenuAndGetSelection menu
    
    match selection with
    | "0" ->  // Exit
        displayExitScreen()
        state
    
    | "1" ->  // View All Products
        let newState = handleViewAllProducts state
        mainMenuLoop newState
    
    | "2" ->  // Search Products
        let newState = handleSearchProducts state
        mainMenuLoop newState
    
    | "3" ->  // Browse by Category
        let newState = handleBrowseByCategory state
        mainMenuLoop newState
    
    | "4" ->  // Filter by Price
        let newState = handleFilterByPrice state
        mainMenuLoop newState
    
    | "5" ->  // View Cart
        let newState = handleViewCart state
        mainMenuLoop newState
    
    | "6" ->  // Cart Management
        let newState = handleCartManagement state
        mainMenuLoop newState
    
    | "7" ->  // Checkout
        let newState = handleCheckout state
        mainMenuLoop newState
    
    | "8" ->  // Statistics
        let newState = handleStatistics state
        mainMenuLoop newState
    
    | "9" ->  // Best Deals
        let newState = handleBestDeals state
        mainMenuLoop newState
    
    | _ ->
        handleInvalidSelection()
        mainMenuLoop state

// ============================================
// APPLICATION ENTRY POINT
// ============================================

/// Start the application with the given catalog
let startApplication (catalog: ProductCatalog) =
    // Set console encoding for proper emoji/unicode support
    Console.OutputEncoding <- System.Text.Encoding.UTF8
    
    // Display welcome screen
    displayWelcomeScreen()
    
    printfn ""
    printInfo (sprintf "Loaded %d products into catalog." (Map.count catalog))
    printfn ""
    
    // Show quick tips
    displayQuickTips()
    
    waitForUserWithMessage "Press Enter to start shopping... "
    
    // Initialize application state
    let initialState = createInitialState catalog
    
    // Start main menu loop
    let _ = mainMenuLoop initialState
    
    // Application finished
    printfn ""
    printSuccess "Application closed successfully."
    printfn ""

// ============================================
// DEMO MODE
// ============================================

/// Run the application in demo mode with sample data
let runDemo () =
    let catalog = initializeCatalog()
    startApplication catalog
