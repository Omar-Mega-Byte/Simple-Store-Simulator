namespace SimpleStoreSimulator.GUI.ViewModels

open System
open System.Collections.ObjectModel
open CommunityToolkit.Mvvm.Input
open Product
open ProductDatabase
open SearchOperations
open SearchTypes
open CartTypes
open CartOperations
open CartConfig
open PriceCalculator
open DiscountEngine

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

// ViewModel for Cart Item - now using PriceCalculator
type CartItemViewModel(entry: CartEntry) =
    inherit ViewModelBase()
    
    member this.ProductId = entry.Product.Id
    member this.Product = entry.Product
    member this.Name = entry.Product.Name
    member this.Price = entry.Product.Price
    member this.Quantity = entry.Quantity
    // Use PriceCalculator module for subtotal calculation
    member this.SubtotalDisplay = formatPrice (calculateItemSubtotal entry)
    member this.QuantityDisplay = sprintf "Qty: %d" entry.Quantity

// Main Window ViewModel
type MainWindowViewModel() as this =
    inherit ViewModelBase()
    
    // Get configuration from Cart module (no business rules in GUI)
    let config = CartConfig.getConfig()
    
    let mutable catalog = initializeCatalogWithDatabase()
    let mutable cart = CartOperations.empty
    let mutable products = ObservableCollection<ProductViewModel>()
    let mutable cartItems = ObservableCollection<CartItemViewModel>()
    let mutable searchText = ""
    let mutable selectedCategory = "All Categories"
    let mutable selectedSortBy = "Name"
    let mutable inStockOnly = false
    let mutable minPrice = 0m
    let mutable maxPrice = 1000m
    let mutable minStock = 0
    let mutable maxStock = 100
    let mutable sortAscending = true
    let mutable cartSubtotal = 0m
    let mutable cartDiscount = 0m
    let mutable cartTax = 0m
    let mutable cartShipping = 0m
    let mutable cartTotal = 0m
    let mutable cartItemCount = 0
    let mutable statusMessage = "Ready"
    let mutable activeDiscount: DiscountRule option = None
    let mutable availableDiscounts = ObservableCollection<string>()
    let mutable selectedDiscountName = "No discount"
    
    // Initialize available discounts
    let allDiscounts = [
        discount10Percent
        discount20Percent
        sweetsCategoryDiscount
        snacksCategoryDiscount
        buy2Get1Free
        buy3Get2Free
    ]
    
    do
        this.LoadAllProducts()
        this.LoadCategories()
        this.InitializeDiscounts()
    
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
    
    member this.SortByOptions = 
        ObservableCollection<string>(["Name"; "Price"; "Stock"; "Category"])
    
    member this.SelectedSortBy
        with get() = selectedSortBy
        and set(value) = 
            selectedSortBy <- value
            this.FilterProducts()
            this.OnPropertyChanged("SelectedSortBy")
    
    member this.InStockOnly
        with get() = inStockOnly
        and set(value) = 
            inStockOnly <- value
            this.FilterProducts()
            this.OnPropertyChanged("InStockOnly")
    
    member this.MinPrice
        with get() = minPrice
        and set(value) = 
            minPrice <- value
            this.OnPropertyChanged("MinPrice")
    
    member this.MaxPrice
        with get() = maxPrice
        and set(value) = 
            maxPrice <- value
            this.OnPropertyChanged("MaxPrice")
    
    member this.MinStock
        with get() = minStock
        and set(value) = 
            minStock <- value
            this.OnPropertyChanged("MinStock")
    
    member this.MaxStock
        with get() = maxStock
        and set(value) = 
            maxStock <- value
            this.OnPropertyChanged("MaxStock")
    
    member this.SortAscending
        with get() = sortAscending
        and set(value) = 
            sortAscending <- value
            this.FilterProducts()
            this.OnPropertyChanged("SortAscending")
    
    member this.SortOrderDisplay = if sortAscending then "↑ Ascending" else "↓ Descending"
    
    member this.CartSubtotal
        with get() = cartSubtotal
        and set(value) =
            cartSubtotal <- value
            this.OnPropertyChanged("CartSubtotal")
            this.OnPropertyChanged("CartSubtotalDisplay")
    
    member this.CartSubtotalDisplay = sprintf "Subtotal: EGP %.2f" cartSubtotal
    
    member this.CartDiscount
        with get() = cartDiscount
        and set(value) =
            cartDiscount <- value
            this.OnPropertyChanged("CartDiscount")
            this.OnPropertyChanged("CartDiscountDisplay")
            this.OnPropertyChanged("HasDiscount")
    
    member this.CartDiscountDisplay = 
        if cartDiscount > 0m then
            sprintf "Discount: -EGP %.2f" cartDiscount
        else
            "Discount: EGP 0.00"
    
    member this.HasDiscount = cartDiscount > 0m
    
    member this.AvailableDiscounts = availableDiscounts
    
    member this.SelectedDiscountName
        with get() = selectedDiscountName
        and set(value) = 
            selectedDiscountName <- value
            this.OnPropertyChanged("SelectedDiscountName")
    
    member this.ActiveDiscountName = 
        match activeDiscount with
        | Some rule -> rule.Name
        | None -> "No discount"
    
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
    
    member this.ToggleSortOrderCommand =
        RelayCommand(fun () -> 
            sortAscending <- not sortAscending
            this.FilterProducts())
    
    member this.ApplyFiltersCommand =
        RelayCommand(fun () -> this.FilterProducts())
    
    member this.ResetFiltersCommand =
        RelayCommand(fun () -> 
            this.SearchText <- ""
            this.SelectedCategory <- "All Categories"
            this.SelectedSortBy <- "Name"
            this.InStockOnly <- false
            this.MinPrice <- 0m
            this.MaxPrice <- 1000m
            this.MinStock <- 0
            this.MaxStock <- 100
            this.SortAscending <- true
            this.LoadAllProducts()
            this.StatusMessage <- "✅ Filters reset")
    
    member this.ApplyDiscountCommand =
        RelayCommand(fun () -> this.ApplyDiscount())
    
    member this.RemoveDiscountCommand =
        RelayCommand(fun () -> this.RemoveDiscount())
    
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
        
        // Create search criteria using SearchTypes
        let criteria: SearchCriteria = {
            SearchTerm = if String.IsNullOrWhiteSpace(searchText) then None else Some searchText
            Category = if selectedCategory = "All Categories" then None else Some selectedCategory
            PriceRange = if minPrice > 0m || maxPrice < 1000m then Some { MinPrice = minPrice; MaxPrice = maxPrice } else None
            StockRange = if minStock > 0 || maxStock < 100 then Some { MinStock = minStock; MaxStock = maxStock } else None
            InStockOnly = inStockOnly
        }
        
        // Apply search criteria
        let filtered = applySearchCriteria allProducts criteria
        
        // Apply sorting based on selected option
        let sorted = 
            match selectedSortBy with
            | "Price" -> sortByPrice filtered sortAscending
            | "Stock" -> sortByStock filtered sortAscending
            | "Category" -> sortByCategory filtered sortAscending
            | _ -> sortByName filtered sortAscending
        
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
        // Use PriceCalculator module for all calculations
        let subtotal = calculateCartSubtotal cart
        this.CartSubtotal <- subtotal
        
        // Apply discount if active
        let discount = 
            match activeDiscount with
            | Some rule -> applyDiscountToCart cart rule
            | None -> 0m
        this.CartDiscount <- discount
        
        // Calculate tax and shipping on discounted subtotal
        let discountedSubtotal = subtotal - discount
        this.CartTax <- calculateTax discountedSubtotal config.TaxRate
        this.CartShipping <- calculateCartShipping cart config.ShippingRates
        this.CartTotal <- calculateTotal discountedSubtotal this.CartTax this.CartShipping
        this.CartItemCount <- calculateTotalQuantity cart
        
        // Update available discounts list
        this.UpdateAvailableDiscounts()
    
    member private this.Checkout() =
        if not (CartOperations.isEmpty cart) then
            match CartOperations.checkout config catalog cart this.CartDiscount with
            | Ok (order, updatedCatalog) ->
                catalog <- updatedCatalog
                
                // Use PriceCalculator's formatPrice for consistent formatting
                let discountMsg = 
                    if order.Discount > 0m then
                        sprintf "\n💸 Discount: -%s" (formatPrice order.Discount)
                    else
                        ""
                
                let message = sprintf "✅ Order %s completed!\n\n📦 Items: %d\n💰 Subtotal: %s%s\n📊 Tax: %s\n🚚 Shipping: %s\n💳 Total: %s\n\n🕒 %s" 
                                      (order.OrderId.Substring(0, 8))
                                      (order.Items |> List.sumBy (fun e -> e.Quantity))
                                      (formatPrice order.Subtotal)
                                      discountMsg
                                      (formatPrice order.Tax)
                                      (formatPrice order.Shipping)
                                      (formatPrice order.Total)
                                      (order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"))
                
                printfn "%s" message
                this.StatusMessage <- sprintf "✅ Order completed! Total: %s" (formatPrice order.Total)
                
                this.ClearCart()
                this.LoadAllProducts() // Refresh to show updated stock
            | Error msg ->
                this.StatusMessage <- sprintf "❌ Checkout failed: %s" msg
                printfn "❌ Checkout failed: %s" msg
        else
            this.StatusMessage <- "⚠️ Cart is empty"
    
    member private this.ClearCart() =
        cart <- CartOperations.clear cart
        activeDiscount <- None
        this.RefreshCartDisplay()
    
    member private this.InitializeDiscounts() =
        availableDiscounts.Add("No discount")
        for discount in allDiscounts do
            availableDiscounts.Add(discount.Name)
        selectedDiscountName <- "No discount"
    
    member private this.ApplyDiscount() =
        if selectedDiscountName = "No discount" then
            this.RemoveDiscount()
        else
            let discount = allDiscounts |> List.tryFind (fun d -> d.Name = selectedDiscountName)
            match discount with
            | Some rule ->
                activeDiscount <- Some rule
                this.UpdateCartTotals()
                this.OnPropertyChanged("ActiveDiscountName")
                this.StatusMessage <- sprintf "✅ Applied: %s" rule.Name
            | None -> 
                this.StatusMessage <- sprintf "⚠️ Discount not found: %s" selectedDiscountName
    member private this.RemoveDiscount() =
        activeDiscount <- None
        this.CartDiscount <- 0m
        this.UpdateCartTotals()
        this.OnPropertyChanged("ActiveDiscountName")
        this.StatusMessage <- "🗑️ Discount removed"
    
    member private this.UpdateAvailableDiscounts() =
        ()  // Discounts are pre-loaded in InitializeDiscounts
