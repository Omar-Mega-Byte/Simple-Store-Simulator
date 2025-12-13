module ProductDatabase

open Microsoft.Data.Sqlite
open System.IO
open Product

// Database file path
let private dbPath = Path.Combine("data", "products.db")

// Ensure data directory exists
let private ensureDataDirectoryExists () =
    let dataDir = "data"
    if not (Directory.Exists dataDir) then
        Directory.CreateDirectory dataDir |> ignore

// Initialize database and create table if not exists (public for tests)
let initializeDatabase () =
    ensureDataDirectoryExists()
    use connection = new SqliteConnection(sprintf "Data Source=%s" dbPath)
    connection.Open()
    
    let createTableQuery = """
        CREATE TABLE IF NOT EXISTS Products (
            Id INTEGER PRIMARY KEY,
            Name TEXT NOT NULL,
            Price REAL NOT NULL,
            Description TEXT NOT NULL,
            Category TEXT NOT NULL,
            Stock INTEGER NOT NULL
        );
    """
    
    use command = new SqliteCommand(createTableQuery, connection)
    command.ExecuteNonQuery() |> ignore

// Load products from database
let loadProductsFromDatabase () : ProductCatalog =
    ensureDataDirectoryExists()
    initializeDatabase()
    
    use connection = new SqliteConnection(sprintf "Data Source=%s" dbPath)
    connection.Open()
    
    let selectQuery = "SELECT Id, Name, Price, Description, Category, Stock FROM Products"
    use command = new SqliteCommand(selectQuery, connection)
    use reader = command.ExecuteReader()
    
    let mutable products = Map.empty
    
    while reader.Read() do
        let product = {
            Id = reader.GetInt32(0)
            Name = reader.GetString(1)
            Price = decimal (reader.GetDouble(2))
            Description = reader.GetString(3)
            Category = reader.GetString(4)
            Stock = reader.GetInt32(5)
        }
        products <- Map.add product.Id product products
    
    products

// Save product catalog to database
let saveProductsToDatabase (catalog: ProductCatalog) : unit =
    ensureDataDirectoryExists()
    initializeDatabase()
    
    use connection = new SqliteConnection(sprintf "Data Source=%s" dbPath)
    connection.Open()
    
    // Clear existing data
    use deleteCommand = new SqliteCommand("DELETE FROM Products", connection)
    deleteCommand.ExecuteNonQuery() |> ignore
    
    // Insert all products
    let insertQuery = """
        INSERT INTO Products (Id, Name, Price, Description, Category, Stock)
        VALUES (@Id, @Name, @Price, @Description, @Category, @Stock)
    """
    
    for kvp in catalog do
        let product = kvp.Value
        use command = new SqliteCommand(insertQuery, connection)
        command.Parameters.AddWithValue("@Id", product.Id) |> ignore
        command.Parameters.AddWithValue("@Name", product.Name) |> ignore
        command.Parameters.AddWithValue("@Price", float product.Price) |> ignore
        command.Parameters.AddWithValue("@Description", product.Description) |> ignore
        command.Parameters.AddWithValue("@Category", product.Category) |> ignore
        command.Parameters.AddWithValue("@Stock", product.Stock) |> ignore
        command.ExecuteNonQuery() |> ignore

// Update single product stock in database
let updateProductStock (productId: int) (newStock: int) : unit =
    ensureDataDirectoryExists()
    initializeDatabase()
    
    use connection = new SqliteConnection(sprintf "Data Source=%s" dbPath)
    connection.Open()
    
    let updateQuery = "UPDATE Products SET Stock = @Stock WHERE Id = @Id"
    use command = new SqliteCommand(updateQuery, connection)
    command.Parameters.AddWithValue("@Stock", newStock) |> ignore
    command.Parameters.AddWithValue("@Id", productId) |> ignore
    command.ExecuteNonQuery() |> ignore

// Update multiple products' stock (batch update with transaction)
let updateMultipleProductsStock (updates: (int * int) list) : unit =
    ensureDataDirectoryExists()
    initializeDatabase()
    
    use connection = new SqliteConnection(sprintf "Data Source=%s" dbPath)
    connection.Open()
    
    use transaction = connection.BeginTransaction()
    try
        let updateQuery = "UPDATE Products SET Stock = @Stock WHERE Id = @Id"
        
        for (productId, newStock) in updates do
            use command = new SqliteCommand(updateQuery, connection, transaction)
            command.Parameters.AddWithValue("@Stock", newStock) |> ignore
            command.Parameters.AddWithValue("@Id", productId) |> ignore
            command.ExecuteNonQuery() |> ignore
        
        transaction.Commit()
    with
    | ex ->
        transaction.Rollback()
        printfn "Error updating stocks: %s" ex.Message
        reraise()


// Initialize catalog: load from database or create default and save
let initializeCatalogWithDatabase () : ProductCatalog =
    ensureDataDirectoryExists()
    initializeDatabase()
    
    let catalog = loadProductsFromDatabase()
    
    if Map.isEmpty catalog then
        // Database is empty, initialize with default catalog
        printfn "Database empty. Initializing with default products..."
        let defaultCatalog = initializeCatalog()
        saveProductsToDatabase defaultCatalog
        printfn "✅ %d products saved to database." (Map.count defaultCatalog)
        defaultCatalog
    else
        printfn "✅ Loaded %d products from database." (Map.count catalog)
        catalog
