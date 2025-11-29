# UI Module Documentation

**Developer:** علي محمد جمعة زكي (Ali Mohamed Gomaa Zaki)  
**Role:** UI Developer  
**Module:** User Interface (UI)

## Overview

The UI module provides a comprehensive console-based user interface for the Simple Store Simulator. It includes formatting utilities, menu navigation, and complete user interaction flows.

## Module Structure

```
src/UI/
├── DisplayHelpers.fs    # Formatting and display functions
├── Menu.fs              # Menu system and navigation
└── ConsoleUI.fs         # Main UI logic and integration
```

## Files Description

### 1. DisplayHelpers.fs

**Purpose:** Provides formatting and display utilities for consistent UI presentation.

**Key Features:**
- Price and number formatting
- Colored console output (success, error, warning, info)
- Box and border drawing
- Product display in multiple formats (table, detailed, compact)
- Progress indicators and status messages
- Input/output helpers

**Main Functions:**
- `formatPrice` - Format decimal as currency (EGP)
- `formatNumber` - Format integers with thousand separators
- `printSuccess/Error/Warning/Info` - Colored console messages
- `displayProductTable` - Display products in table format
- `displayProductDetailed` - Show product with full details
- `displayWelcomeScreen/ExitScreen` - Application screens
- `clearScreen` - Clear console
- `waitForUser` - Pause for user interaction

### 2. Menu.fs

**Purpose:** Implements menu system with navigation and input handling.

**Key Features:**
- Menu creation and configuration
- User selection validation
- Predefined menus (main, search, cart, sort)
- Input prompts for various data types
- Confirmation dialogs
- Category and item selection

**Main Types:**
```fsharp
type MenuOption = {
    Key: string
    Label: string
    Description: string option
    Icon: string option
}

type Menu = {
    Title: string
    Options: MenuOption list
    ShowExit: bool
}
```

**Main Functions:**
- `createMenu` - Build menu structure
- `displayMenu` - Show menu to user
- `showMenuAndGetSelection` - Display and get valid input
- `confirmAction` - Yes/no confirmation
- `promptForInt/Decimal/String` - Typed input prompts
- `displayCategorySelection` - Category picker
- `promptForPriceRange` - Price range input

### 3. ConsoleUI.fs

**Purpose:** Main UI orchestration integrating all modules (Product, Search, Cart).

**Key Features:**
- Application state management
- Complete user workflows
- Product browsing and searching
- Shopping cart operations (placeholder for Cart module)
- Checkout process
- Statistics and reporting

**Main Functions:**
- `startApplication` - Entry point for UI
- `handleViewAllProducts` - Browse catalog
- `handleSearchProducts` - Search interface
- `handleBrowseByCategory` - Category filtering
- `handleFilterByPrice` - Price range filtering
- `handleViewCart` - View cart contents
- `handleCartManagement` - Add/remove items
- `handleCheckout` - Complete purchase
- `handleStatistics` - View stats
- `handleBestDeals` - Show cheapest products

## Usage

### Starting the Application

```fsharp
open ConsoleUI
open Product

[<EntryPoint>]
let main argv =
    let catalog = initializeCatalog()
    startApplication catalog
    0
```

### Integration with Other Modules

The UI module integrates with:

1. **Product Module** - Access catalog and product data
2. **Search Module** - Filter and search operations
3. **Cart Module** (Future) - Shopping cart management
4. **Calculator Module** (Future) - Price calculations
5. **FileIO Module** (Future) - Data persistence

## Features Implemented

### ✅ Completed Features

1. **Product Catalog Display**
   - Table view with all details
   - Detailed view with full information
   - Compact list view

2. **Search Functionality**
   - Search by name
   - Search by name or description
   - Display results with stock status

3. **Category Browsing**
   - List all categories
   - Filter by selected category
   - Sort results by price

4. **Price Filtering**
   - Input min/max price range
   - Display matching products
   - Show in-stock items only

5. **Shopping Cart (Basic)**
   - Add products with quantity
   - Remove products
   - View cart summary
   - Clear cart
   - Calculate totals

6. **Checkout Process**
   - Review order
   - Confirm purchase
   - Generate order ID
   - Display receipt

7. **Statistics**
   - Product count by status
   - Price statistics (min, max, avg)
   - Category breakdown
   - Low stock alerts

8. **Best Deals**
   - Show cheapest products
   - Filter in-stock only
   - Display top 5 deals

### 🚧 Features Requiring Other Modules

These features are implemented but need integration with other team members' modules:

