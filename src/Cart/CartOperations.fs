module CartOperations

open CartTypes
open Product
open PriceCalculator
open FileIO.FileOperations
open ProductDatabase

let empty: Cart = { Items = Map.empty }

let private getCurrentQuantity (productId: int) (cart: Cart) =
    match Map.tryFind productId cart.Items with
    | Some entry -> entry.Quantity
    | None -> 0

let addItem (catalog: ProductCatalog) (productId: int) (quantity: int) (cart: Cart) : Result<Cart, string> =
    if quantity < 1 then
        Error "Quantity must be at least 1"
    else
        match getProduct catalog productId with
        | None -> Error(sprintf "Product with ID %d not found" productId)
        | Some product ->
            let totalRequiredQty = (getCurrentQuantity productId cart) + quantity
            if not (isInStock product totalRequiredQty) then
                Error(
                    sprintf
                        "Insufficient stock for %s. Needed: %d, Available: %d"
                        product.Name
                        totalRequiredQty
                        product.Stock
                )
            else
                let newEntry =
                    { Product = product; Quantity = totalRequiredQty }
                let newItems = Map.add productId newEntry cart.Items
                Ok { cart with Items = newItems }

let removeItem (productId: int) (quantity: int) (cart: Cart) : Result<Cart, string> =
    if quantity < 1 then
        Error "Quantity must be at least 1"
    else
        match Map.tryFind productId cart.Items with
        | None -> Error(sprintf "Product ID %d is not in cart" productId)
        | Some entry ->
            let newQuantity = entry.Quantity - quantity
            if newQuantity <= 0 then
                Ok
                    { cart with
                        Items = Map.remove productId cart.Items }
            else
                let updatedEntry = { entry with Quantity = newQuantity }
                Ok
                    { cart with
                        Items = Map.add productId updatedEntry cart.Items }

let removeItemCompletely (productId: int) (cart: Cart) : Cart =
    { cart with
        Items = Map.remove productId cart.Items }

let getItems (cart: Cart) : CartEntry list =
    cart.Items |> Map.toList |> List.map snd

let isEmpty (cart: Cart) : bool = Map.isEmpty cart.Items

let clear (cart: Cart) : Cart = empty

//! this is with assuming no concurrency. will not double check stocks again.
let checkout (config: CheckoutConfig) (catalog: ProductCatalog) (cart: Cart) (discount: decimal)
    : Result<Order * ProductCatalog, string> =
    if isEmpty cart then
        Error "Cannot checkout an empty cart"
    else
        let updatedCatalog =
            cart.Items
            |> Map.fold (fun catalog productId entry ->
                let newStock = 
                    match getProduct catalog productId with
                    | Some product -> product.Stock - entry.Quantity
                    | None -> 0 //! again this is never happening on a single user system
                
                // Update database with new stock
                updateProductStock productId newStock
                
                // Update in-memory catalog
                updateStock catalog productId newStock
            ) catalog

        // Use PriceCalculator module for order creation with discount
        let order = createOrderSummary cart config discount
        
        // Save order to JSON and TXT files
        match saveOrder order with
        | Ok jsonPath -> 
            printfn "✅ Order saved to: %s" jsonPath
        | Error msg -> 
            printfn "⚠️ Failed to save order JSON: %s" msg
        
        match saveOrderSummary order with
        | Ok txtPath -> 
            printfn "✅ Order summary saved to: %s" txtPath
        | Error msg -> 
            printfn "⚠️ Failed to save order summary: %s" msg
        
        Ok (order, updatedCatalog)
