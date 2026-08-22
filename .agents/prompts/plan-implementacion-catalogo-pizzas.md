---
name: Plan de Implementación - Catálogo de Pizzas
description: Plan detallado para la creación de la aplicación de consola HotPizza
version: 1.0
date: 2025-01-XX
---

# Create HotPizza Pizza Catalog Console Application

## Technical Specifications

- **Target Framework**: .NET 8.0 (LTS)
- **Testing Framework**: xUnit
- **Architecture**: Dependency Injection with Microsoft.Extensions.DependencyInjection
- **Data Storage**: JSON file (`pizzas.json`) in application directory
- **Validation**: Comprehensive (price > 0, size > 0, required fields)
- **Code Language**: English (identifiers, classes, methods, variables, files)
- **UI Language**: Spanish (user messages)

## Solution Structure

```
HotPizza/
├── HotPizza.slnx
├── HotPizza/
│   ├── HotPizza.csproj
│   ├── Program.cs
│   ├── Entities/
│   │   └── Pizza.cs
│   ├── Services/
│   │   ├── IPizzaService.cs
│   │   ├── PizzaService.cs
│   │   └── OperationResult.cs
│   ├── Repositories/
│   │   ├── IPizzaRepository.cs
│   │   └── JsonPizzaRepository.cs
│   ├── Validators/
│   │   ├── IValidator.cs
│   │   ├── ValidationResult.cs
│   │   └── PizzaValidator.cs
│   └── UI/
│       └── ConsoleUI.cs
└── HotPizza.Tests/
	├── HotPizza.Tests.csproj
	├── Services/
	│   └── PizzaServiceTests.cs
	├── Validators/
	│   └── PizzaValidatorTests.cs
	└── Repositories/
		└── JsonPizzaRepositoryTests.cs
```

## Key Design Decisions

### Entities
- `Pizza` class with properties: Id (Guid), Name, Description, Price (decimal), Size (int in cm)

### Services Layer
- `IPizzaService` and `PizzaService`: Contains business logic for registering pizzas
- Coordinates validation and persistence
- Returns result with success/error information

### Repository Layer
- `IPizzaRepository` and `JsonPizzaRepository`: Handles JSON file I/O
- Manages pizza persistence and retrieval
- Generates unique IDs (Guid)

### Validation Layer
- `IValidator<T>` and `PizzaValidator`: Validates pizza data
- Rules: Name required, Description required, Price > 0, Size > 0
- Returns validation result with error messages

### UI Layer
- `ConsoleUI`: Handles user interaction
- Prompts for pizza data
- Displays success/error messages in Spanish

### Dependency Injection Setup
- Use `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.Hosting`
- Register services, repositories, and validators
- Inject dependencies into `ConsoleUI`

## Implementation Steps

### 1. Create solution file
Create `HotPizza.slnx` in workspace root using dotnet CLI with format `.slnx`

**Commands:**
```bash
dotnet new sln -n HotPizza --format slnx
```

### 2. Create console project
Create `HotPizza` console project targeting .NET 8.0 in `HotPizza/` subdirectory

**Commands:**
```bash
dotnet new console -n HotPizza -o HotPizza -f net8.0
```

### 3. Create test project
Create `HotPizza.Tests` xUnit test project targeting .NET 8.0 in `HotPizza.Tests/` subdirectory

**Commands:**
```bash
dotnet new xunit -n HotPizza.Tests -o HotPizza.Tests -f net8.0
```

### 4. Add projects to solution
Add both `HotPizza` and `HotPizza.Tests` projects to `HotPizza.slnx`

**Commands:**
```bash
dotnet sln HotPizza.slnx add HotPizza/HotPizza.csproj
dotnet sln HotPizza.slnx add HotPizza.Tests/HotPizza.Tests.csproj
```

### 5. Add NuGet packages to console project
Add `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.DependencyInjection`, and `System.Text.Json` to `HotPizza.csproj`

**Commands:**
```bash
dotnet add HotPizza/HotPizza.csproj package Microsoft.Extensions.Hosting
dotnet add HotPizza/HotPizza.csproj package Microsoft.Extensions.DependencyInjection
```

