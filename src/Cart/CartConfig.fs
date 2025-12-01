module CartConfig

open CartTypes

/// Default checkout configuration with standard tax and shipping rates
let defaultConfig: CheckoutConfig = {
    TaxRate = 0.14m // 14% tax
    ShippingRates = (10m, 20m, 30m) // Tier 1, 2, 3 shipping fees (EGP)
}

/// Get the checkout configuration (can be extended to load from file/database)
let getConfig () : CheckoutConfig = defaultConfig
