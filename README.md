# Simple Store Simulator 🛒

A virtual F# store application with cart functionality and product management capabilities. This project demonstrates functional programming concepts including immutable data structures, pure functions, and state management in F#.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Objectives](#objectives)
- [Features](#features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Team Roles & Responsibilities](#team-roles--responsibilities)
- [Development Guidelines](#development-guidelines)
- [Data Structures](#data-structures)
- [Core Modules](#core-modules)
- [Testing Strategy](#testing-strategy)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Project Overview

The Simple Store Simulator is an educational project designed to help students learn F# functional programming concepts through building a practical e-commerce application. Users can browse products, manage shopping carts, and complete checkout processes—all implemented using immutable data structures and pure functions.

## 🎓 Objectives

- **Learn Functional Programming**: Master F# syntax and functional programming paradigms
- **Master Data Structures**: Work with immutable Lists and Maps
- **Practice Pure Functions**: Implement side-effect-free functions
- **Understand State Management**: Handle application state immutably
- **Apply Real-World Scenarios**: Build a practical e-commerce system
- **Collaborate as a Team**: Experience role-based software development

## ✨ Features

### Core Features
1. **Product Catalog Management**
   - Store products in an immutable Map data structure
   - Product attributes: ID, Name, Price, Description, Stock quantity
   - View complete product listings

2. **Shopping Cart Operations**
   - Add products to cart with quantity selection
   - Remove products from cart
   - Update product quantities
   - View cart contents
   - Immutable list-based cart implementation

3. **Price Calculation**
   - Calculate subtotals for individual items
   - Compute cart total
   - Apply discounts (optional enhancement)
   - Tax calculation (optional enhancement)

4. **Search & Filter**
   - Search products by name
   - Filter by price range
   - Filter by category
   - Sort products (by price, name, etc.)

5. **Data Persistence**
   - Save cart summary to JSON
   - Export purchase history
   - Load product catalog from JSON
   - Session state management

6. **User Interface**
   - Browse product catalog
   - Interactive cart management
   - Checkout summary display
   - Forms for product browsing and cart operations

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

- **Language**: F# (.NET 6.0 or higher)
- **Build Tool**: .NET CLI / MSBuild
- **Data Format**: JSON (for persistence)
- **Libraries**:
  - FSharp.Core (standard library)
  - System.Text.Json or Newtonsoft.Json (JSON serialization)
  - FsUnit (testing - optional)
  - Expecto (testing framework - optional)

## 📁 Project Structure

```
Simple Store Simulator/
├── src/
│   ├── Domain/
│   │   ├── Types.fs              # Core domain types (Product, Cart, Order)
│   │   └── DomainTypes.fs        # Additional type definitions
│   ├── Catalog/
│   │   ├── CatalogTypes.fs       # Catalog-specific types
│   │   ├── CatalogOperations.fs  # Product catalog operations
│   │   └── ProductData.fs        # Sample product data
│   ├── Cart/
│   │   ├── CartTypes.fs          # Cart-specific types
│   │   └── CartOperations.fs     # Cart add/remove/update logic
│   ├── Calculator/
│   │   ├── PriceCalculator.fs    # Price and total calculations
│   │   └── DiscountEngine.fs     # Discount logic (optional)
│   ├── Search/
│   │   ├── SearchTypes.fs        # Search criteria types
│   │   └── SearchOperations.fs   # Filter and search functions
│   ├── FileIO/
│   │   ├── JsonSerializer.fs     # JSON save/load operations
│   │   └── FileOperations.fs     # File system interactions
│   ├── UI/
│   │   ├── ConsoleUI.fs          # Console interface
│   │   ├── Menu.fs               # Menu system
│   │   └── DisplayHelpers.fs     # Display formatting functions
│   └── Program.fs                # Application entry point
├── tests/
│   ├── Domain.Tests/
│   ├── Catalog.Tests/
│   ├── Cart.Tests/
│   ├── Calculator.Tests/
│   ├── Search.Tests/
│   └── FileIO.Tests/
├── data/
│   ├── products.json             # Product catalog data
│   └── orders/                   # Saved order summaries
├── docs/
│   ├── ARCHITECTURE.md           # Detailed architecture documentation
│   ├── API.md                    # Module APIs and function signatures
│   └── TEAM_GUIDE.md             # Team collaboration guidelines
├── .gitignore
├── README.md
├── SimpleStoreSimulator.fsproj   # F# project file
└── LICENSE
```

## 🚀 Getting Started

### Prerequisites

- **.NET SDK 6.0+**: Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- **F# Language Support**: Included with .NET SDK
- **IDE** (Choose one):
  - Visual Studio 2022 (with F# support)
  - Visual Studio Code (with Ionide-fsharp extension)
  - JetBrains Rider
- **Git**: For version control and collaboration

### Project Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "Simple Store Simulator"
   ```

2. **Review the architecture**
   - Read this README thoroughly
   - Understand the module structure
   - Review team roles and responsibilities

3. **Set up development environment**
   - Install required prerequisites
   - Configure your IDE for F# development
   - Install recommended extensions (Ionide for VS Code)

4. **Plan your implementation**
   - Review your assigned role
   - Check the project structure
   - Read development guidelines

### Next Steps for Team

- Create F# project file (`.fsproj`)
- Set up project structure according to architecture
- Create module files based on responsibilities
- Implement types in Domain module first
- Build modules incrementally
- Write tests alongside implementation
- Document your code

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

### Core Types Overview

**Product Type**
- Product ID (unique identifier)
- Product Name
- Price (decimal for precision)
- Description
- Category
- Stock quantity

**Cart Item Type**
- Reference to Product
- Quantity ordered

**Shopping Cart Type**
- List of Cart Items
- Creation timestamp

**Order Summary Type**
- Unique Order ID
- List of ordered items
- Subtotal amount
- Tax amount
- Total amount
- Order date and time

**Product Catalog Type**
- Map structure with Product ID as key and Product as value

### State Management Principles

All state must be managed immutably in this project:
- Never modify existing data structures
- Always return new instances when state changes
- Use F#'s immutable collections (List, Map, Set)
- Functions should be pure (no side effects)

## 🧩 Core Modules

### 1. Domain Module
Defines core business types and domain logic.

**Purpose:**
- Central type definitions for the entire application
- Domain validation rules
- Business rule enforcement

**Components:**
- Product type definition
- Cart-related types
- Order types
- Catalog structure

---

### 2. Catalog Module
Manages product catalog operations.

**Purpose:**
- Initialize and maintain product catalog
- Provide access to product information
- Manage product inventory

**Required Functions:**
- Initialize empty or pre-populated catalog
- Retrieve product by ID
- Get all products as a list
- Filter products by category
- Check stock availability
- Update product stock levels

---

### 3. Cart Module
Handles shopping cart operations.

**Purpose:**
- Manage shopping cart state
- Handle cart item operations
- Maintain cart integrity

**Required Functions:**
- Create empty cart
- Add product to cart (with quantity)
- Remove product from cart
- Update product quantity in cart
- Clear entire cart
- Get cart item count
- Check if cart is empty

---

### 4. Calculator Module
Performs price calculations.

**Purpose:**
- Calculate prices and totals
- Apply discounts
- Calculate taxes

**Required Functions:**
- Calculate item subtotal (price × quantity)
- Calculate cart total (sum of all items)
- Apply discount percentage to price
- Calculate tax amount
- Calculate final total with tax
- Create order summary from cart

---

### 5. Search Module
Implements search and filter operations.

**Purpose:**
- Enable product discovery
- Filter product listings
- Sort products

**Required Functions:**
- Search products by name (case-insensitive)
- Filter products by price range
- Filter products by category
- Sort products by price (ascending/descending)
- Sort products by name (ascending/descending)
- Get list of all unique categories
- Apply multiple search criteria

---

### 6. FileIO Module
Handles data persistence.

**Purpose:**
- Save and load data
- JSON serialization
- File system operations

**Required Functions:**
- Save order summary to JSON file
- Load product catalog from JSON
- Export cart summary to file
- Generic JSON serialization
- Generic JSON deserialization
- File read/write operations with error handling

---

### 7. UI Module
Provides user interface.

**Purpose:**
- Display information to users
- Collect user input
- Navigate between features

**Required Functions:**
- Display main menu
- Display product listings
- Display cart contents
- Display order summary
- Get user input (text and numbers)
- Display success/error/info messages
- Format prices as currency
- Clear screen and wait for user input

## 🧪 Testing Strategy

### Unit Testing
- Test each function in isolation
- Use property-based testing where applicable
- Cover edge cases and error conditions
- Test with valid and invalid inputs

### Integration Testing
- Test module interactions
- Verify workflow correctness
- Test data persistence
- Ensure modules work together properly

### Test Coverage Goals
- Minimum 80% code coverage
- 100% coverage for critical cart operations
- All calculation functions fully tested
- All edge cases covered

### Testing Best Practices
- Write tests before or alongside implementation
- Use descriptive test names
- Test one thing per test case
- Keep tests independent
- Use test frameworks like Expecto or FsUnit
- Mock external dependencies when needed

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

## 📚 Resources

### F# Learning Resources
- [F# for Fun and Profit](https://fsharpforfunandprofit.com/)
- [F# Documentation](https://docs.microsoft.com/en-us/dotnet/fsharp/)
- [F# Cheat Sheet](https://dungpa.github.io/fsharp-cheatsheet/)

### Functional Programming Concepts
- Immutability
- Pure Functions
- Pattern Matching
- Higher-Order Functions
- Function Composition
- Discriminated Unions
- Option and Result Types

### Tools & Libraries
- [Ionide](http://ionide.io/) - F# VS Code extension
- [Expecto](https://github.com/haf/expecto) - Testing framework
- [FsUnit](https://fsprojects.github.io/FsUnit/) - Unit testing library
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)

## 📞 Contact & Support

- **Project Repository**: [https://github.com/Omar-Mega-Byte/Simple-Store-Simulator](https://github.com/Omar-Mega-Byte/Simple-Store-Simulator)
- **Project Lead Contact**: omar.tolis2004@gmail.com
- **Issue Tracking**: GitHub Issues
- **Documentation**: `/docs` directory

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

---

**Built with 💖 using F# - Functional Programming for Real-World Applications**

**Course**: Programming Languages - 3
**Academic Year**: 2025-2026
**Term**: First Term 
**Institution**: Faculty of Computers and Artificial Intelligence