1. **Advanced Cart Operations** (Requires Cart Module by باسل وليد حامد محمد)
   - Update quantities
   - Proper cart item management
   - Cart persistence

2. **Price Calculations** (Requires Calculator Module by عمر أحمد محمد أحمد)
   - Tax calculations
   - Discount application
   - Subtotal calculations

3. **Data Persistence** (Requires FileIO Module by مايكل عماد عدلي)
   - Save orders to JSON
   - Load catalog from file
   - Export purchase history

## UI Design Principles

1. **Clarity** - Clear, easy-to-read layouts
2. **Consistency** - Uniform styling across all screens
3. **Feedback** - Immediate visual feedback for actions
4. **Error Handling** - Friendly error messages
5. **Navigation** - Intuitive menu structure
6. **Accessibility** - Support for RTL text (Arabic names)

## Color Coding

- 🟢 **Green (Success)** - Successful operations, confirmations
- 🔴 **Red (Error)** - Errors, failures, out of stock
- 🟡 **Yellow (Warning)** - Warnings, low stock, cautions
- 🔵 **Cyan (Info)** - Information messages, tips
- 🟣 **Magenta (Highlight)** - Important highlighted text

## Menu Structure

```
Main Menu
├── View All Products
├── Search Products
│   ├── Search by Name
│   ├── Search by Description
│   └── Advanced Search
├── Browse by Category
│   └── [Dynamic Category List]
├── Filter by Price Range
├── View Cart
├── Shopping Cart Management
│   ├── Add Product
│   ├── Remove Product
│   ├── Update Quantity
│   ├── View Cart Summary
│   └── Clear Cart
├── Checkout
├── Product Statistics
├── Best Deals
└── Exit
```

## Testing

The UI has been tested with:
- ✅ Product catalog display (5 sample products)
- ✅ Search functionality
- ✅ Category filtering (Sweets, Snacks, Frozen, Beverages)
- ✅ Price range filtering
- ✅ Cart operations (add, remove, view, clear)
- ✅ Checkout flow
- ✅ Statistics display
- ✅ Best deals display
- ✅ Navigation between menus
- ✅ Input validation
- ✅ Error handling

## Dependencies

- **F# Core** - Base F# functionality
- **System** - Console I/O
- **Product Module** - Product types and catalog
- **SearchTypes Module** - Search criteria types
- **SearchOperations Module** - Search and filter functions

## Future Enhancements

1. **Advanced Features**
   - Product recommendations
   - Search history
   - Favorites/Wishlist
   - Order history view

2. **UI Improvements**
   - Pagination for large lists
   - Sorting options menu
   - Keyboard shortcuts
   - Help system

3. **Integration**
   - Cart module integration
   - Calculator module integration
   - FileIO module integration
   - Real-time stock updates

## Code Quality

- ✅ Pure functions where possible
- ✅ Immutable data structures
- ✅ Pattern matching for control flow
- ✅ Descriptive function names
- ✅ Comprehensive comments
- ✅ Consistent formatting
- ✅ Error handling
- ✅ Type safety

## Build & Run

### Build
```powershell
dotnet build
```

### Run
```powershell
dotnet run
```

### Clean
```powershell
dotnet clean
```

## Troubleshooting

### Issue: Console encoding errors (missing emojis)
**Solution:** The application automatically sets UTF-8 encoding:
```fsharp
Console.OutputEncoding <- System.Text.Encoding.UTF8
```

### Issue: Colors not showing in terminal
**Solution:** Use Windows Terminal or PowerShell 7+ for best color support

### Issue: Screen not clearing properly
**Solution:** Console.Clear() is called at appropriate points

## Team Integration Notes

### For Cart Logic Developer (باسل وليد حامد محمد)
- Replace placeholder cart implementation in `ConsoleUI.fs`
- Update `AppState` type to use proper Cart type
- Implement `addToCart`, `removeFromCart`, `updateQuantity` functions

### For Calculator Developer (عمر أحمد محمد أحمد)
- Integrate price calculation functions in checkout
- Add discount application UI
- Implement tax calculation display

### For FileIO Developer (مايكل عماد عدلي)
- Add save/load functionality to cart operations
- Implement order export after checkout
- Add catalog load from JSON file

## Contact

**Developer:** علي محمد جمعة زكي  
**Module:** UI (User Interface)  
**Status:** ✅ Complete and Tested  
**Version:** 1.0

---

**Note:** This module provides a fully functional UI ready for integration with other team modules. All placeholder implementations are clearly marked and ready to be replaced with actual module functionality.
