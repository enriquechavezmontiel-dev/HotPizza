# 🍕 HotPizza - Pizza Catalog System

A console-based pizza catalog management system built with .NET 8.0, featuring clean architecture, dependency injection, and comprehensive testing.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Architecture](#architecture)
- [Testing](#testing)
- [Technologies](#technologies)
- [Development](#development)

## 🎯 Overview

HotPizza is a console application designed to manage a pizza catalog for a pizzeria. The application allows users to register new pizzas with comprehensive validation, automatic ID generation, and JSON-based persistence.

This project demonstrates modern .NET development practices including:
- Clean Architecture principles
- Dependency Injection
- Repository Pattern
- Service Layer Pattern
- Comprehensive unit testing with xUnit and Moq

## ✨ Features

### Core Functionality
- ✅ **Pizza Registration** - Register new pizzas with name, description, price, and size
- ✅ **Automatic ID Generation** - Each pizza receives a unique GUID
- ✅ **Data Persistence** - All data stored in JSON format
- ✅ **Input Validation** - Comprehensive validation with user-friendly error messages
- ✅ **Spanish UI** - All user-facing messages in Spanish
- ✅ **Error Handling** - Graceful error handling with detailed feedback

### Validation Rules
- Name is required
- Description is required
- Price must be greater than 0
- Size (in centimeters) must be greater than 0

### Technical Features
- Clean separation of concerns
- Interface-based design for testability
- Async/await for I/O operations
- Generic validator pattern
- Operation result pattern for type-safe returns
- Unit tested with 100% pass rate

## 📦 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor (Visual Studio 2022, Visual Studio Code, or Rider recommended)
- PowerShell or Bash terminal

## 🚀 Getting Started

### 1. Clone or Download

If you have Git:
```bash
git clone <repository-url>
cd HotPizza
```

### 2. Restore Dependencies

```bash
dotnet restore HotPizza.slnx
```

### 3. Build the Solution

```bash
dotnet build HotPizza.slnx
```

### 4. Run Tests (Optional but Recommended)

```bash
dotnet test HotPizza.slnx
```

Expected output:
```
Test summary: total: 15, failed: 0, succeeded: 15, skipped: 0
```

### 5. Run the Application

```bash
dotnet run --project HotPizza/HotPizza.csproj
```

## 💻 Usage

### Registering a Pizza

When you run the application, you'll be prompted to enter pizza details:

```
=== Registro de Pizza ===

Nombre: Margherita
Descripción: Tomate, mozzarella y albahaca
Precio: 12.50
Tamaño (cm): 30

Pizza registrada exitosamente con ID: 72eafb60-452f-4210-860d-30e3a42de08b
```

### Validation Examples

**Invalid Input (Negative Price):**
```
Nombre: Test
Descripción: Test Description
Precio: -5
Tamaño (cm): 30

Error al registrar la pizza:
El precio debe ser mayor a 0.
```

**Multiple Validation Errors:**
```
Nombre: 
Descripción: 
Precio: -10
Tamaño (cm): 25

Error al registrar la pizza:
El nombre es obligatorio.
La descripción es obligatoria.
El precio debe ser mayor a 0.
```

### Data Storage

All registered pizzas are stored in `pizzas.json` in the application directory:

```json
[
  {
	"Id": "72eafb60-452f-4210-860d-30e3a42de08b",
	"Name": "Margherita",
	"Description": "Tomate, mozzarella y albahaca",
	"Price": 12.50,
	"Size": 30
  }
]
```

## 📁 Project Structure

```
HotPizza/
├── HotPizza.slnx                          # Solution file
├── pizzas.json                            # Data persistence file
├── README.md                              # This file
├── IMPLEMENTATION_SUMMARY.md              # Implementation details
├── HotPizza/                              # Main application
│   ├── HotPizza.csproj
│   ├── Program.cs                         # Application entry point & DI setup
│   ├── Entities/
│   │   └── Pizza.cs                       # Pizza entity
│   ├── Services/
│   │   ├── IPizzaService.cs               # Service interface
│   │   ├── PizzaService.cs                # Business logic
│   │   └── OperationResult.cs             # Operation result wrapper
│   ├── Repositories/
│   │   ├── IPizzaRepository.cs            # Repository interface
│   │   └── JsonPizzaRepository.cs         # JSON persistence implementation
│   ├── Validators/
│   │   ├── IValidator.cs                  # Generic validator interface
│   │   ├── ValidationResult.cs            # Validation result
│   │   └── PizzaValidator.cs              # Pizza validation logic
│   └── UI/
│       └── ConsoleUI.cs                   # User interaction handler
└── HotPizza.Tests/                        # Test project
	├── HotPizza.Tests.csproj
	├── Validators/
	│   └── PizzaValidatorTests.cs         # Validator tests (7 tests)
	├── Repositories/
	│   └── JsonPizzaRepositoryTests.cs    # Repository tests (3 tests)
	└── Services/
		└── PizzaServiceTests.cs           # Service tests (4 tests)
```

## 🏗️ Architecture

### Layers

```
┌─────────────────────────────────────────┐
│              Program.cs                 │
│        (DI Configuration)               │
└────────────────┬────────────────────────┘
				 │
┌────────────────▼────────────────────────┐
│             UI Layer                    │
│          (ConsoleUI)                    │
└────────────────┬────────────────────────┘
				 │
┌────────────────▼────────────────────────┐
│          Service Layer                  │
│       (PizzaService)                    │
│    - Business Logic                     │
│    - Orchestration                      │
└─────┬──────────────────────┬────────────┘
	  │                      │
┌─────▼──────────┐   ┌───────▼────────────┐
│   Validator    │   │   Repository       │
│  (Pizza        │   │  (JsonPizza        │
│   Validator)   │   │   Repository)      │
│ - Validation   │   │ - Data Access      │
│   Rules        │   │ - Persistence      │
└────────────────┘   └────────────────────┘
		 │                    │
		 ▼                    ▼
┌────────────────┐   ┌────────────────────┐
│    Entities    │   │    pizzas.json     │
│    (Pizza)     │   │   (Storage)        │
└────────────────┘   └────────────────────┘
```

### Design Patterns

- **Dependency Injection** - Constructor injection throughout
- **Repository Pattern** - Abstract data access
- **Service Layer Pattern** - Business logic encapsulation
- **Strategy Pattern** - Generic validator interface
- **Result Pattern** - Type-safe operation results

### Key Components

**Entities:**
- `Pizza` - Core domain entity with Id, Name, Description, Price, Size

**Services:**
- `IPizzaService` / `PizzaService` - Orchestrates validation and persistence

**Repositories:**
- `IPizzaRepository` / `JsonPizzaRepository` - Handles data persistence

**Validators:**
- `IValidator<T>` / `PizzaValidator` - Validates business rules

**UI:**
- `ConsoleUI` - Manages user interaction

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test HotPizza.slnx

# Run with detailed output
dotnet test HotPizza.slnx --verbosity detailed

# Run specific test class
dotnet test --filter "FullyQualifiedName~PizzaValidatorTests"
```

### Test Coverage

| Component             | Tests | Coverage |
|----------------------|-------|----------|
| PizzaValidator       | 7     | All validation rules |
| JsonPizzaRepository  | 3     | Add, retrieve, multiple |
| PizzaService         | 4     | Success, failures, exceptions |
| **Total**            | **14** | **100% pass rate** |

### Test Details

**PizzaValidatorTests:**
- ✅ Valid pizza returns success
- ✅ Empty name returns error
- ✅ Empty description returns error
- ✅ Negative price returns error
- ✅ Zero price returns error
- ✅ Negative size returns error
- ✅ Zero size returns error

**JsonPizzaRepositoryTests:**
- ✅ Add pizza returns valid GUID
- ✅ Get all from empty file returns empty list
- ✅ Multiple pizzas persist correctly

**PizzaServiceTests:**
- ✅ Valid data returns success with ID
- ✅ Invalid data returns failure with errors
- ✅ Repository exception handled gracefully
- ✅ Multiple validation errors combined

## 🛠️ Technologies

### Core
- **.NET 8.0** - Application framework (LTS)
- **C# 12** - Programming language
- **System.Text.Json** - JSON serialization

### Dependency Injection
- **Microsoft.Extensions.Hosting** - Host builder and DI container
- **Microsoft.Extensions.DependencyInjection** - DI services

### Testing
- **xUnit** - Testing framework
- **Moq** - Mocking library for unit tests
- **Microsoft.NET.Test.Sdk** - Test SDK

## 👨‍💻 Development

### Code Conventions

- **Code Language:** English (classes, methods, variables, files)
- **UI Language:** Spanish (user messages)
- **Naming:** PascalCase for classes, camelCase for parameters
- **Async:** Use async/await for I/O operations
- **Nullability:** Nullable reference types enabled

### Building

```bash
# Clean build
dotnet clean HotPizza.slnx
dotnet build HotPizza.slnx

# Release build
dotnet build HotPizza.slnx --configuration Release
```

### Adding Features

To extend this application:

1. **New Entities** - Add to `Entities/` folder
2. **New Services** - Add interface to `Services/`, implement with tests
3. **New Validators** - Implement `IValidator<T>` interface
4. **New Repositories** - Implement `IPizzaRepository` or create new interface

### Future Enhancements

Potential features for expansion:
- [ ] List all pizzas
- [ ] Update pizza details
- [ ] Delete pizzas
- [ ] Search pizzas by name
- [ ] Filter by price range or size
- [ ] Export to different formats
- [ ] Database integration (SQL Server, PostgreSQL)
- [ ] Web API version
- [ ] Blazor UI

## 📝 Notes

### Data File Location
- The `pizzas.json` file is created in the application's running directory
- On first run, it will be created automatically
- The file uses indented JSON for readability

### Validation
- All validation messages are in Spanish for end users
- Validation happens before any persistence operations
- Multiple validation errors are displayed together

### Error Handling
- Service layer catches and wraps exceptions
- User-friendly error messages returned to UI
- Technical details logged but not exposed to users

## 📄 License

This project is created for educational purposes as part of the AI4Devs program.

## 👥 Contributing

This is a learning project. Feel free to:
- Report issues
- Suggest improvements
- Create pull requests

## 📞 Support

For questions or issues:
1. Check the [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)
2. Review the test cases for usage examples
3. Examine the architecture documentation above

---

**Built with ❤️ and .NET 8.0**

**Version:** 1.0  
**Last Updated:** 2025  
**Framework:** .NET 8.0 (LTS)
