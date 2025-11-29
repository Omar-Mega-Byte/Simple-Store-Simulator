module Product

// Product Module - عمر أحمد محمود عواد (Catalog Developer)

// Product Type Definition
type Product = { 
    Id: int
    Name: string
    Price: decimal
    Description: string
    Category: string
    Stock: int 
}

// Product Catalog Type
type ProductCatalog = Map<int, Product>

// Initialize Sample Product Catalog
let initializeCatalog () : ProductCatalog =
    Map.ofList [
        1, { Id = 1; Name = "Chocolate"; Price = 15.00m; Description = "Delicious milk chocolate bar"; Category = "Sweets"; Stock = 10 }
        2, { Id = 2; Name = "Biscuits"; Price = 10.00m; Description = "Crunchy butter biscuits"; Category = "Snacks"; Stock = 20 }
        3, { Id = 3; Name = "Ice Cream"; Price = 12.50m; Description = "Vanilla ice cream tub"; Category = "Frozen"; Stock = 5 }
        4, { Id = 4; Name = "Cookies"; Price = 8.00m; Description = "Chocolate chip cookies"; Category = "Snacks"; Stock = 15 }
        5, { Id = 5; Name = "Candy"; Price = 5.00m; Description = "Assorted fruit candies"; Category = "Sweets"; Stock = 30 }
    ]

// Catalog Operations

/// Get a product by ID
let getProduct (catalog: ProductCatalog) (productId: int) : Product option =
    catalog |> Map.tryFind productId

/// Get all products as a list
let getAllProducts (catalog: ProductCatalog) : Product list =
    catalog 
    |> Map.toList 
    |> List.map snd

/// Get products by category
let getProductsByCategory (catalog: ProductCatalog) (category: string) : Product list =
    catalog
    |> Map.toList
    |> List.map snd
    |> List.filter (fun p -> p.Category.ToLower() = category.ToLower())

/// Check if product is in stock with required quantity
let isInStock (product: Product) (quantity: int) : bool =
    product.Stock >= quantity

/// Update product stock
let updateStock (catalog: ProductCatalog) (productId: int) (newStock: int) : ProductCatalog =
    match getProduct catalog productId with
    | Some product -> 
        let updatedProduct = { product with Stock = newStock }
        catalog |> Map.add productId updatedProduct
    | None -> catalog

// Display Functions

/// Format price as currency
let formatPrice (price: decimal) : string =
    sprintf "EGP %.2f" price

/// Display a single product with details
let displayProduct (product: Product) =
    printfn "  [%d] %s - %s" product.Id product.Name (formatPrice product.Price)
    printfn "      %s" product.Description
    printfn "      Category: %s | Stock: %d units" product.Category product.Stock
    printfn ""

/// Display the entire catalog
let displayCatalog (title: string) (catalog: ProductCatalog) =
    printfn "=========================================="
    printfn "%s" title
    printfn "=========================================="
    if Map.isEmpty catalog then
        printfn "No products available."
    else
        catalog 
        |> Map.toList 
        |> List.map snd
        |> List.iter displayProduct
    printfn "Total products: %d" (Map.count catalog)
    printfn ""

/// Display products by category
let displayByCategory (catalog: ProductCatalog) (category: string) =
    let products = getProductsByCategory catalog category
    printfn "\n--- %s Category ---" category
    if List.isEmpty products then
        printfn "No products in this category."
    else
        products |> List.iter displayProduct