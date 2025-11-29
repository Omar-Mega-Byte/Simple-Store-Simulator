module Menu

// UI Module - Menu System
// Developer: علي محمد جمعة زكي
// Provides menu navigation and option handling

open System
open DisplayHelpers

// ============================================
// MENU DATA TYPES
// ============================================

/// Menu option definition
type MenuOption = {
    Key: string
    Label: string
    Description: string option
    Icon: string option
}

/// Menu configuration
type Menu = {
    Title: string
    Options: MenuOption list
    ShowExit: bool
}

// ============================================
// MENU CREATION FUNCTIONS
// ============================================

/// Create a menu option
let createMenuOption (key: string) (label: string) (description: string option) (icon: string option) : MenuOption =
    {
        Key = key
        Label = label
        Description = description
        Icon = icon
    }

/// Create a simple menu option (no description or icon)
let createSimpleMenuOption (key: string) (label: string) : MenuOption =
    createMenuOption key label None None

/// Create a menu with options
let createMenu (title: string) (options: MenuOption list) (showExit: bool) : Menu =
    {
        Title = title
        Options = options
        ShowExit = showExit
    }

// ============================================
// MAIN MENU DEFINITIONS
// ============================================

/// Main menu options
let mainMenuOptions = [
    createMenuOption "1" "View All Products" (Some "Browse complete product catalog") (Some "📦")
    createMenuOption "2" "Search Products" (Some "Find products by name") (Some "🔍")
    createMenuOption "3" "Browse by Category" (Some "Filter products by category") (Some "🏷️")
    createMenuOption "4" "Filter by Price Range" (Some "Find products in your budget") (Some "💰")
    createMenuOption "5" "View Cart" (Some "See your shopping cart") (Some "🛒")
    createMenuOption "6" "Shopping Cart Management" (Some "Add/Remove items from cart") (Some "📝")
    createMenuOption "7" "Checkout" (Some "Complete your purchase") (Some "💳")
    createMenuOption "8" "Product Statistics" (Some "View inventory stats") (Some "📊")
    createMenuOption "9" "Best Deals" (Some "See cheapest products") (Some "⭐")
]

/// Create main menu
let createMainMenu () : Menu =
    createMenu "🛒 SIMPLE STORE SIMULATOR 🛒" mainMenuOptions true

// ============================================
// SEARCH MENU DEFINITIONS
// ============================================

/// Search menu options
let searchMenuOptions = [
    createMenuOption "1" "Search by Name" (Some "Search products by name") (Some "🔤")
    createMenuOption "2" "Search by Description" (Some "Search in descriptions") (Some "📝")
    createMenuOption "3" "Advanced Search" (Some "Multi-criteria search") (Some "🔍")
    createMenuOption "4" "View All Products" (Some "Show entire catalog") (Some "📦")
]

/// Create search menu
let createSearchMenu () : Menu =
    createMenu "🔍 SEARCH PRODUCTS" searchMenuOptions true

// ============================================
// CART MENU DEFINITIONS
// ============================================

/// Cart menu options
let cartMenuOptions = [
    createMenuOption "1" "Add Product to Cart" (Some "Add item with quantity") (Some "➕")
    createMenuOption "2" "Remove Product from Cart" (Some "Remove item by ID") (Some "➖")
    createMenuOption "3" "Update Product Quantity" (Some "Change item quantity") (Some "🔄")
    createMenuOption "4" "View Cart Summary" (Some "See cart details") (Some "📋")
    createMenuOption "5" "Clear Cart" (Some "Remove all items") (Some "🗑️")
]

/// Create cart menu
let createCartMenu () : Menu =
    createMenu "🛒 CART MANAGEMENT" cartMenuOptions true

// ============================================
// SORT MENU DEFINITIONS
// ============================================

/// Sort menu options
let sortMenuOptions = [
    createMenuOption "1" "Sort by Name (A-Z)" None (Some "🔤")
    createMenuOption "2" "Sort by Name (Z-A)" None (Some "🔤")
    createMenuOption "3" "Sort by Price (Low to High)" None (Some "💰")
    createMenuOption "4" "Sort by Price (High to Low)" None (Some "💰")
    createMenuOption "5" "Sort by Stock (Low to High)" None (Some "📦")
    createMenuOption "6" "Sort by Stock (High to Low)" None (Some "📦")
]

/// Create sort menu
let createSortMenu () : Menu =
    createMenu "SORT OPTIONS" sortMenuOptions true

// ============================================
// MENU DISPLAY FUNCTIONS
// ============================================

/// Display a single menu option
let displayMenuOption (option: MenuOption) =
    let icon = match option.Icon with | Some i -> i + " " | None -> ""
    let desc = match option.Description with | Some d -> sprintf " - %s" d | None -> ""
    printfn "  [%s] %s%s%s" option.Key icon option.Label desc

/// Display menu header
let displayMenuHeader (title: string) =
    printfn ""
    printfn "╔════════════════════════════════════════════════════════╗"
    printfn "║%s║" (centerText title 56)
    printfn "╚════════════════════════════════════════════════════════╝"
    printfn ""

