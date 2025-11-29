module DisplayHelpers

// UI Module - Display Helpers
// Developer: علي محمد جمعة زكي
// Provides formatting and display functions for the user interface

open System
open Product

// ============================================
// CONSTANTS & CONFIGURATION
// ============================================

/// Console colors for different message types
module Colors =
    let success = ConsoleColor.Green
    let error = ConsoleColor.Red
    let warning = ConsoleColor.Yellow
    let info = ConsoleColor.Cyan
    let highlight = ConsoleColor.Magenta
    let normal = ConsoleColor.White

/// UI configuration constants
module UIConfig =
    let lineWidth = 60
    let separator = String('═', lineWidth)
    let thinSeparator = String('─', lineWidth)
    let currencySymbol = "EGP"

// ============================================
// BASIC FORMATTING FUNCTIONS
// ============================================

/// Format a decimal price as currency
let formatPrice (price: decimal) : string =
    sprintf "%s %.2f" UIConfig.currencySymbol price

/// Format a large number with thousand separators
let formatNumber (number: int) : string =
    number.ToString("N0")

/// Center text within a given width
let centerText (text: string) (width: int) : string =
    let padding = max 0 ((width - text.Length) / 2)
    text.PadLeft(padding + text.Length).PadRight(width)

/// Pad text to the right
let padRight (text: string) (width: int) : string =
    if text.Length >= width then text
    else text.PadRight(width)

/// Pad text to the left
let padLeft (text: string) (width: int) : string =
    if text.Length >= width then text
    else text.PadLeft(width)

/// Truncate text if too long
let truncate (text: string) (maxLength: int) : string =
    if text.Length <= maxLength then text
    else text.Substring(0, maxLength - 3) + "..."

// ============================================
// CONSOLE OUTPUT FUNCTIONS
// ============================================

/// Clear the console screen
let clearScreen () =
    Console.Clear()

/// Print a blank line
let printBlankLine () =
    printfn ""

/// Print multiple blank lines
let printBlankLines (count: int) =
    for _ in 1..count do printfn ""

/// Wait for user to press Enter
let waitForUser () =
    printfn ""
    printf "Press Enter to continue..."
    Console.ReadLine() |> ignore

/// Wait for user with custom message
let waitForUserWithMessage (message: string) =
    printfn ""
    printf "%s" message
    Console.ReadLine() |> ignore

// ============================================
// COLORED MESSAGE FUNCTIONS
// ============================================

/// Print text in a specific color
let printColored (color: ConsoleColor) (text: string) =
    let originalColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    printfn "%s" text
    Console.ForegroundColor <- originalColor

/// Print success message (green)
let printSuccess (message: string) =
    printColored Colors.success (sprintf "✓ %s" message)

/// Print error message (red)
let printError (message: string) =
    printColored Colors.error (sprintf "✗ %s" message)

/// Print warning message (yellow)
let printWarning (message: string) =
    printColored Colors.warning (sprintf "⚠ %s" message)

/// Print info message (cyan)
let printInfo (message: string) =
    printColored Colors.info (sprintf "ℹ %s" message)

/// Print highlighted message (magenta)
let printHighlight (message: string) =
    printColored Colors.highlight message

// ============================================
// BOX & BORDER FUNCTIONS
// ============================================

/// Print a separator line
let printSeparator () =
    printfn "%s" UIConfig.separator

/// Print a thin separator line
let printThinSeparator () =
    printfn "%s" UIConfig.thinSeparator

/// Print a header box with title
let printHeader (title: string) =
    printfn "╔%s╗" UIConfig.separator
    printfn "║%s║" (centerText title UIConfig.lineWidth)
    printfn "╚%s╝" UIConfig.separator

/// Alias for printHeader (for consistency)
let displayHeader = printHeader

/// Print a simple header
let printSimpleHeader (title: string) =
    printfn "%s" UIConfig.separator
    printfn "%s" (centerText title UIConfig.lineWidth)
    printfn "%s" UIConfig.separator

/// Print a section header
let printSectionHeader (title: string) =
    printfn ""
    printfn "═══ %s ═══" title
    printfn ""

/// Print a box around text
let printBox (lines: string list) =
    let maxLength = 
        if List.isEmpty lines then 0
        else lines |> List.map (fun s -> s.Length) |> List.max
    
    let width = min maxLength UIConfig.lineWidth
    let border = String('═', width + 2)
    
    printfn "╔%s╗" border
    lines |> List.iter (fun line ->
        printfn "║ %s ║" (padRight line width))
    printfn "╚%s╝" border

// ============================================
// PRODUCT DISPLAY FUNCTIONS
// ============================================

/// Display a single product in compact format
let displayProductCompact (product: Product) =
    let stockStatus = 
        if product.Stock > 10 then "✓ In Stock"
        elif product.Stock > 0 then sprintf "⚠ Low (%d)" product.Stock
        else "✗ Out of Stock"
    
    let stockColor = 
        if product.Stock > 10 then Colors.success
        elif product.Stock > 0 then Colors.warning
        else Colors.error
    
    printf "  [%d] %-25s %12s  " product.Id (truncate product.Name 25) (formatPrice product.Price)
    printColored stockColor stockStatus

