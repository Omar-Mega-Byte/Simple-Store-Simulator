module JsonSerializer

open System
open System.Text.Json
open System.Text.Json.Serialization

// JSON Serialization Options
let private jsonOptions = 
    let options = JsonSerializerOptions()
    options.WriteIndented <- true
    options.PropertyNameCaseInsensitive <- true
    options.Converters.Add(JsonFSharpConverter())
    options

// Generic JSON Serialization
let serializeToJson<'T> (data: 'T) : string =
    try
        JsonSerializer.Serialize(data, jsonOptions)
    with
    | ex -> 
        printfn "Error serializing to JSON: %s" ex.Message
        ""

// Serialize Product to JSON
let serializeProduct (product: Product.Product) : string =
    serializeToJson product

// Serialize Product List to JSON
let serializeProducts (products: Product.Product list) : string =
    serializeToJson products

// Serialize Product Catalog (Map) to JSON
let serializeProductCatalog (catalog: Product.ProductCatalog) : string =
    let productList = catalog |> Map.toList |> List.map snd
    serializeToJson productList

// Serialize Cart Order to JSON
let serializeOrder (order: CartTypes.Order) : string =
    serializeToJson order

// Serialize Cart to JSON (for export)
let serializeCart (cart: CartTypes.Cart) : string =
    serializeToJson cart

