module Product

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

        // Added 30 more
        6,  { Id = 6;  Name = "Chips"; Price = 7.00m; Description = "Salted potato chips"; Category = "Snacks"; Stock = 25 }
        7,  { Id = 7;  Name = "Juice"; Price = 12.00m; Description = "Fresh orange juice bottle"; Category = "Drinks"; Stock = 18 }
        8,  { Id = 8;  Name = "Soda"; Price = 9.00m; Description = "Carbonated soft drink"; Category = "Drinks"; Stock = 22 }
        9,  { Id = 9;  Name = "Water"; Price = 5.00m; Description = "Mineral water bottle"; Category = "Drinks"; Stock = 40 }
        10, { Id = 10; Name = "Tea"; Price = 20.00m; Description = "Black tea bags box"; Category = "Beverages"; Stock = 12 }

        11, { Id = 11; Name = "Coffee"; Price = 35.00m; Description = "Instant coffee jar"; Category = "Beverages"; Stock = 8 }
        12, { Id = 12; Name = "Sugar"; Price = 18.00m; Description = "White granulated sugar pack"; Category = "Groceries"; Stock = 50 }
        13, { Id = 13; Name = "Rice"; Price = 30.00m; Description = "Long-grain rice bag"; Category = "Groceries"; Stock = 25 }
        14, { Id = 14; Name = "Pasta"; Price = 14.00m; Description = "Italian spaghetti pack"; Category = "Groceries"; Stock = 20 }
        15, { Id = 15; Name = "Cheese"; Price = 25.00m; Description = "Cheddar cheese block"; Category = "Dairy"; Stock = 10 }

        16, { Id = 16; Name = "Milk"; Price = 13.00m; Description = "Fresh whole milk pack"; Category = "Dairy"; Stock = 15 }
        17, { Id = 17; Name = "Yogurt"; Price = 6.00m; Description = "Strawberry flavored yogurt cup"; Category = "Dairy"; Stock = 30 }
        18, { Id = 18; Name = "Butter"; Price = 22.00m; Description = "Creamy salted butter pack"; Category = "Dairy"; Stock = 12 }
        19, { Id = 19; Name = "Eggs"; Price = 28.00m; Description = "Pack of 12 fresh eggs"; Category = "Groceries"; Stock = 16 }
        20, { Id = 20; Name = "Chicken"; Price = 75.00m; Description = "Frozen chicken breast"; Category = "Frozen"; Stock = 8 }

        21, { Id = 21; Name = "Beef"; Price = 95.00m; Description = "Lean ground beef"; Category = "Frozen"; Stock = 6 }
        22, { Id = 22; Name = "Fish"; Price = 60.00m; Description = "Frozen tilapia fillets"; Category = "Frozen"; Stock = 10 }
        23, { Id = 23; Name = "Tomatoes"; Price = 9.00m; Description = "Fresh ripe tomatoes"; Category = "Vegetables"; Stock = 35 }
        24, { Id = 24; Name = "Potatoes"; Price = 8.00m; Description = "Fresh potatoes"; Category = "Vegetables"; Stock = 28 }
        25, { Id = 25; Name = "Onions"; Price = 7.00m; Description = "Dry onions bag"; Category = "Vegetables"; Stock = 30 }

        26, { Id = 26; Name = "Apples"; Price = 15.00m; Description = "Red apples pack"; Category = "Fruits"; Stock = 25 }
        27, { Id = 27; Name = "Bananas"; Price = 12.00m; Description = "Fresh bananas"; Category = "Fruits"; Stock = 20 }
        28, { Id = 28; Name = "Grapes"; Price = 18.00m; Description = "Sweet seedless grapes"; Category = "Fruits"; Stock = 12 }
        29, { Id = 29; Name = "Shampoo"; Price = 30.00m; Description = "Anti-dandruff shampoo"; Category = "Personal Care"; Stock = 10 }
        30, { Id = 30; Name = "Soap"; Price = 6.00m; Description = "Scented bathing soap"; Category = "Personal Care"; Stock = 40 }

        31, { Id = 31; Name = "Toothpaste"; Price = 18.00m; Description = "Mint-flavored toothpaste"; Category = "Personal Care"; Stock = 25 }
        32, { Id = 32; Name = "Tissues"; Price = 10.00m; Description = "Soft tissue paper box"; Category = "Home"; Stock = 35 }
        33, { Id = 33; Name = "Detergent"; Price = 45.00m; Description = "Laundry detergent powder"; Category = "Home"; Stock = 14 }
        34, { Id = 34; Name = "Bread"; Price = 7.00m; Description = "Fresh bakery bread"; Category = "Bakery"; Stock = 20 }
        35, { Id = 35; Name = "Croissant"; Price = 10.00m; Description = "Butter croissant"; Category = "Bakery"; Stock = 18 }
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

// Example Usage
let ProductCatalog = initializeCatalog()

displayCatalog "Store Inventory" ProductCatalog

// Display by categories
displayByCategory ProductCatalog "Sweets"
displayByCategory ProductCatalog "Snacks"

// Test individual operations
printfn "--- Testing Catalog Operations ---\n"

match getProduct ProductCatalog 1 with
| Some p -> printfn "Found product: %s at %s" p.Name (formatPrice p.Price)
| None -> printfn "Product not found"

printfn "\nChecking stock for Chocolate (need 5): %b" (
    match getProduct ProductCatalog 1 with
    | Some p -> isInStock p 5
    | None -> false
)

let updatedCatalog = updateStock ProductCatalog 1 25
printfn "\nUpdated Chocolate stock to 25:"
match getProduct updatedCatalog 1 with
| Some p -> printfn "New stock level: %d units" p.Stock
| None -> printfn "Product not found"