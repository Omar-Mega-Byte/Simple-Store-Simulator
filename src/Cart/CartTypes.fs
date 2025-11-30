module CartTypes

type CartEntry = {
    Product: Product.Product
    Quantity: int
}

type Cart = {
    Items: Map<int, CartEntry>  // Key is Product.Id
}

type Order = {
    OrderId: string
    Items: CartEntry list
    Subtotal: decimal
    Tax: decimal
    Shipping: decimal
    Total: decimal
    Timestamp: System.DateTime
}
