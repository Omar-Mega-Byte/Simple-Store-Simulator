type Product = { Name: string; Price: int; Stock: int }

let ProductCatalog =
    Map.ofList [
        1, { Name = "Chocolate"; Price = 15; Stock = 10 }
        2, { Name = "Biscuits";  Price = 10; Stock = 20 }
        3, { Name = "Ice Cream"; Price = 12; Stock = 5  }
    ]

let displayCatalog (title: string) (catalog: Map<int, Product>) =
    printfn "%s" title
    if Map.isEmpty catalog then
        printfn "No products available."
    else
        catalog |> Map.iter (fun k p -> printfn "  %d -> Name: %s, Price: %d, Stock: %d" k p.Name p.Price p.Stock)
    printfn ""

displayCatalog "Initial inventory:" ProductCatalog