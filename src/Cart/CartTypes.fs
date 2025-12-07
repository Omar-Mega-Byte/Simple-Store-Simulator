module CartTypes

type CartEntry = {
    Product: Product.Product
    Quantity: int
}

type Cart = {
    Items: Map<int, CartEntry>  // Key is Product.Id
}

type CheckoutConfig = {
    TaxRate: decimal
    ShippingRates: decimal * decimal * decimal
}

type Order = {
    OrderId: string
    Items: CartEntry list
    Subtotal: decimal
    Discount: decimal
    Tax: decimal
    Shipping: decimal
    Total: decimal
    Timestamp: System.DateTime
}
