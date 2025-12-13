# Test Suite - Quick Start Guide

## Running the Tests

### Prerequisites
- .NET SDK 6.0 or higher installed
- F# support enabled

### Quick Run
```bash
# From the tests directory
cd tests
dotnet test

# From the root directory
dotnet test tests/SimpleStoreSimulator.Tests.fsproj
```

## Test Summary

| Test File | Test Count | What It Tests |
|-----------|-----------|---------------|
| `ProductTests.fs` | 3 tests | Product catalog initialization, retrieval, stock management |
| `CartOperationsTests.fs` | 4 tests | Adding items, accumulation, removal, checkout |
| `PriceCalculatorTests.fs` | 3 tests | Subtotal, tax, and total calculations |
| `SearchOperationsTests.fs` | 3 tests | Search by name, filter by category, criteria application |
| `DiscountEngineAutomationTests.fs` | 2 tests | Percentage discount and Buy X Get Y promotions |
| `FileOperationsAutomationTests.fs` | 1 test | Order JSON file creation |
| `JsonAndDatabaseTests.fs` | 1 test | Database initialization |
| **TOTAL** | **17 tests** | **Essential coverage of core features** |

## Expected Output

When all tests pass, you should see:
```
Test run succeeded.
Total tests: 17
     Passed: 17
     Failed: 0
 Total time: ~1 second
```

## Test Coverage

✅ **Product Module** - Essential catalog operations and stock management  
✅ **Cart Module** - Critical cart operations and checkout flow  
✅ **Calculator Module** - Core pricing, tax, and total calculations  
✅ **Search Module** - Key search and filter operations  
✅ **Discount Module** - Main discount calculation features  
✅ **FileIO Module** - Order persistence functionality  
✅ **Database Module** - Database initialization verification  

## Documentation

See `TEST_DOCUMENTATION.md` for:
- Detailed explanation of each test
- F# syntax guide for testing
- How to write new tests
- Troubleshooting guide
- Best practices

## Quick Test Examples

### Run only Product tests
```bash
dotnet test --filter "FullyQualifiedName~ProductTests"
```

### Run only Cart tests
```bash
dotnet test --filter "FullyQualifiedName~CartOperationsTests"
```

### Run with verbose output
```bash
dotnet test -v n
```

## Support

For detailed documentation and explanations, refer to:
- **TEST_DOCUMENTATION.md** - Complete testing guide with F# syntax explanations
- **README.md** - Project overview and architecture

---

**Testing Team:**  
عمر أحمد الرفاعي طليس & جمال الدين أيمن عبد الرحمن