/// Display a single product in detailed format
let displayProductDetailed (product: Product) =
    printfn "┌─────────────────────────────────────────────────────────┐"
    printfn "│ ID: %-4d %-48s│" product.Id product.Name
    printfn "├─────────────────────────────────────────────────────────┤"
    printfn "│ Price:       %-44s│" (formatPrice product.Price)
    printfn "│ Category:    %-44s│" product.Category
    printfn "│ Stock:       %-44s│" 
        (if product.Stock > 0 then sprintf "%d units available" product.Stock
         else "Out of Stock")
    printfn "│ Description: %-44s│" (truncate product.Description 44)
    printfn "└─────────────────────────────────────────────────────────┘"

/// Display a single product in table row format
let displayProductRow (index: int) (product: Product) =
    printf "│ %-4d │ %-20s │ %10s │ %-12s │ %6d │\n" 
        index
        (truncate product.Name 20)
        (formatPrice product.Price)
        (truncate product.Category 12)
        product.Stock

/// Display a list of products in a table
let displayProductTable (products: Product list) =
    if List.isEmpty products then
        printInfo "No products to display."
    else
        printfn "┌──────┬──────────────────────┬────────────┬──────────────┬────────┐"
        printfn "│  ID  │        Name          │   Price    │   Category   │ Stock  │"
        printfn "├──────┼──────────────────────┼────────────┼──────────────┼────────┤"
        products |> List.iter (fun p -> displayProductRow p.Id p)
        printfn "└──────┴──────────────────────┴────────────┴──────────────┴────────┘"
        printfn ""
        printfn "Total products: %d" (List.length products)

/// Display products with numbering
let displayProductsNumbered (products: Product list) =
    if List.isEmpty products then
        printInfo "No products to display."
    else
        products |> List.iteri (fun i product ->
            printfn "%d. %s - %s" (i + 1) product.Name (formatPrice product.Price)
            printfn "   %s" product.Description
            printfn "   Category: %s | Stock: %d units" product.Category product.Stock
            printfn "")

// ============================================
// CART DISPLAY FUNCTIONS (Placeholder for Cart Module)
// ============================================

/// Display cart summary (to be implemented with Cart module)
let displayCartSummary (itemCount: int) (total: decimal) =
    printfn ""
    printfn "╔════════════════════════════════════════╗"
    printfn "║           🛒 CART SUMMARY             ║"
    printfn "╠════════════════════════════════════════╣"
    printfn "║ Items:       %-25s ║" (formatNumber itemCount)
    printfn "║ Total:       %-25s ║" (formatPrice total)
    printfn "╚════════════════════════════════════════╝"

// ============================================
// STATISTICS & SUMMARY FUNCTIONS
// ============================================

/// Display statistics box
let displayStatistics (stats: (string * string) list) =
    printfn "╔════════════════════════════════════════════════════════╗"
    printfn "║                    📊 STATISTICS                       ║"
    printfn "╠════════════════════════════════════════════════════════╣"
    stats |> List.iter (fun (label, value) ->
        printfn "║ %-30s %-22s ║" label value)
    printfn "╚════════════════════════════════════════════════════════╝"

/// Display a simple key-value pair
let displayKeyValue (key: string) (value: string) =
    printfn "%-25s: %s" key value

/// Display a list of items with bullets
let displayBulletList (items: string list) =
    items |> List.iter (fun item -> printfn "  • %s" item)

/// Display a numbered list
let displayNumberedList (items: string list) =
    items |> List.iteri (fun i item -> printfn "  %d. %s" (i + 1) item)

// ============================================
// PROGRESS & STATUS INDICATORS
// ============================================

/// Display a progress bar
let displayProgressBar (current: int) (total: int) (width: int) =
    let percentage = float current / float total
    let filled = int (percentage * float width)
    let empty = width - filled
    
    printf "["
    printf "%s" (String('█', filled))
    printf "%s" (String('░', empty))
    printf "] %d%%" (int (percentage * 100.0))
    printfn ""

/// Display loading animation (simple spinner)
let displayLoadingMessage (message: string) =
    printf "%s... " message
    Console.Out.Flush()

/// Display completion message
let displayCompletionMessage (message: string) =
    printSuccess message
    printfn ""

// ============================================
// INPUT PROMPT FUNCTIONS
// ============================================

/// Display a prompt for user input
let displayPrompt (message: string) =
    printf "%s: " message

/// Display a prompt with default value
let displayPromptWithDefault (message: string) (defaultValue: string) =
    printf "%s [%s]: " message defaultValue

/// Display a yes/no prompt
let displayYesNoPrompt (message: string) =
    printf "%s (y/n): " message

/// Display a selection prompt
let displaySelectionPrompt (options: string list) =
    printfn ""
    printfn "Please select an option:"
    options |> List.iteri (fun i opt -> printfn "  %d. %s" (i + 1) opt)
    printfn ""
    displayPrompt "Enter your choice"

// ============================================
// ERROR & VALIDATION DISPLAY
// ============================================

