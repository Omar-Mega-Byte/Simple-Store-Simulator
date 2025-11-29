# Simple Store Simulator - Dual UI System

**Developer:** علي محمد جمعة زكي (Ali Mohamed Gomaa Zaki)  
**UI Systems:** Console UI + Windows GUI  
**Status:** ✅ Production Ready

---

## 🎯 Overview

The Simple Store Simulator now features **TWO professional user interfaces**:

1. **Console UI** - Terminal-based interface (Already completed)
2. **Windows GUI** - Modern Avalonia-based graphical interface (NEW!)

---

## 🖥️ Option 1: Console/Terminal UI

### Features
- ✅ Text-based menu system
- ✅ Colored output (success, error, warning, info)
- ✅ ASCII box-drawing characters
- ✅ Interactive prompts
- ✅ Works in PowerShell, CMD, Windows Terminal

### How to Run
```powershell
cd "c:\Users\Ali\Downloads\supermarket\Simple-Store-Simulator"
dotnet run
```

### Screenshots
```
╔════════════════════════════════════════════════════════╗
║              🛒 SIMPLE STORE SIMULATOR 🛒             ║
╚════════════════════════════════════════════════════════╝

  [1] 📦 View All Products
  [2] 🔍 Search Products
  [3] 🏷️ Browse by Category
  [4] 💰 Filter by Price Range
  [5] 🛒 View Cart
  [6] 📝 Shopping Cart Management
  [7] 💳 Checkout
  [8] 📊 Product Statistics
  [9] ⭐ Best Deals
  [0] 🚪 Exit
```

---

## 🪟 Option 2: Windows GUI Application (NEW!)

### Features
- ✅ **Professional Windows Application** with modern UI
- ✅ **Product Grid** with search and filtering
- ✅ **Shopping Cart Panel** with real-time updates
- ✅ **Category Dropdown** for quick filtering
- ✅ **Search Bar** for instant product search
- ✅ **Add to Cart Buttons** on each product
- ✅ **Remove from Cart** functionality
- ✅ **Checkout System** with order confirmation
- ✅ **Real-time Total Calculation**
- ✅ **Stock Status Indicators**
- ✅ **Responsive Layout** (resizable window)

### How to Run
```powershell
cd "c:\Users\Ali\Downloads\supermarket\Simple-Store-Simulator\GUI"
dotnet run
```

### UI Components

#### 1. **Header Bar** (Dark Blue)
- Store title and branding
- Cart item count badge

#### 2. **Toolbar** (Light Gray)
- Search textbox with watermark
- Search button
- Category dropdown (All Categories, Sweets, Snacks, etc.)
- Show All button

#### 3. **Main Content Area**

**Left Panel - Products (60% width)**
- Scrollable product list
- Each product card shows:
  - Product name (bold)
  - Price (large, red)
  - Description
  - Category and stock info
  - "Add to Cart" button (green, disabled if out of stock)

**Right Panel - Shopping Cart (40% width)**
- Cart items list
- Each cart item shows:
  - Product name
  - Quantity × Price
  - Subtotal
  - Remove button (×)
- Total amount (highlighted)
- Checkout button (blue)
- Clear Cart button (red)

#### 4. **Status Bar** (Dark Gray)
- Status message
- Team credits
- Copyright info

### Technology Stack
- **UI Framework**: Avalonia UI 11.3.9
- **Pattern**: MVVM (Model-View-ViewModel)
- **Language**: F# 
- **Target Framework**: .NET 9.0
- **Cross-platform**: Works on Windows, macOS, Linux

### Architecture

```
GUI/
├── Views/
│   ├── MainWindow.axaml         # UI Layout (XAML)
│   └── MainWindow.axaml.fs      # View Code-behind
├── ViewModels/
│   ├── ViewModelBase.fs         # Base ViewModel
│   └── MainWindowViewModel.fs   # Main Window Logic
├── Program.fs                   # Application Entry Point
└── SimpleStoreSimulator.GUI.fsproj
```

### ViewModels

**ProductViewModel**
- Wraps Product type for UI binding
- Properties: Id, Name, Price, PriceDisplay, Description, Category, Stock, etc.
- Computed properties: IsInStock, IsLowStock

**CartItemViewModel**
- Represents cart line item
- Properties: Product, Quantity, Subtotal, SubtotalDisplay
- Mutable quantity for updates

**MainWindowViewModel**
- Main application logic
- Collections: Products, CartItems, Categories
- Commands: Search, AddToCart, RemoveFromCart, Checkout, ClearCart
- Methods: FilterProducts, UpdateCartTotals, etc.

### Data Binding

The GUI uses **two-way data binding**:
- `SearchText` → updates search textbox
- `SelectedCategory` → updates dropdown and triggers filtering
- `CartTotal` → displays in cart summary
- `CartItemCount` → shows in header badge
- `Products` → bound to product list
- `CartItems` → bound to cart items list

### Commands (Button Actions)

