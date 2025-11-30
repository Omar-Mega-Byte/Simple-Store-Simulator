module CartOperations

open CartTypes
open Product

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

let getItemCount (cart: Cart) : int =
    cart.Items |> Map.values |> Seq.sumBy (fun entry -> entry.Quantity)

let getSubtotal (cart: Cart) : decimal =
    cart.Items
    |> Map.values
    |> Seq.sumBy (fun entry -> entry.Product.Price * decimal entry.Quantity)

let getTax (cart: Cart) (taxRate: decimal) : decimal = getSubtotal cart * taxRate

let getShippingFee (cart: Cart) (tierOne: decimal) (tierTwo: decimal) (tierThree: decimal) : decimal =
    let count = getItemCount cart

    match count with
    | 0 -> 0m
    | n when n <= 5 -> tierOne
    | n when n <= 10 -> tierTwo
    | _ -> tierThree

let getTotal (cart: Cart) (taxRate: decimal) (shippingFees: decimal * decimal * decimal) : decimal =
    let (s1, s2, s3) = shippingFees
    let subtotal = getSubtotal cart
    let tax = getTax cart taxRate
    let shipping = getShippingFee cart s1 s2 s3
    subtotal + tax + shipping

let isEmpty (cart: Cart) : bool = Map.isEmpty cart.Items

let clear (cart: Cart) : Cart = empty

//! this is with assuming no concurrency. will not double check stocks again.
let checkout (config: {|taxRate: decimal; shippingRates: decimal * decimal * decimal|}) (catalog: ProductCatalog) (cart: Cart) 
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
                updateStock catalog productId newStock
            ) catalog

        let (tier1, tier2, tier3) = config.shippingRates
        let subtotal = getSubtotal cart
        let tax = getTax cart config.taxRate 
        let shipping = getShippingFee cart tier1 tier2 tier3
        let total = subtotal + tax + shipping
    
        //TODO do something with this
        let order = {
            OrderId = System.Guid.NewGuid().ToString()
            Items = getItems cart
            Subtotal = subtotal
            Tax = tax
            Shipping = shipping
            Total = total
            Timestamp = System.DateTime.UtcNow
        }
        Ok (order, updatedCatalog)
