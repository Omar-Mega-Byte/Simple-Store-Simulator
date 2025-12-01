namespace SimpleStoreSimulator.GUI.ViewModels

open System
open System.Collections.ObjectModel
open CommunityToolkit.Mvvm.Input
open Product
open SearchOperations
open CartTypes
open CartOperations
open CartConfig

// ViewModel for a Product in the UI
type ProductViewModel(product: Product) =
    inherit ViewModelBase()
    
    member this.Id = product.Id
    member this.Name = product.Name
    member this.Price = product.Price
    member this.PriceDisplay = sprintf "EGP %.2f" product.Price
    member this.Description = product.Description
    member this.Category = product.Category
    member this.Stock = product.Stock
    member this.StockDisplay = sprintf "%d units" product.Stock
    member this.IsInStock = product.Stock > 0
    member this.IsLowStock = product.Stock > 0 && product.Stock <= 5
    member this.Product = product

// ViewModel for Cart Item
type CartItemViewModel(entry: CartEntry) =
    inherit ViewModelBase()
    
    member this.ProductId = entry.Product.Id
    member this.Product = entry.Product
    member this.Name = entry.Product.Name
    member this.Price = entry.Product.Price
    member this.Quantity = entry.Quantity
    // Display subtotal directly without calculation logic - value comes from CartEntry
    member this.SubtotalDisplay = sprintf "EGP %.2f" (entry.Product.Price * decimal entry.Quantity)
    member this.QuantityDisplay = sprintf "Qty: %d" entry.Quantity

