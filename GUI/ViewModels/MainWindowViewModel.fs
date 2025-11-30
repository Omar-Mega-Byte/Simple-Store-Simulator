namespace SimpleStoreSimulator.GUI.ViewModels

open System
open System.Collections.ObjectModel
open CommunityToolkit.Mvvm.Input
open Product
open SearchOperations

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
type CartItemViewModel(product: Product, quantity: int) =
    inherit ViewModelBase()
    
    let mutable qty = quantity
    
    member this.Quantity 
        with get() = qty
        and set(value) = 
            qty <- value
            this.OnPropertyChanged("Quantity")
            this.OnPropertyChanged("Subtotal")
            this.OnPropertyChanged("SubtotalDisplay")
    
    member this.Product = product
    member this.Name = product.Name
    member this.Price = product.Price
    member this.Subtotal = product.Price * decimal qty
    member this.SubtotalDisplay = sprintf "EGP %.2f" this.Subtotal

// Main Window ViewModel
type MainWindowViewModel() as this =
    inherit ViewModelBase()
    
    let catalog = initializeCatalog()
    let mutable products = ObservableCollection<ProductViewModel>()
    let mutable cartItems = ObservableCollection<CartItemViewModel>()
    let mutable searchText = ""
    let mutable selectedCategory = "All Categories"
    let mutable cartTotal = 0m
    let mutable cartItemCount = 0
    
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
    
    // Commands
    member this.SearchCommand = 
        RelayCommand(fun () -> this.SearchProducts())
    
    member this.AddToCartCommand = 
        RelayCommand<ProductViewModel>(fun product -> 
            if not (isNull (box product)) then this.AddToCart(product))
    
    member this.RemoveFromCartCommand =
        RelayCommand<CartItemViewModel>(fun item ->
            if not (isNull (box item)) then this.RemoveFromCart(item))
    
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
            // Check if product already in cart
            let existing = cartItems |> Seq.tryFind (fun item -> item.Product.Id = productVM.Id)
            match existing with
            | Some item ->
                // Check if we can add more (stock limit)
                if item.Quantity < productVM.Stock then
                    item.Quantity <- item.Quantity + 1
                    this.OnPropertyChanged("CartItems")
                else
                    printfn "⚠️ Cannot add more - only %d units in stock" productVM.Stock
            | None ->
                cartItems.Add(CartItemViewModel(productVM.Product, 1))
            
            this.UpdateCartTotals()
    
    member private this.RemoveFromCart(item: CartItemViewModel) =
        cartItems.Remove(item) |> ignore
        this.UpdateCartTotals()
    
    member private this.UpdateCartTotals() =
        let mutable total = 0m
        let mutable count = 0
        for item in cartItems do
            total <- total + item.Subtotal
            count <- count + item.Quantity
        this.CartTotal <- total
        this.CartItemCount <- count
    
    member private this.Checkout() =
        if cartItems.Count > 0 then
            // Generate order ID
            let orderId = System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            let message = sprintf "Order %s completed successfully!\nTotal: EGP %.2f\nItems: %d" 
                                  orderId this.CartTotal this.CartItemCount
            
            // For now, print to console. Can add Avalonia MessageBox later
            printfn "✅ CHECKOUT COMPLETE"
            printfn "%s" message
            
            this.ClearCart()
    
    member private this.ClearCart() =
        cartItems.Clear()
        this.UpdateCartTotals()
