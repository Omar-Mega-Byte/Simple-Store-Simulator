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
| `ProductTests.fs` | 15 tests | Product catalog, retrieval, stock management |
| `CartOperationsTests.fs` | 24 tests | Adding/removing items, cart operations, checkout |
| `PriceCalculatorTests.fs` | 30 tests | Price calculations, tax, shipping, totals |
| `SearchOperationsTests.fs` | 18 tests | Search, filtering, sorting products |
| **TOTAL** | **87 tests** | **Complete coverage of core features** |

## Expected Output

When all tests pass, you should see:
```
Test run succeeded.
Total tests: 87
     Passed: 87
     Failed: 0
 Total time: ~1-2 seconds
```

## Test Coverage

✅ **Product Module** - Complete coverage of catalog operations  
✅ **Cart Module** - All cart operations including edge cases  
✅ **Calculator Module** - All pricing, tax, and shipping calculations  
✅ **Search Module** - All search, filter, and sort operations  

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