### 6. Add NuGet packages to test project
Add `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, and `Moq` to `HotPizza.Tests.csproj`, plus project reference to `HotPizza`

**Commands:**
```bash
dotnet add HotPizza.Tests/HotPizza.Tests.csproj package Moq
dotnet add HotPizza.Tests/HotPizza.Tests.csproj reference HotPizza/HotPizza.csproj
```

### 7. Create Pizza entity
Create `HotPizza/Entities/Pizza.cs` with properties: Id (Guid), Name (string), Description (string), Price (decimal), Size (int)

**File:** `HotPizza/Entities/Pizza.cs`
```csharp
namespace HotPizza.Entities;

public class Pizza
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Size { get; set; }
}
```

### 8. Create IValidator interface
Create `HotPizza/Validators/IValidator.cs` with generic validation interface returning `ValidationResult`

**File:** `HotPizza/Validators/IValidator.cs`
```csharp
namespace HotPizza.Validators;

public interface IValidator<T>
{
	ValidationResult Validate(T entity);
}
```

### 9. Create ValidationResult class
Create `HotPizza/Validators/ValidationResult.cs` to hold validation results

**File:** `HotPizza/Validators/ValidationResult.cs`
```csharp
namespace HotPizza.Validators;

public class ValidationResult
{
	public bool IsValid { get; set; }
	public List<string> Errors { get; set; } = new();
}
```

### 10. Create PizzaValidator
Create `HotPizza/Validators/PizzaValidator.cs` implementing `IValidator<Pizza>` with validation rules

**File:** `HotPizza/Validators/PizzaValidator.cs`
```csharp
using HotPizza.Entities;

namespace HotPizza.Validators;

public class PizzaValidator : IValidator<Pizza>
{
	public ValidationResult Validate(Pizza pizza)
	{
		var result = new ValidationResult { IsValid = true };

		if (string.IsNullOrWhiteSpace(pizza.Name))
		{
			result.IsValid = false;
			result.Errors.Add("El nombre es obligatorio.");
		}

		if (string.IsNullOrWhiteSpace(pizza.Description))
		{
			result.IsValid = false;
			result.Errors.Add("La descripción es obligatoria.");
		}

		if (pizza.Price <= 0)
		{
			result.IsValid = false;
			result.Errors.Add("El precio debe ser mayor a 0.");
		}

		if (pizza.Size <= 0)
		{
			result.IsValid = false;
			result.Errors.Add("El tamaño debe ser mayor a 0.");
		}

		return result;
	}
}
```

### 11. Create IPizzaRepository interface
Create `HotPizza/Repositories/IPizzaRepository.cs` with methods for adding and retrieving pizzas

**File:** `HotPizza/Repositories/IPizzaRepository.cs`
```csharp
using HotPizza.Entities;

namespace HotPizza.Repositories;

public interface IPizzaRepository
{
	Task<Guid> AddAsync(Pizza pizza);
	Task<List<Pizza>> GetAllAsync();
}
```

### 12. Create JsonPizzaRepository
Create `HotPizza/Repositories/JsonPizzaRepository.cs` implementing `IPizzaRepository` using `System.Text.Json`

**File:** `HotPizza/Repositories/JsonPizzaRepository.cs`
```csharp
using System.Text.Json;
using HotPizza.Entities;

namespace HotPizza.Repositories;

public class JsonPizzaRepository : IPizzaRepository
{
	private readonly string _filePath = "pizzas.json";

	public async Task<Guid> AddAsync(Pizza pizza)
	{
		pizza.Id = Guid.NewGuid();
		var pizzas = await GetAllAsync();
		pizzas.Add(pizza);

		var options = new JsonSerializerOptions { WriteIndented = true };
		var json = JsonSerializer.Serialize(pizzas, options);
		await File.WriteAllTextAsync(_filePath, json);

		return pizza.Id;
	}