/// Display a complete menu
let displayMenu (menu: Menu) =
    clearScreen()
    displayMenuHeader menu.Title
    
    // Display all options
    menu.Options |> List.iter displayMenuOption
    
    // Display exit option if enabled
    if menu.ShowExit then
        printfn ""
        printfn "  [0] 🚪 Exit / Go Back"
    
    printfn ""
    displayPrompt "Select an option"

/// Display a compact menu (without descriptions)
let displayCompactMenu (menu: Menu) =
    clearScreen()
    displayMenuHeader menu.Title
    
    menu.Options |> List.iter (fun opt ->
        let icon = match opt.Icon with | Some i -> i + " " | None -> ""
        printfn "  [%s] %s%s" opt.Key icon opt.Label)
    
    if menu.ShowExit then
        printfn ""
        printfn "  [0] 🚪 Exit / Go Back"
    
    printfn ""
    displayPrompt "Select an option"

// ============================================
// MENU NAVIGATION FUNCTIONS
// ============================================

/// Get user's menu selection
let getUserSelection () : string =
    Console.ReadLine().Trim()

/// Validate if selection is valid for menu
let isValidSelection (menu: Menu) (selection: string) : bool =
    if menu.ShowExit && selection = "0" then
        true
    else
        menu.Options |> List.exists (fun opt -> opt.Key = selection)

/// Get menu option by key
let getMenuOption (menu: Menu) (key: string) : MenuOption option =
    menu.Options |> List.tryFind (fun opt -> opt.Key = key)

/// Handle invalid menu selection
let handleInvalidSelection () =
    printError "Invalid option selected. Please try again."
    System.Threading.Thread.Sleep(1500)

/// Show menu and get valid selection
let rec showMenuAndGetSelection (menu: Menu) : string =
    displayMenu menu
    let selection = getUserSelection()
    
    if isValidSelection menu selection then
        selection
    else
        handleInvalidSelection()
        showMenuAndGetSelection menu

/// Show compact menu and get valid selection
let rec showCompactMenuAndGetSelection (menu: Menu) : string =
    displayCompactMenu menu
    let selection = getUserSelection()
    
    if isValidSelection menu selection then
        selection
    else
        handleInvalidSelection()
        showCompactMenuAndGetSelection menu

// ============================================
// CONFIRMATION DIALOGS
// ============================================

/// Ask for yes/no confirmation
let confirmAction (message: string) : bool =
    printfn ""
    displayYesNoPrompt message
    let response = getUserSelection().ToLower()
    response = "y" || response = "yes"

/// Ask for confirmation with default
let confirmActionWithDefault (message: string) (defaultYes: bool) : bool =
    printfn ""
    let defaultText = if defaultYes then "Y/n" else "y/N"
    printf "%s (%s): " message defaultText
    let response = getUserSelection().ToLower()
    
    if String.IsNullOrWhiteSpace(response) then
        defaultYes
    else
        response = "y" || response = "yes"

/// Display confirmation message and wait
let showConfirmation (message: string) =
    printSuccess message
    waitForUser()

/// Display error and wait
let showError (message: string) =
    printError message
    waitForUser()

/// Display warning and wait
let showWarning (message: string) =
    printWarning message
    waitForUser()

/// Display info and wait
let showInfo (message: string) =
    printInfo message
    waitForUser()

// ============================================
// INPUT PROMPTS
// ============================================

/// Prompt for integer input
let promptForInt (message: string) : int option =
    displayPrompt message
    match Int32.TryParse(getUserSelection()) with
    | (true, value) -> Some value
    | _ -> None

/// Prompt for positive integer
let promptForPositiveInt (message: string) : int option =
    match promptForInt message with
    | Some value when value > 0 -> Some value
    | _ -> None

/// Prompt for decimal input
let promptForDecimal (message: string) : decimal option =
    displayPrompt message
    match Decimal.TryParse(getUserSelection()) with
    | (true, value) -> Some value
    | _ -> None

/// Prompt for positive decimal
let promptForPositiveDecimal (message: string) : decimal option =
    match promptForDecimal message with
    | Some value when value > 0m -> Some value
    | _ -> None

/// Prompt for string input
let promptForString (message: string) : string =
    displayPrompt message
    getUserSelection()

/// Prompt for non-empty string
let promptForNonEmptyString (message: string) : string option =
    let input = promptForString message
    if String.IsNullOrWhiteSpace(input) then None
    else Some input

/// Prompt for string with default value
let promptForStringWithDefault (message: string) (defaultValue: string) : string =
    displayPromptWithDefault message defaultValue
    let input = getUserSelection()
    if String.IsNullOrWhiteSpace(input) then defaultValue
    else input

// ============================================
// BREADCRUMB NAVIGATION
// ============================================

/// Display navigation breadcrumb
let displayBreadcrumb (path: string list) =
    if not (List.isEmpty path) then
        printfn ""
        printf "📍 "
        path |> List.iteri (fun i item ->
            if i > 0 then printf " › "
            printf "%s" item)
        printfn ""
        printfn ""

