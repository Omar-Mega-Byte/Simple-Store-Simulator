namespace FileIO

module FileOperations =

    open System
    open System.IO
    open JsonSerializer

    // Constants
    let private DataFolder = "data"
    let private OrdersFolder = Path.Combine(DataFolder, "orders")

    // Ensure directories exist
    let private ensureDirectoriesExist () =
        if not (Directory.Exists DataFolder) then
            Directory.CreateDirectory DataFolder |> ignore
        if not (Directory.Exists OrdersFolder) then
            Directory.CreateDirectory OrdersFolder |> ignore

    // Read file content
    let private readFileContent (filePath: string) : Result<string, string> =
        try
            if File.Exists filePath then
                let content = File.ReadAllText(filePath)
                Ok content
            else
                Error (sprintf "File not found: %s" filePath)
        with
        | ex -> Error (sprintf "Error reading file '%s': %s" filePath ex.Message)

    // Write file content
    let private writeFileContent (filePath: string) (content: string) : Result<unit, string> =
        try
            ensureDirectoriesExist()
            File.WriteAllText(filePath, content)
            Ok ()
        with
        | ex -> Error (sprintf "Error writing file '%s': %s" filePath ex.Message)

    // ========== Order Operations ==========

    // Generate unique order filename
    let private generateOrderFileName (orderId: string) : string =
        let timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        let fileName = sprintf "order_%s_%s.json" orderId timestamp
        Path.Combine(OrdersFolder, fileName)

    // Save Order to JSON file
    let saveOrder (order: CartTypes.Order) : Result<string, string> =
        let json = serializeOrder order
        if String.IsNullOrEmpty(json) then
            Error "Failed to serialize order"
        else
            let filePath = generateOrderFileName order.OrderId
            match writeFileContent filePath json with
            | Ok () -> Ok filePath
            | Error msg -> Error msg

    // Save Order Summary (human-readable format)
    let saveOrderSummary (order: CartTypes.Order) : Result<string, string> =
        try
            ensureDirectoriesExist()
            let timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            let fileName = sprintf "order_summary_%s_%s.txt" order.OrderId timestamp
            let filePath = Path.Combine(OrdersFolder, fileName)
            
            let summary = 
                [
                    "═══════════════════════════════════════════════════════"
                    "                    ORDER SUMMARY"
                    "═══════════════════════════════════════════════════════"
                    sprintf "Order ID: %s" order.OrderId
                    sprintf "Date: %s" (order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"))
                    ""
                    "───────────────────────────────────────────────────────"
                    "                       ITEMS"
                    "───────────────────────────────────────────────────────"
                    ""
                ] @ (
                    order.Items 
                    |> List.map (fun item ->
                        sprintf "%-40s x%d\n  Price: $%.2M each | Subtotal: $%.2M"
                            item.Product.Name
                            item.Quantity
                            item.Product.Price
                            (item.Product.Price * decimal item.Quantity)
                    )
                ) @ [
                    ""
                    "───────────────────────────────────────────────────────"
                    sprintf "Subtotal:        $%.2M" order.Subtotal
                ] @ (
                    if order.Discount > 0m then
                        [sprintf "Discount:        -$%.2M" order.Discount]
                    else
                        []
                ) @ [
                    sprintf "Tax:             $%.2M" order.Tax
                    sprintf "Shipping:        $%.2M" order.Shipping
                    "───────────────────────────────────────────────────────"
                    sprintf "TOTAL:           $%.2M" order.Total
                    "═══════════════════════════════════════════════════════"
                    ""
                    "Thank you for your purchase!"
                    ""
                ]
            
            File.WriteAllLines(filePath, summary)
            Ok filePath
        with
        | ex -> Error (sprintf "Error saving order summary: %s" ex.Message)

    // ========== Export Operations ==========

    // Export Product Catalog to CSV
    let exportProductCatalogToCsv (catalog: Product.ProductCatalog) : Result<string, string> =
        try
            ensureDirectoriesExist()
            let timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            let fileName = sprintf "products_export_%s.csv" timestamp
            let filePath = Path.Combine(DataFolder, fileName)
            
            let csvHeader = "Id,Name,Price,Description,Category,Stock"
            let csvRows = 
                catalog
                |> Map.toList
                |> List.map (fun (_, p) ->
                    sprintf "%d,\"%s\",%.2M,\"%s\",\"%s\",%d" 
                        p.Id p.Name p.Price p.Description p.Category p.Stock
                )
            
            let csvContent = csvHeader :: csvRows
            File.WriteAllLines(filePath, csvContent)
            Ok filePath
        with
        | ex -> Error (sprintf "Error exporting catalog to CSV: %s" ex.Message)

    // Export Cart to text file (shopping list format)
    let exportCartToText (cart: CartTypes.Cart) : Result<string, string> =
        try
            ensureDirectoriesExist()
            let timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            let fileName = sprintf "cart_export_%s.txt" timestamp
            let filePath = Path.Combine(DataFolder, fileName)
            
            let cartItems = cart.Items |> Map.toList |> List.map snd
            let lines = 
                [
                    "═══════════════════════════════════════════════════════"
                    "                   SHOPPING CART"
                    "═══════════════════════════════════════════════════════"
                    sprintf "Exported: %s" (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    ""
                ] @ (
                    if List.isEmpty cartItems then
                        ["Your cart is empty."]
                    else
                        cartItems
                        |> List.map (fun item ->
                            sprintf "• %s (x%d) - $%.2M each"
                                item.Product.Name
                                item.Quantity
                                item.Product.Price
                        )
                ) @ [
                    "═══════════════════════════════════════════════════════"
                ]
            
            File.WriteAllLines(filePath, lines)
            Ok filePath
        with
        | ex -> Error (sprintf "Error exporting cart to text: %s" ex.Message)

    // Save updated product catalog to data folder (used after checkout to persist stock changes)
    let saveProductCatalog (catalog: Product.ProductCatalog) : Result<unit, string> =
        let json = serializeProductCatalog catalog
        if String.IsNullOrEmpty(json) then
            Error "Failed to serialize product catalog"
        else
            let filePath = Path.Combine(DataFolder, "products.json")
            writeFileContent filePath json