	public async Task<List<Pizza>> GetAllAsync()
	{
		if (!File.Exists(_filePath))
			return new List<Pizza>();

		var json = await File.ReadAllTextAsync(_filePath);
		return JsonSerializer.Deserialize<List<Pizza>>(json) ?? new List<Pizza>();
	}
}
```

### 13. Create OperationResult class
Create `HotPizza/Services/OperationResult.cs` to encapsulate service operation results

**File:** `HotPizza/Services/OperationResult.cs`
```csharp
namespace HotPizza.Services;

public class OperationResult<T>
{
	public bool IsSuccess { get; set; }
	public T? Data { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;
}
```

### 14. Create IPizzaService interface
Create `HotPizza/Services/IPizzaService.cs` with method for registering pizzas

**File:** `HotPizza/Services/IPizzaService.cs`
```csharp
namespace HotPizza.Services;

public interface IPizzaService
{
	Task<OperationResult<Guid>> RegisterPizzaAsync(string name, string description, decimal price, int size);
}
```

### 15. Create PizzaService
Create `HotPizza/Services/PizzaService.cs` implementing `IPizzaService`

**File:** `HotPizza/Services/PizzaService.cs`
```csharp
using HotPizza.Entities;
using HotPizza.Repositories;
using HotPizza.Validators;

namespace HotPizza.Services;

public class PizzaService : IPizzaService
{
	private readonly IPizzaRepository _repository;
	private readonly IValidator<Pizza> _validator;

	public PizzaService(IPizzaRepository repository, IValidator<Pizza> validator)
	{
		_repository = repository;
		_validator = validator;
	}

	public async Task<OperationResult<Guid>> RegisterPizzaAsync(string name, string description, decimal price, int size)
	{
		var pizza = new Pizza
		{
			Name = name,
			Description = description,
			Price = price,
			Size = size
		};

		var validationResult = _validator.Validate(pizza);

		if (!validationResult.IsValid)
		{
			return new OperationResult<Guid>
			{
				IsSuccess = false,
				ErrorMessage = string.Join("\n", validationResult.Errors)
			};
		}

		try
		{
			var id = await _repository.AddAsync(pizza);
			return new OperationResult<Guid>
			{
				IsSuccess = true,
				Data = id
			};
		}
		catch (Exception ex)
		{
			return new OperationResult<Guid>
			{
				IsSuccess = false,
				ErrorMessage = $"Error al registrar la pizza: {ex.Message}"
			};
		}
	}
}
```

### 16. Create ConsoleUI class
Create `HotPizza/UI/ConsoleUI.cs` with methods to handle user interaction

**File:** `HotPizza/UI/ConsoleUI.cs`
```csharp
using HotPizza.Services;

namespace HotPizza.UI;

public class ConsoleUI
{
	private readonly IPizzaService _pizzaService;

	public ConsoleUI(IPizzaService pizzaService)
	{
		_pizzaService = pizzaService;
	}