// Main Window ViewModel
type MainWindowViewModel() as this =
    inherit ViewModelBase()
    
    // Get configuration from Cart module (no business rules in GUI)
    let config = CartConfig.getConfig()
    
    let mutable catalog = initializeCatalog()
    let mutable cart = CartOperations.empty
    let mutable products = ObservableCollection<ProductViewModel>()
    let mutable cartItems = ObservableCollection<CartItemViewModel>()
    let mutable searchText = ""
    let mutable selectedCategory = "All Categories"
    let mutable cartSubtotal = 0m
    let mutable cartTax = 0m
    let mutable cartShipping = 0m
    let mutable cartTotal = 0m
    let mutable cartItemCount = 0
    let mutable statusMessage = "Ready"
    
    do
        this.LoadAllProducts()
        this.LoadCategories()
    
    // Properties
    member this.Products = products
    member this.CartItems = cartItems
    member this.Categories = 
        let cats = ResizeArray<string>()
        cats.Add("All Categories")
        let productList = getAllProducts catalog
        let categories = getCategories productList
        cats.AddRange(categories)
        ObservableCollection<string>(cats)
    
    member this.SearchText 
        with get() = searchText
        and set(value) = 
            searchText <- value
            this.OnPropertyChanged("SearchText")
    
    member this.SelectedCategory
        with get() = selectedCategory
        and set(value) = 
            selectedCategory <- value
            this.FilterProducts()
            this.OnPropertyChanged("SelectedCategory")
    
    member this.CartSubtotal
        with get() = cartSubtotal
        and set(value) =
            cartSubtotal <- value
            this.OnPropertyChanged("CartSubtotal")
            this.OnPropertyChanged("CartSubtotalDisplay")
    
    member this.CartSubtotalDisplay = sprintf "Subtotal: EGP %.2f" cartSubtotal
    
    member this.CartTax
        with get() = cartTax
        and set(value) =
            cartTax <- value
            this.OnPropertyChanged("CartTax")
            this.OnPropertyChanged("CartTaxDisplay")
    
    member this.CartTaxDisplay = sprintf "Tax (14%%): EGP %.2f" cartTax
    
    member this.CartShipping
        with get() = cartShipping
        and set(value) =
            cartShipping <- value
            this.OnPropertyChanged("CartShipping")
            this.OnPropertyChanged("CartShippingDisplay")
    
    member this.CartShippingDisplay = sprintf "Shipping: EGP %.2f" cartShipping
    
    member this.CartTotal
        with get() = cartTotal
        and set(value) =
            cartTotal <- value
            this.OnPropertyChanged("CartTotal")
            this.OnPropertyChanged("CartTotalDisplay")
    
    member this.CartTotalDisplay = sprintf "Total: EGP %.2f" cartTotal
    
    member this.CartItemCount
        with get() = cartItemCount
        and set(value) =
            cartItemCount <- value
            this.OnPropertyChanged("CartItemCount")
            this.OnPropertyChanged("CartItemCountDisplay")
    
    member this.CartItemCountDisplay = sprintf "Cart (%d)" cartItemCount
    
    member this.StatusMessage
        with get() = statusMessage
        and set(value) =
            statusMessage <- value
            this.OnPropertyChanged("StatusMessage")
    
    // Commands
    member this.SearchCommand = 
        RelayCommand(fun () -> this.SearchProducts())
    
    member this.AddToCartCommand = 
        RelayCommand<ProductViewModel>(fun product -> 
            if not (isNull (box product)) then this.AddToCart(product))
    
    member this.RemoveFromCartCommand =
        RelayCommand<CartItemViewModel>(fun item ->
            if not (isNull (box item)) then this.RemoveFromCart(item))
    
    member this.IncrementQuantityCommand =
        RelayCommand<CartItemViewModel>(fun item ->
            if not (isNull (box item)) then this.IncrementQuantity(item))
    
    member this.DecrementQuantityCommand =
        RelayCommand<CartItemViewModel>(fun item ->
            if not (isNull (box item)) then this.DecrementQuantity(item))
    
    member this.CheckoutCommand =
        RelayCommand(fun () -> this.Checkout())
    
    member this.ClearCartCommand =
        RelayCommand(fun () -> this.ClearCart())
    
    member this.ShowAllProductsCommand =
        RelayCommand(fun () -> this.LoadAllProducts())
    
    // Methods
    member private this.LoadAllProducts() =
        products.Clear()
        let allProducts = getAllProducts catalog
        let sorted = sortByName allProducts true
        for p in sorted do
            products.Add(ProductViewModel(p))
    
    member private this.LoadCategories() =
        ()  // Categories are loaded in the property
    
    member private this.SearchProducts() =
        products.Clear()
        let allProducts = getAllProducts catalog
        let filtered = 
            if String.IsNullOrWhiteSpace(searchText) then
                allProducts
            else
                searchByNameOrDescription allProducts searchText
        
        let sorted = sortByName filtered true
        for p in sorted do
            products.Add(ProductViewModel(p))
    
    member private this.FilterProducts() =
        products.Clear()
        let allProducts = getAllProducts catalog
        let filtered =
            if selectedCategory = "All Categories" then
                allProducts
            else
                filterByCategory allProducts selectedCategory
        
        let sorted = sortByName filtered true
        for p in sorted do
            products.Add(ProductViewModel(p))
    
    member private this.AddToCart(productVM: ProductViewModel) =
        if productVM.IsInStock then
            match CartOperations.addItem catalog productVM.Id 1 cart with
            | Ok newCart ->
                cart <- newCart
                this.RefreshCartDisplay()
                this.StatusMessage <- sprintf "✅ Added %s to cart" productVM.Name
            | Error msg ->
                this.StatusMessage <- sprintf "⚠️ %s" msg
                printfn "⚠️ %s" msg
    
    member private this.RemoveFromCart(item: CartItemViewModel) =
        cart <- CartOperations.removeItemCompletely item.ProductId cart
        this.RefreshCartDisplay()
        this.StatusMessage <- sprintf "🗑️ Removed %s from cart" item.Name
    
    member private this.IncrementQuantity(item: CartItemViewModel) =
        match CartOperations.addItem catalog item.ProductId 1 cart with
        | Ok newCart ->
            cart <- newCart
            this.RefreshCartDisplay()
            this.StatusMessage <- sprintf "➕ Increased %s quantity" item.Name
        | Error msg ->
            this.StatusMessage <- sprintf "⚠️ %s" msg
            printfn "⚠️ %s" msg
    
    member private this.DecrementQuantity(item: CartItemViewModel) =
        match CartOperations.removeItem item.ProductId 1 cart with
        | Ok newCart ->
            cart <- newCart
            this.RefreshCartDisplay()
            this.StatusMessage <- sprintf "➖ Decreased %s quantity" item.Name
        | Error msg ->
            this.StatusMessage <- sprintf "⚠️ %s" msg
            printfn "⚠️ %s" msg
    
    member private this.RefreshCartDisplay() =
        cartItems.Clear()
        let entries = CartOperations.getItems cart
        for entry in entries do
            cartItems.Add(CartItemViewModel(entry))
        this.UpdateCartTotals()
        
        // Refresh product list to show updated stock
        let currentProducts = products |> Seq.map (fun p -> p.Product) |> Seq.toList
        products.Clear()
        for p in currentProducts do
            // Get updated product from catalog
            match getProduct catalog p.Id with
            | Some updatedProduct -> products.Add(ProductViewModel(updatedProduct))
            | None -> products.Add(ProductViewModel(p))
    
    member private this.UpdateCartTotals() =
        let (tier1, tier2, tier3) = config.ShippingRates
        this.CartSubtotal <- CartOperations.getSubtotal cart
        this.CartTax <- CartOperations.getTax cart config.TaxRate
        this.CartShipping <- CartOperations.getShippingFee cart tier1 tier2 tier3
        this.CartTotal <- CartOperations.getTotal cart config.TaxRate config.ShippingRates
        this.CartItemCount <- CartOperations.getItemCount cart
    
    member private this.Checkout() =
        if not (CartOperations.isEmpty cart) then
            match CartOperations.checkout config catalog cart with
            | Ok (order, updatedCatalog) ->
                catalog <- updatedCatalog
                
                let message = sprintf "✅ Order %s completed!\n\n📦 Items: %d\n💰 Subtotal: EGP %.2f\n📊 Tax: EGP %.2f\n🚚 Shipping: EGP %.2f\n💳 Total: EGP %.2f\n\n🕒 %s" 
                                      (order.OrderId.Substring(0, 8))
                                      (order.Items |> List.sumBy (fun e -> e.Quantity))
                                      order.Subtotal
                                      order.Tax
                                      order.Shipping
                                      order.Total
                                      (order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"))
                
                printfn "%s" message
                this.StatusMessage <- sprintf "✅ Order completed! Total: EGP %.2f" order.Total
                
                this.ClearCart()
                this.LoadAllProducts() // Refresh to show updated stock
            | Error msg ->
                this.StatusMessage <- sprintf "❌ Checkout failed: %s" msg
                printfn "❌ Checkout failed: %s" msg
        else
            this.StatusMessage <- "⚠️ Cart is empty"
    
    member private this.ClearCart() =
        cart <- CartOperations.clear cart
        this.RefreshCartDisplay()