All UI actions are implemented as **RelayCommands**:
1. **SearchCommand** - Filters products by search text
2. **AddToCartCommand** - Adds product to cart
3. **RemoveFromCartCommand** - Removes item from cart
4. **CheckoutCommand** - Completes purchase
5. **ClearCartCommand** - Empties cart
6. **ShowAllProductsCommand** - Resets filters

---

## 📊 Comparison

| Feature | Console UI | Windows GUI |
|---------|-----------|-------------|
| **Interface Type** | Text-based | Graphical |
| **Platform** | Cross-platform | Cross-platform (Avalonia) |
| **Input Method** | Keyboard (numbers) | Mouse + Keyboard |
| **Visual Appeal** | ASCII art, colors | Modern Windows design |
| **Learning Curve** | Simple | Intuitive |
| **Navigation** | Menu-based | Direct interaction |
| **Product Display** | List/Table | Grid with cards |
| **Cart View** | Separate screen | Side panel (always visible) |
| **Search** | Input then search | Live filtering |
| **Best For** | Terminal users, automation | Desktop users, visual interaction |

---

## 🚀 Deployment

### Console UI
```powershell
# Publish standalone executable
cd "c:\Users\Ali\Downloads\supermarket\Simple-Store-Simulator"
dotnet publish -c Release -r win-x64 --self-contained

# Output: bin\Release\net6.0\win-x64\publish\SimpleStoreSimulator.exe
```

### Windows GUI
```powershell
# Publish standalone executable
cd "c:\Users\Ali\Downloads\supermarket\Simple-Store-Simulator\GUI"
dotnet publish -c Release -r win-x64 --self-contained

# Output: bin\Release\net9.0\win-x64\publish\SimpleStoreSimulator.GUI.exe
```

---

## 🔧 Customization

### Changing Colors (GUI)

Edit `MainWindow.axaml` and modify the `Background` properties:

```xml
<!-- Header -->
<Border Background="#2C3E50">  <!-- Dark Blue -->

<!-- Cart Button -->
<Button Background="#E74C3C">  <!-- Red -->

<!-- Search Button -->
<Button Background="#3498DB">  <!-- Blue -->

<!-- Add to Cart -->
<Button Background="#27AE60">  <!-- Green -->
```

### Adding New Features (GUI)

1. **Add property to ViewModel**:
```fsharp
member val MyNewProperty = "" with get, set
```

2. **Add binding in XAML**:
```xml
<TextBlock Text="{Binding MyNewProperty}" />
```

3. **Add command**:
```fsharp
member this.MyCommand = RelayCommand(fun () -> this.DoSomething())
```

---

## 📦 Dependencies

### Console UI
- Product module
- SearchTypes module
- SearchOperations module
- DisplayHelpers module
- Menu module

### Windows GUI
- All Console UI dependencies
- Avalonia UI (11.3.9)
- CommunityToolkit.Mvvm (8.2.1)
- .NET 9.0 Runtime

---

## 🐛 Troubleshooting

### GUI Not Opening?
1. **Check .NET version**: `dotnet --version` (should be 9.0+)
2. **Rebuild project**: `dotnet clean && dotnet build`
3. **Check output**: Look for error messages in terminal
4. **Windows only?**: Avalonia works on Windows, macOS, Linux

### Products Not Showing?
- Check that Product catalog is initialized
- Verify project reference in `.fsproj`
- Ensure Product module compiles successfully

### Buttons Not Working?
- Commands may need null checks
- Check if `IsEnabled` binding is correct
- Verify ViewModel command is properly bound

---

## 🎨 Future Enhancements

### Console UI
- [ ] Pagination for large product lists
- [ ] Advanced filtering options
- [ ] Order history view
- [ ] Multi-language support

### Windows GUI
- [ ] Product images
- [ ] Quantity spinners in cart
- [ ] Custom dialogs for checkout
- [ ] Drag-and-drop cart management
- [ ] Print receipt functionality
- [ ] Dark/Light theme toggle
- [ ] Animations and transitions
- [ ] Settings panel
- [ ] Customer login system
- [ ] Order history view

---

## 👥 Team Credits

**UI Developer:** علي محمد جمعة زكي  
- Console UI (DisplayHelpers, Menu, ConsoleUI)
- Windows GUI (Avalonia MVVM implementation)

**Integration with:**
- **Product Module**: عمر أحمد محمود عواد
- **Search Module**: عمر أحمد الرفاعي طليس
- **Cart Module** (Future): باسل وليد حامد محمد
- **Calculator Module** (Future): عمر أحمد محمد أحمد
- **FileIO Module** (Future): مايكل عماد عدلي

---

## 📄 License

Educational project for Programming Languages - 3 course  
Faculty of Computers and Artificial Intelligence  
Academic Year: 2025-2026

---

**Built with ❤️ using F# & Avalonia UI**

Both UIs share the same business logic, ensuring consistency and maintainability! 🎉
