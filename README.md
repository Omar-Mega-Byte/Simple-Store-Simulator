# Simple Store Simulator 🛒

A comprehensive F# e-commerce simulation system with dual user interfaces (Console CLI + Modern GUI), SQLite database integration, advanced price calculation engine, and complete order management. This production-ready application demonstrates functional programming excellence with 17 automated tests and enterprise-level architecture.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Key Highlights](#key-highlights)
- [Objectives](#objectives)
- [Features](#features)
- [User Interfaces](#user-interfaces)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Database Information](#database-information)
- [Team Roles & Responsibilities](#team-roles--responsibilities)
- [Development Guidelines](#development-guidelines)
- [Data Structures](#data-structures)
- [Core Modules](#core-modules)
- [Testing Strategy](#testing-strategy)
- [Export Capabilities](#export-capabilities)
- [Contributing](#contributing)
- [Documentation](#documentation)
- [License](#license)

## 🎯 Project Overview

The Simple Store Simulator is a production-ready F# e-commerce application that demonstrates advanced functional programming concepts. Built with clean architecture principles, the system features dual user interfaces (Console CLI and Avalonia GUI), SQLite database integration with 33+ products, comprehensive testing suite with 100% pass rate, and enterprise-grade features including order management, discount engine, and multi-format data export capabilities.

**Development Period:** Fall 2024 - Winter 2025  
**Version:** 1.0 (Production Ready)  
**Target Framework:** .NET 6.0  
**Total Tests:** 17 Automated Unit Tests (All Passing)

## 🌟 Key Highlights

- ✅ **Dual User Interface**: Professional Console CLI + Modern Avalonia Windows GUI
- ✅ **Database Integration**: SQLite with 33+ products across 6 categories
- ✅ **Advanced Features**: Tax calculation, shipping costs, multi-tier discount engine
- ✅ **Data Export**: JSON, CSV, and Text format support with timestamped orders
- ✅ **Comprehensive Testing**: 17 automated tests with 100% pass rate
- ✅ **Functional Programming**: Pure functions, immutable data, pattern matching
- ✅ **Clean Architecture**: Multi-tier layered design with clear separation of concerns
- ✅ **Production Ready**: Error handling, validation, and professional UX

## 🎓 Objectives

- **Master Functional Programming**: Demonstrate F# syntax, immutability, and functional paradigms
- **Build Real-World Systems**: Create production-grade e-commerce application
- **Database Operations**: Integrate SQLite for persistent product catalog
- **UI Development**: Implement both CLI and GUI interfaces
- **Advanced Calculations**: Tax, shipping, discounts, and price calculations
- **Testing Excellence**: Achieve comprehensive test coverage
- **Team Collaboration**: Practice role-based agile development
- **Best Practices**: Apply clean code, SOLID principles, and documentation standards

## ✨ Features

### Core E-Commerce Features

1. **Product Catalog Management**
   - 33+ products across 6 categories (Snacks, Dairy, Beverages, Sweets, Fruits, Vegetables)
   - SQLite database integration with persistent storage
   - Real-time stock tracking and validation
   - Product attributes: ID, Name, Price (EGP), Description, Category, Stock
   - Categories: Snacks, Dairy, Beverages, Sweets, Fruits, Vegetables, Frozen

2. **Shopping Cart Operations**
   - Add products with quantity selection and stock validation
   - Remove products from cart
   - Update product quantities dynamically
   - View cart contents with line items
   - Automatic total calculation
   - Cart persistence across sessions
   - Immutable list-based implementation

3. **Advanced Price Calculation Engine**
   - Subtotal calculation per line item (price × quantity)
   - Cart subtotal aggregation
   - **Tax Calculation**: 14% VAT (configurable)
   - **Shipping Costs**: Free shipping over 200 EGP, otherwise 30 EGP
   - **Multi-Tier Discount System**:
     - Percentage-based discounts (e.g., 10% off)
     - Fixed amount discounts (e.g., 50 EGP off)
     - Buy X Get Y Free (e.g., Buy 3 Get 1 Free)
     - Minimum purchase requirements
   - Final total with all adjustments

4. **Search & Filter System**
   - **Search by Name**: Case-insensitive product search
   - **Filter by Price Range**: Min/max price filtering
   - **Filter by Category**: Browse by product category
   - **Sort Products**: By price (ascending/descending) or name
   - **Combined Filters**: Apply multiple criteria simultaneously
   - **Category Listing**: View all available categories

5. **Order Management & Persistence**
   - Generate unique order IDs (GUID-based)
   - Timestamped order creation (YYYYMMDD_HHMMSS)
   - Complete order summaries with all details
   - Save orders to JSON files in `data/orders/` directory
   - Order history tracking
   - Automatic file naming with order ID and timestamp

6. **Multi-Format Data Export**
   - **JSON Export**: Structured data with full order details
   - **CSV Export**: Spreadsheet-compatible format
   - **Text Export**: Human-readable receipt format
   - Customizable export paths
   - Error handling and validation

### User Interface Features

#### Console CLI Interface
- 🎨 Colored output (success, error, warning, info)
- 📦 Box-drawing ASCII art menus
- 🔢 Interactive numeric menu selection
- 💬 Input validation and error messaging
- 📊 Formatted tables and summaries
- 🎯 Clear navigation and workflow
- ⚡ Fast keyboard-driven interaction

#### Windows GUI Application (Avalonia)
- 🖥️ Modern desktop application
- 🎨 Professional UI with Fluent design
- 📋 Product grid with search and filters
- 🛒 Real-time shopping cart panel
- 🔍 Instant search functionality
- 📂 Category dropdown filtering
- 🛡️ Stock status indicators
- ✅ Add to cart buttons with validation
- 🗑️ Remove from cart functionality
- 💳 Checkout with order confirmation
- 📱 Responsive layout (resizable windows)
- 🎯 MVVM architecture pattern

## 🖥️ User Interfaces

The Simple Store Simulator features **TWO professional user interfaces** to suit different user preferences:
### 1 Windows GUI Application (Avalonia)

**Location:** `GUI/` folder

**Features:**
- ✨ Modern desktop application with Fluent design
- 🎨 Professional Windows interface
- 📋 **Product Grid**: Scrollable list with all product details
- 🛒 **Shopping Cart Panel**: Real-time cart updates
- 🔍 **Search Bar**: Instant product search as you type
- 📂 **Category Dropdown**: Quick filtering by category
- ➕ **Add to Cart Buttons**: One-click product addition
- 🗑️ **Remove Buttons**: Easy cart item removal
- 💳 **Checkout Button**: Complete order processing
- 📊 **Total Display**: Real-time price calculation
- 🛡️ **Stock Indicators**: Visual out-of-stock warnings
- 📱 **Responsive Design**: Resizable window with adaptive layout

**How to Run:**
```powershell
cd GUI
dotnet run
```

**UI Components:**

1. **Header Bar** (Dark Blue Background)
   - Store title: "🛒 Simple Store Simulator"
   - Cart item count badge

2. **Toolbar** (Light Gray Background)
   - Search textbox with placeholder "Search products..."
   - Search button
   - Category dropdown (All Categories, Snacks, Dairy, etc.)
   - Show All button

3. **Main Content** (Split View)
   - **Left Panel (60%)**: Product catalog
     - Product cards with name, price, description
     - Category and stock information
     - "Add to Cart" button (green, disabled if out of stock)
   - **Right Panel (40%)**: Shopping cart
     - Cart items with quantity and price
     - Remove buttons for each item
     - Subtotal, Tax, Shipping, Discount, Total
     - Checkout button

**Technology Stack:**
- **UI Framework**: Avalonia 11.3.9 (cross-platform .NET UI)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Data Binding**: Two-way binding with INotifyPropertyChanged
- **Navigation**: ViewLocator pattern

**Project File:** `GUI/SimpleStoreSimulator.GUI.fsproj`

## 🏗️ Architecture

### High-Level Architecture

```
                ┌─────────────────────────────────────────────────────┐
                │                   Presentation Layer                │
                │              (UI Module - Forms/Console)            │
                └──────────────────────┬──────────────────────────────┘
                                    │
                ┌──────────────────────▼──────────────────────────────┐
                │                  Application Layer                  │
                │         (Business Logic & Workflow Orchestration)   │
                │  ┌──────────────┐  ┌──────────────┐  ┌───────────┐  │
                │  │   Catalog    │  │     Cart     │  │  Checkout │  │
                │  │   Service    │  │   Service    │  │  Service  │  │
                │  └──────────────┘  └──────────────┘  └───────────┘  │
                └──────────────────────┬──────────────────────────────┘
                                    │
                ┌──────────────────────▼──────────────────────────────┐
                │                    Domain Layer                     │
                │              (Core Business Logic)                  │
                │  ┌──────────────┐  ┌──────────────┐  ┌───────────┐  │
                │  │   Product    │  │  CartItem    │  │   Order   │  │
                │  │    Types     │  │    Types     │  │   Types   │  │
                │  └──────────────┘  └──────────────┘  └───────────┘  │
                └──────────────────────┬──────────────────────────────┘
                                    │
                ┌──────────────────────▼──────────────────────────────┐
                │                 Data Access Layer                   │
                │            (JSON Persistence & File I/O)            │
                └─────────────────────────────────────────────────────┘
```

### Module Dependencies

```
UI Module
  ├── Catalog Module
  ├── Cart Module
  ├── Search Module
  └── Checkout Module
      └── Calculator Module
          └── FileIO Module
```

### Data Flow

```
User Input → UI Layer → Service Layer → Domain Logic → Data Layer
                                            ↓
User Output ← UI Layer ← Service Layer ← Pure Functions
```

## 🛠️ Technology Stack

### Core Technologies
- **Language**: F# (Functional-first .NET language)
- **Framework**: .NET 6.0
- **Build Tool**: .NET CLI / MSBuild
- **Version Control**: Git + GitHub

### Database & Data
- **Database**: SQLite 8.0.0 (Microsoft.Data.Sqlite)
- **Data Format**: JSON (System.Text.Json + FSharp.SystemTextJson 1.3.13)
- **Products**: 33+ items in SQLite database

### Testing
- **Test Framework**: Expecto 10.2.1 (F# testing framework)
- **Test SDK**: YoloDev.Expecto.TestSdk 0.14.2
- **Test Runner**: Microsoft.NET.Test.Sdk 17.8.0
- **Coverage**: 30 automated unit tests (100% pass rate)

### GUI (Avalonia Application)
- **UI Framework**: Avalonia 11.3.9 (cross-platform .NET UI)
- **Desktop Support**: Avalonia.Desktop 11.3.9
- **Theme**: Avalonia.Themes.Fluent 11.3.9
- **Fonts**: Avalonia.Fonts.Inter 11.3.9
- **Diagnostics**: Avalonia.Diagnostics 11.3.9 (Debug only)
- **MVVM**: CommunityToolkit.Mvvm 8.2.1

### NuGet Packages
```xml
<!-- Core Library -->
<PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.0" />
<PackageReference Include="FSharp.SystemTextJson" Version="1.3.13" />

<!-- Testing -->
<PackageReference Include="Expecto" Version="10.2.1" />
<PackageReference Include="YoloDev.Expecto.TestSdk" Version="0.14.2" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />

<!-- GUI Application -->
<PackageReference Include="Avalonia" Version="11.3.9" />
<PackageReference Include="Avalonia.Desktop" Version="11.3.9" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.3.9" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.1" />
```

### Development Tools
- **IDEs**: Visual Studio 2022, VS Code (with Ionide), JetBrains Rider
- **Extensions**: Ionide-fsharp (VS Code F# support)
- **Terminal**: PowerShell, CMD, Windows Terminal

## 📁 Project Structure

```
Simple Store Simulator/
├── 📄 SimpleStoreSimulator.fsproj    # Main library project file
├── 📄 README.md                      # This comprehensive guide
├── 📄 .gitignore                     # Git ignore rules
│
├── 📂 src/                           # Source code (Core Business Logic)
│   ├── 📄 Program.fs                 # Console UI entry point
│   │
│   ├── 📂 Product/                   # Product Module
│   │   ├── Product.fs                # Product types and catalog operations
│   │   └── ProductDatabase.fs        # SQLite database integration (33+ products)
│   │
│   ├── 📂 Cart/                      # Shopping Cart Module
│   │   ├── CartTypes.fs              # Cart and CartItem type definitions
│   │   ├── CartConfig.fs             # Cart configuration (tax, shipping)
│   │   └── CartOperations.fs         # Cart add/remove/update operations
│   │
│   ├── 📂 Calculator/                # Price Calculation Module
│   │   ├── PriceCalculator.fs        # Subtotal, tax, shipping, total calculations
│   │   └── DiscountEngine.fs         # Multi-tier discount system
│   │
│   ├── 📂 Search/                    # Search & Filter Module
│   │   ├── SearchTypes.fs            # Search criteria types
│   │   └── SearchOperations.fs       # Search, filter, sort functions
│   │
│   └── 📂 FileIO/                    # Data Persistence Module
│       ├── JsonSerializer.fs         # JSON serialization/deserialization
│       └── FileOperations.fs         # File I/O, CSV, text export
│
├── 📂 GUI/                           # Windows GUI Application (Avalonia)
│   ├── 📄 SimpleStoreSimulator.GUI.fsproj  # GUI project file
│   ├── 📄 Program.fs                 # GUI entry point
│   ├── 📄 App.axaml                  # Application XAML
│   ├── 📄 App.axaml.fs               # Application code-behind
│   ├── 📄 ViewLocator.fs             # MVVM view location
│   ├── 📄 Converters.fs              # Value converters for data binding
│   ├── 📄 app.manifest               # Windows application manifest
│   ├── 📄 README-GUI.md              # GUI-specific documentation
│   │
│   ├── 📂 ViewModels/                # MVVM ViewModels
│   │   ├── ViewModelBase.fs          # Base ViewModel class
│   │   └── MainWindowViewModel.fs    # Main window ViewModel
│   │
│   ├── 📂 Views/                     # MVVM Views
│   │   ├── MainWindow.axaml          # Main window XAML
│   │   └── MainWindow.axaml.fs       # Main window code-behind
│   │
│   ├── 📂 Assets/                    # GUI assets (icons, images)
│   ├── 📂 bin/                       # Build output
│   ├── 📂 obj/                       # Build intermediate files
│   │
│   └── 📂 data/                      # GUI-specific data
│       └── 📂 orders/                # Saved orders from GUI
│           ├── order_*.json          # Individual order files
│           └── ...                   # (100+ order files)
│
├── 📂 tests/                         # Automated Testing Suite
│   ├── 📄 SimpleStoreSimulator.Tests.fsproj  # Test project file
│   ├── 📄 Main.fs                    # Test runner entry point
│   ├── 📄 README.md                  # Testing documentation
│   │
│   ├── 📄 ProductTests.fs            # Product module tests (3 tests)
│   ├── 📄 CartOperationsTests.fs     # Cart operations tests (4 tests)
│   ├── 📄 PriceCalculatorTests.fs    # Price calculator tests (3 tests)
│   ├── 📄 SearchOperationsTests.fs   # Search & filter tests (3 tests)
│   ├── 📄 JsonAndDatabaseTests.fs    # JSON & DB tests (1 test)
│   ├── 📄 FileOperationsAutomationTests.fs  # File I/O tests (1 test)
│   └── 📄 DiscountEngineAutomationTests.fs  # Discount tests (2 tests)
│   
│   └── 📂 bin/, obj/                 # Test build outputs
│
├── 📂 docs/                          # Documentation
│   ├── 📄 PROJECT_DOCUMENTATION.md   # Comprehensive project docs (745 lines)
│   ├── 📄 ARCHITECTURE.md            # Detailed architecture guide
│   ├── 📄 System Architecture.drawio # Architecture diagrams
│   └── 📄 Untitled Diagram.drawio    # Additional diagrams
│
├── 📂 bin/                           # Build output directory
│   └── Debug/
│       ├── net6.0/                   # .NET 6.0 build
│       └── net8.0/                   # .NET 8.0 build (if applicable)
│
└── 📂 obj/                           # Build intermediate files
    ├── project.assets.json
    ├── *.nuget.dgspec.json
    ├── *.nuget.g.props
    ├── *.nuget.g.targets
    └── Debug/
```

### Key Directories Explained

- **`src/`**: Core business logic organized by domain (Product, Cart, Calculator, Search, FileIO)
- **`GUI/`**: Complete Avalonia GUI application with MVVM architecture
- **`tests/`**: 17 essential unit tests with 100% pass rate
- **`docs/`**: Complete project documentation and architecture diagrams
- **`GUI/data/orders/`**: Persistent storage for customer orders (100+ saved orders)

### Module Compilation Order (from .fsproj)

```
SearchTypes.fs → Product.fs → ProductDatabase.fs → SearchOperations.fs
→ CartTypes.fs → CartConfig.fs → PriceCalculator.fs → DiscountEngine.fs
→ JsonSerializer.fs → FileOperations.fs → CartOperations.fs
```

## 🚀 Getting Started

### Prerequisites

- **.NET SDK 6.0+**: Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- **F# Language Support**: Included with .NET SDK
- **Git**: For version control ([https://git-scm.com/](https://git-scm.com/))
- **IDE** (Choose one):
  - Visual Studio 2022 (with F# support)
  - Visual Studio Code (with Ionide-fsharp extension)
  - JetBrains Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Omar-Mega-Byte/Simple-Store-Simulator.git
   cd "Simple-Store-Simulator"
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run tests** (verify everything works)
   ```bash
   cd tests
   dotnet test
   ```
   Expected output: ✅ All 17 tests passed

## 🎮 Running the Application

### Option 1: Console/CLI Interface

Run the console-based interface:

```powershell
# From project root
dotnet run --project SimpleStoreSimulator.fsproj

# Or directly from src folder
cd src
dotnet run
```

### Option 2: Windows GUI Application

Run the Avalonia desktop application:

```powershell
# Navigate to GUI folder
cd GUI
dotnet run

# Or from project root
dotnet run --project GUI/SimpleStoreSimulator.GUI.fsproj
```

### Running Tests

```powershell
# Run all tests
cd tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test file
dotnet test --filter "FullyQualifiedName~ProductTests"
```

### Building for Release

```powershell
# Build optimized release version
dotnet build -c Release

# Publish self-contained executable (Windows)
dotnet publish -c Release -r win-x64 --self-contained

# GUI Application release build
cd GUI
dotnet publish -c Release -r win-x64 --self-contained
```

## 🗄️ Database Information

### SQLite Database

- **Location**: In-memory database (ProductDatabase.fs)
- **Products**: 33+ items across 6 categories
- **Schema**:
  ```sql
  CREATE TABLE Products (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Price REAL NOT NULL,
    Description TEXT,
    Category TEXT,
    Stock INTEGER DEFAULT 0
  )
  ```

### Product Categories

1. **Snacks** (8 products): Chips, Popcorn, Crackers, Nuts, Pretzels, Cookies, Energy Bars
2. **Dairy** (5 products): Milk, Cheese, Yogurt, Butter, Cream
3. **Beverages** (7 products): Water, Juice, Soda, Coffee, Tea, Energy Drinks
4. **Sweets** (6 products): Chocolate, Candy, Gummies, Mints
5. **Fruits** (4 products): Apples, Bananas, Oranges, Grapes
6. **Vegetables** (3 products): Tomatoes, Lettuce, Carrots

### Sample Products

| ID | Name | Price (EGP) | Category | Stock |
|---|---|---|---|---|
| 1 | Chips | 15.00 | Snacks | 50 |
| 2 | Milk | 25.00 | Dairy | 30 |
| 3 | Water | 5.00 | Beverages | 100 |
| 4 | Chocolate | 20.00 | Sweets | 40 |
| 5 | Apples | 30.00 | Fruits | 25 |

*See `src/Product/ProductDatabase.fs` for complete product list*

## 👥 Team Roles & Responsibilities

### 1. Catalog Developer - عمر أحمد محمود عواد
**Responsibilities:**
- Design and implement the `Product` type
- Initialize product catalog using Map data structure
- Create functions to retrieve and display products
- Implement product catalog structure

**Key Deliverables:**
- `Domain/Types.fs` - Product type definition
- `Catalog/CatalogOperations.fs` - Catalog functions
- `Catalog/ProductData.fs` - Sample product data

**Skills Focus:**
- F# record types
- Map data structure
- Immutable collections

---

### 2. Cart Logic Developer - باسل وليد حامد محمد
**Responsibilities:**
- Implement shopping cart operations
- Add products to cart with immutable list operations
- Remove products from cart
- Update product quantities
- Maintain cart state immutably

**Key Deliverables:**
- `Cart/CartTypes.fs` - Cart and CartItem types
- `Cart/CartOperations.fs` - Cart manipulation functions

**Skills Focus:**
- List operations
- Pattern matching
- Immutable state management
- Pure functions

---

### 3. Price Calculator - عمر أحمد محمد أحمد
**Responsibilities:**
- Implement price calculation functions
- Calculate subtotals for cart items
- Compute total cart value
- Apply discounts (if applicable)
- Implement tax calculations (optional)

**Key Deliverables:**
- `Calculator/PriceCalculator.fs` - Calculation functions
- `Calculator/DiscountEngine.fs` - Discount logic

**Skills Focus:**
- Mathematical operations in F#
- Function composition
- Option types for discounts

---

### 4. Search & Filter Developer - عمر أحمد الرفاعي طليس (Team Leader)
**Responsibilities:**
- Implement product search functionality
- Create filter functions (by price, category, etc.)
- Implement sorting operations
- Build query composition functions

**Key Deliverables:**
- `Search/SearchTypes.fs` - Search criteria types
- `Search/SearchOperations.fs` - Search and filter functions

**Skills Focus:**
- List filtering and mapping
- Higher-order functions
- Query expressions
- Function pipelines

---

### 5. File Save/Load Developer - مايكل عماد عدلي
**Responsibilities:**
- Implement JSON serialization
- Save cart summary to file
- Load product catalog from JSON
- Handle file I/O operations
- Manage data persistence

**Key Deliverables:**
- `FileIO/JsonSerializer.fs` - JSON operations
- `FileIO/FileOperations.fs` - File system functions
- `data/products.json` - Product data file

**Skills Focus:**
- JSON serialization/deserialization
- File I/O
- Error handling
- Side effects management

---

### 6. UI Developer - علي محمد جمعة زكي
**Responsibilities:**
- Design and implement user interface
- Create menu system
- Display product catalog
- Show cart contents
- Format output displays
- Handle user input

**Key Deliverables:**
- `UI/ConsoleUI.fs` - Main UI logic
- `UI/Menu.fs` - Menu system
- `UI/DisplayHelpers.fs` - Formatting functions

**Skills Focus:**
- Console I/O
- String formatting
- User interaction flows
- Integration of all modules

---

### 7. Testers - عمر أحمد الرفاعي طليس & جمال الدين أيمن عبد الرحمن
**Responsibilities:**
- Write unit tests for all modules
- Ensure cart operations correctness
- Test edge cases
- Validate calculation accuracy
- Integration testing
- Create test data

**Key Deliverables:**
- All files in `tests/` directory
- Test documentation
- Bug reports and fixes

**Skills Focus:**
- Unit testing in F#
- Test frameworks (Expecto, FsUnit)
- Test-driven development
- Quality assurance

---

### 8. Documentation & Version Control Leads

**Documentation Lead - كيرلس ساري عيد رومان**

**Responsibilities:**
- Maintain comprehensive documentation
- Create technical documentation files
- Document APIs and module interfaces
- Write team collaboration guidelines

**Key Deliverables:**
- README.md updates
- ARCHITECTURE.md
- API.md
- TEAM_GUIDE.md

**Skills Focus:**
- Technical writing
- Documentation standards
- Markdown formatting

---

**Version Control Lead - عمر أحمد الرفاعي طليس**

**Responsibilities:**
- Manage GitHub repository
- Review and merge pull requests
- Create and assign issues
- Track project progress
- Facilitate team collaboration

**Key Deliverables:**
- GitHub Issues and Projects setup
- PR review and merge management
- Branch management
- Repository organization

**Skills Focus:**
- Git workflows
- GitHub collaboration
- Project management
- Team coordination

## 📝 Development Guidelines

### Coding Standards

1. **Naming Conventions**
   - Use PascalCase for types and modules: `Product`, `CartOperations`
   - Use camelCase for functions and values: `addToCart`, `calculateTotal`
   - Use descriptive names: `productId` instead of `id`

2. **Function Design**
   - Keep functions pure (no side effects) whenever possible
   - Use pattern matching instead of if-else chains
   - Prefer function composition and pipelines
   - Keep functions small and focused (single responsibility)

3. **Immutability**
   - Never mutate data structures
   - Return new instances when state changes
   - Use immutable collections (List, Map, Set)

4. **Type Safety**
   - Define custom types for domain concepts
   - Use discriminated unions for state representation
   - Avoid primitive obsession

5. **Error Handling**
   - Use `Option` type for values that might not exist
   - Use `Result` type for operations that can fail
   - Provide meaningful error messages

### Git Workflow

1. Create feature branch from `main`
2. Make commits with descriptive messages
3. Push branch and create pull request
4. Request review from team members
5. Merge after approval

### Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

**Types:** feat, fix, docs, style, refactor, test, chore

**Example:**
```
feat: add product search by name

Implemented search function that filters products by name
using case-insensitive matching.

Closes #15
```

## 🗂️ Data Structures

### Core Type Definitions

#### Product Type
```fsharp
type Product = { 
    Id: int
    Name: string
    Price: decimal
    Description: string
    Category: string
    Stock: int
}
```

#### Cart Item Type
```fsharp
type CartItem = {
    Product: Product
    Quantity: int
}
```

#### Shopping Cart Type
```fsharp
type ShoppingCart = {
    Items: CartItem list
    CreatedAt: DateTime
}
```

#### Cart Configuration
```fsharp
type CartConfig = {
    TaxRate: decimal              // 14% VAT
    FreeShippingThreshold: decimal // 200 EGP
    ShippingCost: decimal          // 30 EGP
}
```

#### Order Summary Type
```fsharp
type OrderSummary = {
    OrderId: string               // GUID
    Items: CartItem list
    Subtotal: decimal
    Tax: decimal
    Shipping: decimal
    Discount: decimal
    Total: decimal
    OrderDate: DateTime
}
```

#### Discount Types
```fsharp
type DiscountType =
    | Percentage of float          // e.g., 10% off
    | FixedAmount of decimal       // e.g., 50 EGP off
    | BuyXGetYFree of int * int    // e.g., Buy 3 Get 1 Free

type Discount = {
    Name: string
    Type: DiscountType
    MinimumPurchase: decimal option
}
```

#### Search Criteria
```fsharp
type SearchCriteria = {
    Name: string option
    MinPrice: decimal option
    MaxPrice: decimal option
    Category: string option
}

type SortOption =
    | PriceAscending
    | PriceDescending
    | NameAscending
    | NameDescending
```

### Data Structure Principles

All data structures in this project follow functional programming principles:
- ✅ **Immutability**: All types are immutable records
- ✅ **Pure Functions**: Functions never mutate input parameters
- ✅ **Type Safety**: Strong typing prevents runtime errors
- ✅ **Pattern Matching**: Discriminated unions for state representation
- ✅ **Option Types**: Explicit handling of nullable values
- ✅ **Result Types**: Functional error handling

## 🧩 Core Modules

### 1. Product Module (`src/Product/`)

**Files:**
- `Product.fs` - Product types and catalog operations
- `ProductDatabase.fs` - SQLite integration with 33+ products

**Purpose:**
- Define Product type and catalog structure
- Initialize product database from SQLite
- Manage product inventory and stock levels

**Key Functions:**
```fsharp
// Database operations
loadProductsFromDatabase: unit -> Product list
insertProduct: SqliteConnection -> Product -> unit
updateProductStock: SqliteConnection -> int -> int -> unit

// Catalog operations
initializeCatalog: unit -> ProductCatalog
getProduct: ProductCatalog -> int -> Product option
getAllProducts: ProductCatalog -> Product list
getProductsByCategory: ProductCatalog -> string -> Product list
isInStock: Product -> int -> bool
updateStock: ProductCatalog -> int -> int -> ProductCatalog
formatPrice: decimal -> string
displayProduct: Product -> unit
displayCatalog: string -> ProductCatalog -> unit
```

**Product Categories:**
- Snacks, Dairy, Beverages, Sweets, Fruits, Vegetables, Frozen

---

### 2. Cart Module (`src/Cart/`)

**Files:**
- `CartTypes.fs` - Cart and CartItem type definitions
- `CartConfig.fs` - Configuration (tax rate, shipping)
- `CartOperations.fs` - Cart manipulation functions

**Purpose:**
- Manage shopping cart state immutably
- Handle add/remove/update operations
- Validate stock availability

**Key Functions:**
```fsharp
// Cart creation and queries
createCart: unit -> ShoppingCart
getCartItemCount: ShoppingCart -> int
isEmpty: ShoppingCart -> bool
findCartItem: Product -> ShoppingCart -> CartItem option

// Cart modifications
addToCart: Product -> int -> ShoppingCart -> ShoppingCart
removeFromCart: Product -> ShoppingCart -> ShoppingCart
updateQuantity: Product -> int -> ShoppingCart -> ShoppingCart
clearCart: ShoppingCart -> ShoppingCart

// Display and reporting
displayCart: ShoppingCart -> unit
getCartSummary: ShoppingCart -> string
```

**Configuration:**
- Tax Rate: 14% (VAT)
- Free Shipping Threshold: 200 EGP
- Standard Shipping Cost: 30 EGP

---

### 3. Calculator Module (`src/Calculator/`)

**Files:**
- `PriceCalculator.fs` - Price and total calculations
- `DiscountEngine.fs` - Multi-tier discount system

**Purpose:**
- Calculate subtotals, tax, shipping, and totals
- Apply various discount types
- Generate order summaries

**Key Functions:**
```fsharp
// Price calculations
calculateItemSubtotal: CartItem -> decimal
calculateSubtotal: ShoppingCart -> decimal
calculateTax: decimal -> decimal -> decimal
calculateShipping: decimal -> CartConfig -> decimal
calculateTotal: ShoppingCart -> CartConfig -> decimal

// Order processing
createOrderSummary: ShoppingCart -> CartConfig -> OrderSummary
displayOrderSummary: OrderSummary -> unit

// Discount engine
applyPercentageDiscount: decimal -> float -> decimal
applyFixedDiscount: decimal -> decimal -> decimal
applyBuyXGetYFree: CartItem -> int -> int -> decimal
calculateDiscount: CartItem list -> Discount -> decimal
```

**Discount Types:**
- Percentage (e.g., 10% off)
- Fixed Amount (e.g., 50 EGP off)
- Buy X Get Y Free (e.g., Buy 3 Get 1 Free)

---

### 4. Search Module (`src/Search/`)

**Files:**
- `SearchTypes.fs` - Search criteria and sort options
- `SearchOperations.fs` - Search, filter, and sort functions

**Purpose:**
- Enable product discovery and filtering
- Sort products by various criteria
- Combine multiple search filters

**Key Functions:**
```fsharp
// Search and filter
searchByName: Product list -> string -> Product list
filterByPriceRange: Product list -> decimal -> decimal -> Product list
filterByCategory: Product list -> string -> Product list
applyCriteria: Product list -> SearchCriteria -> Product list

// Sorting
sortProducts: Product list -> SortOption -> Product list
sortByPriceAsc: Product list -> Product list
sortByPriceDesc: Product list -> Product list
sortByName: Product list -> Product list

// Utilities
getCategories: Product list -> string list
displaySearchResults: Product list -> unit
```

**Search Capabilities:**
- Case-insensitive name search
- Price range filtering
- Category filtering
- Multiple sort options
- Combined criteria

---

### 5. FileIO Module (`src/FileIO/`)

**Files:**
- `JsonSerializer.fs` - JSON serialization/deserialization
- `FileOperations.fs` - File I/O and export functions

**Purpose:**
- Persist order data in JSON format
- Export data to multiple formats (JSON, CSV, Text)
- Handle file system operations with error handling

**Key Functions:**
```fsharp
// JSON operations
serializeToJson<'T>: 'T -> string
deserializeFromJson<'T>: string -> 'T option
saveToJsonFile<'T>: string -> 'T -> Result<unit, string>
loadFromJsonFile<'T>: string -> Result<'T, string>

// Order persistence
saveOrder: OrderSummary -> string -> Result<unit, string>
loadOrder: string -> Result<OrderSummary, string>
generateOrderFileName: string -> string

// Export formats
exportToCsv: CartItem list -> string -> Result<unit, string>
exportToText: OrderSummary -> string -> Result<unit, string>
ensureDirectoryExists: string -> unit
```

**File Formats:**
- **JSON**: Structured order data with full details
- **CSV**: Spreadsheet-compatible format for cart items
- **Text**: Human-readable receipt format

**Storage Location:**
- Console orders: `data/orders/`
- GUI orders: `GUI/data/orders/`

---

### 6. UI Module (`src/Program.fs`)

**Purpose:**
- Provide interactive console user interface
- Display menus and handle user input
- Coordinate all modules for complete workflow

**Features:**
- Colored console output (success/error/warning/info)
- ASCII box-drawing for visual appeal
- Input validation and error handling
- Menu-driven navigation
- Interactive prompts

**Main Menu Options:**
1. View All Products
2. Search Products
3. Browse by Category
4. Filter by Price Range
5. View Cart
6. Shopping Cart Management
7. Checkout
8. Product Statistics
9. Best Deals
0. Exit

---

### 7. GUI Module (`GUI/`)

**Architecture:** MVVM (Model-View-ViewModel)

**Components:**
- **ViewModels**: Business logic and state management
  - `ViewModelBase.fs` - Base class for ViewModels
  - `MainWindowViewModel.fs` - Main window logic
- **Views**: XAML-based UI
  - `MainWindow.axaml` - Main window layout
  - `MainWindow.axaml.fs` - Code-behind
- **Converters**: Data binding converters
  - `Converters.fs` - Value converters
- **App**: Application entry
  - `App.axaml` - App resources and styling
  - `Program.fs` - Entry point

**Features:**
- Product grid with search
- Shopping cart panel
- Real-time updates
- Category filtering
- Stock validation
- Checkout workflow

## 🧪 Testing Strategy

### Test Suite Overview

**Total Tests:** 17 automated unit tests  
**Pass Rate:** 100% ✅  
**Framework:** Expecto 10.2.1  
**Location:** `tests/` directory

### Test Files and Coverage

#### 1. ProductTests.fs (3 tests)
- ✅ Catalog initialization from database
- ✅ Get product by ID with correct data
- ✅ Update stock changes correctly

#### 2. CartOperationsTests.fs (4 tests)
- ✅ Add item to empty cart
- ✅ Add same item twice (quantity accumulation)
- ✅ Remove product from cart
- ✅ Checkout updates catalog stock

#### 3. PriceCalculatorTests.fs (3 tests)
- ✅ Calculate cart subtotal with multiple items
- ✅ Calculate tax from subtotal
- ✅ Calculate cart total with all fees

#### 4. SearchOperationsTests.fs (3 tests)
- ✅ Search by name finds matching products
- ✅ Filter by category returns only matching
- ✅ Apply search criteria filters correctly

#### 5. JsonAndDatabaseTests.fs (1 test)
- ✅ Database initializes successfully

#### 6. FileOperationsAutomationTests.fs (1 test)
- ✅ Save order creates JSON file successfully

#### 7. DiscountEngineAutomationTests.fs (2 tests)
- ✅ Apply percentage discount to cart
- ✅ Buy X Get Y discount calculation

### Running Tests

```powershell
# Run all tests
cd tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test suite
dotnet test --filter "FullyQualifiedName~CartOperationsTests"

# Run tests with coverage (if coverage tool installed)
dotnet test /p:CollectCoverage=true
```

### Test Output Example

```
Passed!  - Failed:     0, Passed:    17, Skipped:     0, Total:    17, Duration: 507 ms

[16:32:47 INF] EXPECTO? Running tests...
[16:32:47 INF] EXPECTO! 17 tests run - 17 passed, 0 failed, 0 errored, 0 ignored
```

### Testing Best Practices

- ✅ **Unit Testing**: Test each function in isolation
- ✅ **Edge Cases**: Test boundary conditions and edge cases
- ✅ **Error Handling**: Verify error conditions are handled properly
- ✅ **Pure Functions**: Leverage F# pure functions for predictable tests
- ✅ **Test Data**: Use realistic test data matching production scenarios
- ✅ **Continuous Testing**: Run tests before every commit
- ✅ **Regression Testing**: Ensure new features don't break existing functionality

### Test Coverage Goals

- ✅ **Achieved:** Essential business logic tested
- ✅ **Achieved:** Critical cart operations covered
- ✅ **Achieved:** Core calculation functions verified
- ✅ **Achieved:** Key search and filter operations tested
- ✅ **Achieved:** Database and file I/O operations validated

### Quality Assurance

- **No Warnings**: Clean compilation with zero warnings
- **No Errors**: All tests pass consistently
- **Code Reviews**: Peer-reviewed by team members
- **Functional Correctness**: Business rules validated through tests

## 📤 Export Capabilities

The system supports multiple export formats for order data:

### 1. JSON Export

**Format:** Structured JSON with complete order details

**Example Output:**
```json
{
  "OrderId": "6efb04ad-758c-426a-ad16-2c9bc7a9e305",
  "Items": [
    {
      "Product": {
        "Id": 1,
        "Name": "Chips",
        "Price": 15.00,
        "Description": "Crispy potato chips",
        "Category": "Snacks",
        "Stock": 50
      },
      "Quantity": 2
    }
  ],
  "Subtotal": 30.00,
  "Tax": 4.20,
  "Shipping": 30.00,
  "Discount": 0.00,
  "Total": 64.20,
  "OrderDate": "2025-12-13T12:33:09"
}
```

**File Location:** `GUI/data/orders/order_{GUID}_{TIMESTAMP}.json`

### 2. CSV Export

**Format:** Comma-separated values for spreadsheet import

**Example Output:**
```csv
ProductId,ProductName,Quantity,Price,Subtotal
1,Chips,2,15.00,30.00
4,Chocolate,1,20.00,20.00
```

**Use Cases:**
- Import into Excel/Google Sheets
- Data analysis
- Inventory reporting

### 3. Text Export

**Format:** Human-readable receipt

**Example Output:**
```
========================================
        ORDER RECEIPT
========================================
Order ID: 6efb04ad-758c-426a-ad16-2c9bc7a9e305
Date: 2025-12-13 12:33:09

----------------------------------------
ITEMS:
----------------------------------------
Chips                     x2    30.00 EGP
Chocolate                 x1    20.00 EGP

----------------------------------------
Subtotal:                      50.00 EGP
Tax (14%):                      7.00 EGP
Shipping:                      30.00 EGP
Discount:                       0.00 EGP
----------------------------------------
TOTAL:                         87.00 EGP
========================================
```

### Export Functions

```fsharp
// Save order to JSON
saveOrder: OrderSummary -> string -> Result<unit, string>

// Export cart items to CSV
exportToCsv: CartItem list -> string -> Result<unit, string>

// Export order summary to text receipt
exportToText: OrderSummary -> string -> Result<unit, string>
```

### Saved Orders

The application maintains a persistent history of all orders:

- **Location:** `GUI/data/orders/`
- **Count:** 100+ saved orders
- **Format:** `order_{GUID}_{YYYYMMDD_HHMMSS}.json`
- **Retention:** Permanent (until manually deleted)

## 🤝 Contributing

### How to Contribute

1. **Choose a Task**
   - Check GitHub Issues for assigned tasks
   - Coordinate with team members

2. **Create Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Implement & Test**
   - Write code following guidelines
   - Add unit tests
   - Test locally

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat: description of changes"
   ```

5. **Push & Create PR**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Code Review**
   - Address review comments
   - Update as needed

7. **Merge**
   - After approval, merge to main branch

### Code Review Checklist

- [ ] Code follows F# style guidelines
- [ ] Functions are pure and immutable
- [ ] Unit tests are included
- [ ] Documentation is updated
- [ ] No compiler warnings
- [ ] Builds successfully

## 📚 Documentation

Comprehensive documentation is available in the `docs/` directory:

### Available Documentation

1. **README.md** (This file)
   - Complete project overview
   - Installation and setup guide
   - Feature descriptions
   - Team roles and responsibilities
   - Development guidelines

2. **docs/PROJECT_DOCUMENTATION.md** (745 lines)
   - Executive summary
   - Detailed functional requirements
   - System architecture
   - Technical implementation details
   - Database design
   - Usage guide
   - Complete API reference

3. **docs/ARCHITECTURE.md**
   - Detailed architecture diagrams
   - Module dependencies
   - Data flow documentation
   - Design patterns used

4. **GUI/README-GUI.md** (337 lines)
   - GUI-specific documentation
   - Avalonia UI guide
   - MVVM architecture explanation
   - UI component descriptions
   - Screenshots and examples

5. **tests/README.md**
   - Testing strategy
   - Test suite documentation
   - How to write tests
   - Test execution guide

### Architecture Diagrams

- `docs/System Architecture.drawio` - System architecture diagram
- `docs/Untitled Diagram.drawio` - Additional diagrams

### Quick Links

- **Repository**: [https://github.com/Omar-Mega-Byte/Simple-Store-Simulator](https://github.com/Omar-Mega-Byte/Simple-Store-Simulator)
- **Issues**: [GitHub Issues](https://github.com/Omar-Mega-Byte/Simple-Store-Simulator/issues)
- **Project Lead**: omar.tolis2004@gmail.com

## 👨‍💻 Team Members

### أعضاء الفريق

- **عمر أحمد الرفاعي طليس** - Team Leader & Search/Filter Developer & Tester
- **عمر أحمد محمد أحمد** - Price Calculator Developer
- **عمر أحمد محمود عواد** - Catalog Developer
- **جمال الدين أيمن عبد الرحمن** - Tester
- **مايكل عماد عدلي** - File Save/Load Developer
- **باسل وليد حامد محمد** - Cart Logic Developer
- **علي محمد جمعة زكي** - UI Developer
- **كيرلس ساري عيد رومان** - Documentation Lead

## 📄 License

This project is developed for educational purposes as part of the Programming Languages - 3 course.

**Academic Information:**
- **Course**: Programming Languages - 3
- **Academic Year**: 2024-2025
- **Term**: First Term (Fall 2024 - Winter 2025)
- **Institution**: Faculty of Computers and Artificial Intelligence
- **Repository**: [https://github.com/Omar-Mega-Byte/Simple-Store-Simulator](https://github.com/Omar-Mega-Byte/Simple-Store-Simulator)

---

## 🎓 Learning Resources

### F# Learning Materials
- [F# for Fun and Profit](https://fsharpforfunandprofit.com/) - Comprehensive F# guide
- [F# Documentation](https://docs.microsoft.com/en-us/dotnet/fsharp/) - Official Microsoft docs
- [F# Cheat Sheet](https://dungpa.github.io/fsharp-cheatsheet/) - Quick reference

### Functional Programming Concepts Used
- ✅ **Immutability** - All data structures are immutable
- ✅ **Pure Functions** - Side-effect-free functions
- ✅ **Pattern Matching** - Elegant control flow
- ✅ **Higher-Order Functions** - Functions as parameters
- ✅ **Function Composition** - Pipe operators and composition
- ✅ **Discriminated Unions** - Type-safe state representation
- ✅ **Option Types** - Explicit nullable handling
- ✅ **Result Types** - Functional error handling
- ✅ **Record Types** - Immutable data structures
- ✅ **List Operations** - Functional list processing

### Tools & Libraries
- [Ionide](http://ionide.io/) - F# extension for VS Code
- [Expecto](https://github.com/haf/expecto) - F# testing framework
- [Avalonia](https://avaloniaui.net/) - Cross-platform .NET UI framework
- [SQLite](https://www.sqlite.org/) - Lightweight database engine
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview) - JSON library

---

## 🌟 Project Achievements

- ✅ **Complete Implementation**: All planned features delivered
- ✅ **Dual Interfaces**: CLI + GUI both fully functional
- ✅ **100% Test Pass Rate**: All 17 automated tests passing
- ✅ **Production Ready**: Error handling, validation, professional UX
- ✅ **Database Integration**: SQLite with 33+ products
- ✅ **Advanced Features**: Tax, shipping, multi-tier discounts
- ✅ **Data Persistence**: 100+ saved orders with multi-format export
- ✅ **Clean Architecture**: Well-organized, maintainable codebase
- ✅ **Comprehensive Documentation**: 2000+ lines of documentation
- ✅ **Team Collaboration**: Successful role-based development

---

**Built with 💖 using F# - Functional Programming for Real-World Applications**

**Developed by Team:**
- عمر أحمد الرفاعي طليس (Team Leader)
- عمر أحمد محمد أحمد
- عمر أحمد محمود عواد
- جمال الدين أيمن عبد الرحمن
- مايكل عماد عدلي
- باسل وليد حامد محمد
- علي محمد جمعة زكي
- كيرلس ساري عيد رومان

**© 2024-2025 | Simple Store Simulator Team**