/// Display validation error
let displayValidationError (field: string) (error: string) =
    printError (sprintf "%s: %s" field error)

/// Display multiple validation errors
let displayValidationErrors (errors: (string * string) list) =
    printfn ""
    printError "Please fix the following errors:"
    errors |> List.iter (fun (field, error) ->
        printfn "  • %s: %s" field error)
    printfn ""

/// Display not found message
let displayNotFound (itemType: string) =
    printWarning (sprintf "%s not found." itemType)

/// Display empty list message
let displayEmptyList (listType: string) =
    printInfo (sprintf "No %s available." listType)

// ============================================
// WELCOME & EXIT SCREENS
// ============================================

/// Display welcome screen
let displayWelcomeScreen () =
    clearScreen()
    printfn ""
    printfn "╔════════════════════════════════════════════════════════╗"
    printfn "║                                                        ║"
    printfn "║              🛒 SIMPLE STORE SIMULATOR 🛒             ║"
    printfn "║                                                        ║"
    printfn "║           F# Functional Programming Project            ║"
    printfn "║           Programming Languages - 3 Course             ║"
    printfn "║                                                        ║"
    printfn "║                    Team Members:                       ║"
    printfn "║         • Omar Ahmed Elrfaay (Team Leader)            ║"
    printfn "║         • Ali Mohamed Gomaa Zaki (UI Dev)             ║"
    printfn "║         • Omar Ahmed Mahmoud Awad (Catalog)           ║"
    printfn "║         • Basel Waleed Hamed (Cart Logic)             ║"
    printfn "║         • Michael Emad Adly (File I/O)                ║"
    printfn "║         • Omar Ahmed Mohamed (Calculator)             ║"
    printfn "║         • Jamal Ayman Abdulrahman (Tester)            ║"
    printfn "║         • Kyrillos Sary Eid (Documentation)           ║"
    printfn "║                                                        ║"
    printfn "╚════════════════════════════════════════════════════════╝"
    printfn ""

/// Display exit screen
let displayExitScreen () =
    clearScreen()
    printfn ""
    printfn "╔════════════════════════════════════════════════════════╗"
    printfn "║                                                        ║"
    printfn "║            Thank you for using our store! 🛒          ║"
    printfn "║                                                        ║"
    printfn "║              We hope to see you again soon!            ║"
    printfn "║                                                        ║"
    printfn "║                Simple Store Simulator                  ║"
    printfn "║            Built with ❤️ using F# & .NET              ║"
    printfn "║                                                        ║"
    printfn "╚════════════════════════════════════════════════════════╝"
    printfn ""

// ============================================
// HELPER UTILITIES
// ============================================

/// Format a timestamp
let formatTimestamp (dateTime: DateTime) : string =
    dateTime.ToString("yyyy-MM-dd HH:mm:ss")

/// Format a time span
let formatDuration (timeSpan: TimeSpan) : string =
    if timeSpan.TotalSeconds < 1.0 then
        sprintf "%.2f ms" timeSpan.TotalMilliseconds
    elif timeSpan.TotalMinutes < 1.0 then
        sprintf "%.2f seconds" timeSpan.TotalSeconds
    else
        sprintf "%.2f minutes" timeSpan.TotalMinutes

/// Display a divider with optional text
let displayDivider (text: string option) =
    match text with
    | Some t -> 
        let padding = (UIConfig.lineWidth - t.Length - 2) / 2
        printfn "%s %s %s" (String('═', padding)) t (String('═', padding))
    | None -> 
        printSeparator()

/// Create a bordered message box
let displayMessageBox (title: string) (message: string) =
    printfn ""
    printfn "╔════════════════════════════════════════════════════════╗"
    printfn "║ %-54s ║" title
    printfn "╠════════════════════════════════════════════════════════╣"
    
    // Split message into lines that fit the box
    let maxLineLength = 54
    let words = message.Split(' ')
    let mutable currentLine = ""
    
    for word in words do
        if currentLine.Length + word.Length + 1 <= maxLineLength then
            currentLine <- if currentLine = "" then word else currentLine + " " + word
        else
            printfn "║ %-54s ║" currentLine
            currentLine <- word
    
    if currentLine <> "" then
        printfn "║ %-54s ║" currentLine
    
    printfn "╚════════════════════════════════════════════════════════╝"
    printfn ""

/// Display an ASCII art banner
let displayBanner () =
    printfn "  _____ _                 _        _____ _                  "
    printfn " / ____(_)               | |      / ____| |                 "
    printfn "| (___  _ _ __ ___  _ __ | | ___ | (___ | |_ ___  _ __ ___ "
    printfn " \\___ \\| | '_ ` _ \\| '_ \\| |/ _ \\ \\___ \\| __/ _ \\| '__/ _ \\"
    printfn " ____) | | | | | | | |_) | |  __/ ____) | || (_) | | |  __/"
    printfn "|_____/|_|_| |_| |_| .__/|_|\\___||_____/ \\__\\___/|_|  \\___|"
    printfn "                   | |                                      "
    printfn "                   |_|                                      "