/// Display current location in menu hierarchy
let displayLocation (location: string) =
    printfn ""
    printfn "Current Location: %s" location
    printfn ""

// ============================================
// HELP & INSTRUCTIONS
// ============================================

/// Display help text
let displayHelp (helpText: string list) =
    printfn ""
    printSectionHeader "HELP"
    helpText |> List.iter (fun line -> printfn "  %s" line)
    printfn ""
    waitForUser()

/// Display quick tips
let displayQuickTips () =
    let tips = [
        "Press '0' to go back to the previous menu"
        "You can search products by typing their name"
        "Use price filters to find products in your budget"
        "Check cart before checkout to review your order"
        "Low stock items are marked with a warning"
    ]
    
    printfn ""
    printSectionHeader "💡 QUICK TIPS"
    displayBulletList tips
    printfn ""

/// Display keyboard shortcuts
let displayShortcuts () =
    let shortcuts = [
        "0 - Go Back / Exit"
        "1-9 - Quick menu selection"
        "Enter - Confirm selection"
        "Ctrl+C - Force exit"
    ]
    
    printfn ""
    printSectionHeader "⌨️  KEYBOARD SHORTCUTS"
    displayBulletList shortcuts
    printfn ""

// ============================================
// CATEGORY SELECTION
// ============================================

/// Display category selection menu
let rec displayCategorySelection (categories: string list) : string option =
    if List.isEmpty categories then
        showInfo "No categories available."
        None
    else
        clearScreen()
        displayMenuHeader "SELECT CATEGORY"
        
        categories |> List.iteri (fun i cat ->
            printfn "  [%d] %s" (i + 1) cat)
        
        printfn ""
        printfn "  [0] 🚪 Go Back"
        printfn ""
        
        displayPrompt "Select category"
        
        match promptForInt "" with
        | Some 0 -> None
        | Some num when num > 0 && num <= List.length categories ->
            Some categories.[num - 1]
        | _ ->
            handleInvalidSelection()
            displayCategorySelection categories

// ============================================
// PROGRESS INDICATORS
// ============================================

/// Show loading screen
let showLoading (message: string) =
    clearScreen()
    printfn ""
    printfn ""
    printfn "%s" (centerText message 60)
    printfn ""
    printfn "%s" (centerText "Please wait..." 60)
    printfn ""

/// Show processing animation
let showProcessing (operation: string) =
    printf "%s" operation
    for _ in 1..3 do
        System.Threading.Thread.Sleep(300)
        printf "."
    printfn " Done!"

// ============================================
// TABLE SELECTION
// ============================================

/// Display a numbered list and get selection
let rec selectFromList (title: string) (items: string list) : int option =
    if List.isEmpty items then
        showInfo "No items available."
        None
    else
        clearScreen()
        displayMenuHeader title
        
        items |> List.iteri (fun i item ->
            printfn "  [%d] %s" (i + 1) item)
        
        printfn ""
        printfn "  [0] Cancel"
        printfn ""
        
        displayPrompt "Select item number"
        
        match promptForInt "" with
        | Some 0 -> None
        | Some num when num > 0 && num <= List.length items ->
            Some (num - 1)  // Return zero-based index
        | _ ->
            handleInvalidSelection()
            selectFromList title items

// ============================================
// QUANTITY SELECTION
// ============================================

/// Prompt for quantity with validation
let promptForQuantity (maxAvailable: int option) : int option =
    let message = 
        match maxAvailable with
        | Some max -> sprintf "Enter quantity (1-%d)" max
        | None -> "Enter quantity"
    
    match promptForPositiveInt message with
    | Some qty ->
        match maxAvailable with
        | Some max when qty > max ->
            printError (sprintf "Only %d units available." max)
            None
        | _ -> Some qty
    | None ->
        printError "Invalid quantity."
        None

// ============================================
// PRICE RANGE INPUT
// ============================================

/// Prompt for price range
let promptForPriceRange () : (decimal * decimal) option =
    printfn ""
    printfn "Enter price range:"
    
    match promptForPositiveDecimal "Minimum price (EGP)" with
    | Some minPrice ->
        match promptForPositiveDecimal "Maximum price (EGP)" with
        | Some maxPrice when maxPrice >= minPrice ->
            Some (minPrice, maxPrice)
        | Some _ ->
            printError "Maximum price must be greater than or equal to minimum price."
            None
        | None ->
            printError "Invalid maximum price."
            None
    | None ->
        printError "Invalid minimum price."
        None

// ============================================
// SUCCESS/FAILURE SCREENS
// ============================================

/// Display success screen with details
let displaySuccessScreen (title: string) (details: string list) =
    clearScreen()
    printfn ""
    printSuccess title
    printfn ""
    details |> List.iter (fun detail -> printfn "  %s" detail)
    printfn ""
    waitForUser()

/// Display failure screen with error
let displayFailureScreen (title: string) (error: string) =
    clearScreen()
    printfn ""
    printError title
    printfn ""
    printfn "  %s" error
    printfn ""
    waitForUser()
