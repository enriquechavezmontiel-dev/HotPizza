---
description: "Use when creating or modifying C# application or test code in HotPizza. Covers layered console architecture, pizza validation, user-facing Spanish messages, English identifiers, xUnit testing, and JSON persistence safeguards."
name: "HotPizza C# Conventions"
applyTo:
  - "HotPizza/**/*.cs"
  - "HotPizza.Tests/**/*.cs"
---

# HotPizza C# Conventions

## Application Code

- Keep responsibilities separated: `UI` handles console input/output, `Services` orchestrate business workflows, `Validators` own validation rules, `Repositories` own JSON persistence, and `Entities` define data.
- Do not put business rules or persistence logic in `Program.cs`; use it only for dependency registration and application startup.
- Use English names for C# identifiers, files, classes, methods, properties, variables, and parameters. Keep console messages and validation errors in Spanish.
- Keep validation rules reusable in `Validators`. `ConsoleUI` may validate individual input fields for immediate feedback, but `PizzaService` must still validate the complete `Pizza` before calling `IPizzaRepository.AddAsync`.
- Do not persist invalid pizzas. Rejected input must not reach `AddAsync`.
- Keep pizza size validation restricted to `20`, `30`, or `40` cm. Required text cannot be empty or whitespace-only; prices must be greater than zero.
- Preserve the JSON catalog format and existing registration and consultation behavior unless the task explicitly changes them.

## Console UI

- Use `IConsoleAdapter` instead of direct `System.Console` calls in `ConsoleUI`.
- When collecting a required field, validate after Enter. Display the Spanish error and retry only that field until the value is valid.
- Preserve the menu loop: register, consult catalog, and exit. Catalog output must include identifier, name, description, price, and size; an empty catalog must say `El catálogo está vacío.`

## Tests

- Use xUnit and Moq following the existing test organization: validators in `HotPizza.Tests/Validators`, service tests in `HotPizza.Tests/Services`, repository tests in `HotPizza.Tests/Repositories`, and console tests in `HotPizza.Tests/UI`.
- Before adding a test, inspect existing coverage and add only a distinct behavior or edge case. Do not modify production code or weaken existing tests merely to make tests pass.
- Test validators directly, service behavior with mocked repositories, repository persistence with isolated test files, and console workflows with a fake `IConsoleAdapter`.
- For invalid registration, verify `IPizzaRepository.AddAsync` is never called. For console input, verify retries occur and `IPizzaService.RegisterPizzaAsync` is called only after all fields are valid.
- Run focused tests after each change, then run `dotnet build HotPizza.slnx` and `dotnet test HotPizza.slnx`. Collect coverage with `dotnet test HotPizza.slnx --collect:"XPlat Code Coverage"` when coverage is requested.