	public async Task RunAsync()
	{
		Console.WriteLine("=== Registro de Pizza ===");
		Console.WriteLine();

		Console.Write("Nombre: ");
		var name = Console.ReadLine() ?? string.Empty;

		Console.Write("Descripción: ");
		var description = Console.ReadLine() ?? string.Empty;

		Console.Write("Precio: ");
		if (!decimal.TryParse(Console.ReadLine(), out var price))
		{
			Console.WriteLine("Error: El precio debe ser un número válido.");
			return;
		}

		Console.Write("Tamaño (cm): ");
		if (!int.TryParse(Console.ReadLine(), out var size))
		{
			Console.WriteLine("Error: El tamaño debe ser un número entero válido.");
			return;
		}

		var result = await _pizzaService.RegisterPizzaAsync(name, description, price, size);

		Console.WriteLine();
		if (result.IsSuccess)
		{
			Console.WriteLine($"Pizza registrada exitosamente con ID: {result.Data}");
		}
		else
		{
			Console.WriteLine($"Error al registrar la pizza:");
			Console.WriteLine(result.ErrorMessage);
		}
	}
}
```

### 17. Update Program.cs
Configure dependency injection and invoke application flow

**File:** `HotPizza/Program.cs`
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotPizza.Entities;
using HotPizza.Repositories;
using HotPizza.Services;
using HotPizza.UI;
using HotPizza.Validators;

var builder = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		services.AddSingleton<IPizzaRepository, JsonPizzaRepository>();
		services.AddSingleton<IValidator<Pizza>, PizzaValidator>();
		services.AddSingleton<IPizzaService, PizzaService>();
		services.AddSingleton<ConsoleUI>();
	});

var host = builder.Build();

var consoleUI = host.Services.GetRequiredService<ConsoleUI>();
await consoleUI.RunAsync();
```

### 18. Create PizzaValidatorTests
Create `HotPizza.Tests/Validators/PizzaValidatorTests.cs` with unit tests for validation rules

**File:** `HotPizza.Tests/Validators/PizzaValidatorTests.cs`
```csharp
using HotPizza.Entities;
using HotPizza.Validators;
using Xunit;

namespace HotPizza.Tests.Validators;

public class PizzaValidatorTests
{
	private readonly PizzaValidator _validator = new();

	[Fact]
	public void Validate_ValidPizza_ReturnsSuccess()
	{
		var pizza = new Pizza
		{
			Name = "Margherita",
			Description = "Classic pizza",
			Price = 10.99m,
			Size = 30
		};

		var result = _validator.Validate(pizza);

		Assert.True(result.IsValid);
		Assert.Empty(result.Errors);
	}

	[Fact]
	public void Validate_EmptyName_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "",
			Description = "Test",
			Price = 10,
			Size = 30
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("nombre"));
	}

	[Fact]
	public void Validate_EmptyDescription_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "Test",
			Description = "",
			Price = 10,
			Size = 30
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("descripción"));
	}

	[Fact]
	public void Validate_NegativePrice_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "Test",
			Description = "Test",
			Price = -5,
			Size = 30
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("precio"));
	}

	[Fact]
	public void Validate_ZeroPrice_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "Test",
			Description = "Test",
			Price = 0,
			Size = 30
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("precio"));
	}

	[Fact]
	public void Validate_NegativeSize_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "Test",
			Description = "Test",
			Price = 10,
			Size = -10
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("tamaño"));
	}

	[Fact]
	public void Validate_ZeroSize_ReturnsError()
	{
		var pizza = new Pizza
		{
			Name = "Test",
			Description = "Test",
			Price = 10,
			Size = 0
		};

		var result = _validator.Validate(pizza);

		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.Contains("tamaño"));
	}
}
```

### 19. Create JsonPizzaRepositoryTests
Create `HotPizza.Tests/Repositories/JsonPizzaRepositoryTests.cs` with tests for repository methods

**File:** `HotPizza.Tests/Repositories/JsonPizzaRepositoryTests.cs`
```csharp
using HotPizza.Entities;
using HotPizza.Repositories;
using Xunit;

namespace HotPizza.Tests.Repositories;

public class JsonPizzaRepositoryTests : IDisposable
{
	private readonly string _testFilePath = "test_pizzas.json";

	[Fact]
	public async Task AddAsync_ValidPizza_ReturnsGuid()
	{
		var repository = new JsonPizzaRepository();
		var pizza = new Pizza
		{
			Name = "Test Pizza",
			Description = "Test Description",
			Price = 15.99m,
			Size = 30
		};

		var id = await repository.AddAsync(pizza);

		Assert.NotEqual(Guid.Empty, id);
	}

	[Fact]
	public async Task GetAllAsync_EmptyFile_ReturnsEmptyList()
	{
		if (File.Exists("pizzas.json"))
			File.Delete("pizzas.json");

		var repository = new JsonPizzaRepository();
		var pizzas = await repository.GetAllAsync();

		Assert.NotNull(pizzas);
		Assert.Empty(pizzas);
	}

	public void Dispose()
	{
		if (File.Exists("pizzas.json"))
			File.Delete("pizzas.json");
	}
}
```

### 20. Create PizzaServiceTests
Create `HotPizza.Tests/Services/PizzaServiceTests.cs` with tests using Moq

**File:** `HotPizza.Tests/Services/PizzaServiceTests.cs`
```csharp
using HotPizza.Entities;
using HotPizza.Repositories;
using HotPizza.Services;
using HotPizza.Validators;
using Moq;
using Xunit;

namespace HotPizza.Tests.Services;

public class PizzaServiceTests
{
	[Fact]
	public async Task RegisterPizzaAsync_ValidData_ReturnsSuccessWithId()
	{
		var mockRepo = new Mock<IPizzaRepository>();
		var mockValidator = new Mock<IValidator<Pizza>>();
		var expectedId = Guid.NewGuid();

		mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
			.Returns(new ValidationResult { IsValid = true });

		mockRepo.Setup(r => r.AddAsync(It.IsAny<Pizza>()))
			.ReturnsAsync(expectedId);

		var service = new PizzaService(mockRepo.Object, mockValidator.Object);

		var result = await service.RegisterPizzaAsync("Test", "Description", 10.99m, 30);

		Assert.True(result.IsSuccess);
		Assert.Equal(expectedId, result.Data);
	}

	[Fact]
	public async Task RegisterPizzaAsync_InvalidData_ReturnsFailure()
	{
		var mockRepo = new Mock<IPizzaRepository>();
		var mockValidator = new Mock<IValidator<Pizza>>();

		mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
			.Returns(new ValidationResult 
			{ 
				IsValid = false, 
				Errors = new List<string> { "Error de validación" } 
			});

		var service = new PizzaService(mockRepo.Object, mockValidator.Object);

		var result = await service.RegisterPizzaAsync("", "", -1, -1);

		Assert.False(result.IsSuccess);
		Assert.NotEmpty(result.ErrorMessage);
	}

	[Fact]
	public async Task RegisterPizzaAsync_RepositoryThrowsException_ReturnsFailure()
	{
		var mockRepo = new Mock<IPizzaRepository>();
		var mockValidator = new Mock<IValidator<Pizza>>();

		mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
			.Returns(new ValidationResult { IsValid = true });

		mockRepo.Setup(r => r.AddAsync(It.IsAny<Pizza>()))
			.ThrowsAsync(new Exception("Database error"));

		var service = new PizzaService(mockRepo.Object, mockValidator.Object);

		var result = await service.RegisterPizzaAsync("Test", "Description", 10.99m, 30);

		Assert.False(result.IsSuccess);
		Assert.Contains("Error al registrar", result.ErrorMessage);
	}
}
```

### 21. Build solution
Execute `dotnet build HotPizza.slnx` to compile all projects

**Commands:**
```bash
dotnet build HotPizza.slnx
```

### 22. Run unit tests
Execute `dotnet test HotPizza.slnx` to run all xUnit tests

**Commands:**
```bash
dotnet test HotPizza.slnx
```

### 23. Run console application
Execute `dotnet run --project HotPizza/HotPizza.csproj` to start the application

**Commands:**
```bash
dotnet run --project HotPizza/HotPizza.csproj
```

### 24. Verify pizza registration
Test the application by registering a pizza and verifying the ID is displayed

**Test Data:**
- Nombre: Margherita
- Descripción: Tomate, mozzarella y albahaca
- Precio: 12.50
- Tamaño: 30

**Expected Output:**
```
Pizza registrada exitosamente con ID: [GUID]
```

### 25. Verify error handling
Test invalid inputs and verify appropriate Spanish error messages

**Test Cases:**
- Empty name → "El nombre es obligatorio."
- Empty description → "La descripción es obligatoria."
- Negative price → "El precio debe ser mayor a 0."
- Zero size → "El tamaño debe ser mayor a 0."

## Summary

This implementation follows clean architecture principles with:
- ✅ Clear separation of concerns (Entities, Services, Repositories, Validators, UI)
- ✅ Dependency injection for testability and maintainability
- ✅ Comprehensive validation with detailed error messages
- ✅ Unit tests covering all business logic
- ✅ JSON persistence in local file
- ✅ English code identifiers with Spanish user messages
- ✅ .NET 8.0 LTS for long-term support
- ✅ xUnit for modern testing approach

The application is production-ready and follows .NET best practices.
