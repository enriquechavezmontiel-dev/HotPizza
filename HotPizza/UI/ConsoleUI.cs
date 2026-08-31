using HotPizza.Validators;
using HotPizza.Services;

namespace HotPizza.UI;

public class ConsoleUI
{
    private readonly IPizzaService _pizzaService;
    private readonly IConsoleAdapter _console;
    private readonly NameFieldValidator _nameValidator;
    private readonly DescriptionFieldValidator _descriptionValidator;
    private readonly PriceFieldValidator _priceValidator;
    private readonly SizeFieldValidator _sizeValidator;

    public ConsoleUI(
        IPizzaService pizzaService,
        IConsoleAdapter console,
        NameFieldValidator nameValidator,
        DescriptionFieldValidator descriptionValidator,
        PriceFieldValidator priceValidator,
        SizeFieldValidator sizeValidator)
    {
        _pizzaService = pizzaService;
        _console = console;
        _nameValidator = nameValidator;
        _descriptionValidator = descriptionValidator;
        _priceValidator = priceValidator;
        _sizeValidator = sizeValidator;
    }

    public async Task RunAsync()
    {
        var shouldExit = false;

        while (!shouldExit)
        {
            _console.WriteLine("=== Pizzería HotPizza ===");
            _console.WriteLine("1. Registrar una pizza");
            _console.WriteLine("2. Consultar las pizzas registradas");
            _console.WriteLine("3. Salir");
            _console.Write("Seleccione una opción: ");

            var option = _console.ReadLine();

            _console.WriteLine();
            switch (option)
            {
                case "1":
                    await RegisterPizzaAsync();
                    break;
                case "2":
                    await DisplayPizzasAsync();
                    break;
                case "3":
                    shouldExit = true;
                    break;
                default:
                    _console.WriteLine("Opción no válida.");
                    break;
            }

            _console.WriteLine();
        }
    }

    private async Task RegisterPizzaAsync()
    {
        _console.WriteLine("=== Registro de Pizza ===");
        _console.WriteLine();

        var name = ReadRequiredText("Nombre: ", _nameValidator);
        var description = ReadRequiredText("Descripción: ", _descriptionValidator);
        var price = ReadPositiveDecimal();
        var size = ReadAllowedSize();

        var result = await _pizzaService.RegisterPizzaAsync(name, description, price, size);

        _console.WriteLine();
        if (result.IsSuccess)
        {
            _console.WriteLine($"Pizza registrada exitosamente con ID: {result.Data}");
        }
        else
        {
            _console.WriteLine("Error al registrar la pizza:");
            _console.WriteLine(result.ErrorMessage);
        }
    }

    private string ReadRequiredText(string prompt, IFieldValidator<string> validator)
    {
        while (true)
        {
            _console.Write(prompt);
            var value = _console.ReadLine();
            var result = validator.Validate(value);

            if (result.IsValid)
            {
                return value;
            }

            _console.WriteLine($"Error: {result.Errors[0]}");
        }
    }

    private decimal ReadPositiveDecimal()
    {
        while (true)
        {
            _console.Write("Precio: ");
            if (!decimal.TryParse(_console.ReadLine(), out var value))
            {
                _console.WriteLine("Error: El precio debe ser un número válido.");
                continue;
            }

            var result = _priceValidator.Validate(value);
            if (result.IsValid)
            {
                return value;
            }

            _console.WriteLine($"Error: {result.Errors[0]}");
        }
    }

    private int ReadAllowedSize()
    {
        while (true)
        {
            _console.Write("Tamaño (cm): ");
            if (!int.TryParse(_console.ReadLine(), out var value))
            {
                _console.WriteLine("Error: El tamaño debe ser un número entero válido.");
                continue;
            }

            var result = _sizeValidator.Validate(value);
            if (result.IsValid)
            {
                return value;
            }

            _console.WriteLine($"Error: {result.Errors[0]}");
        }
    }

    private async Task DisplayPizzasAsync()
    {
        var result = await _pizzaService.GetAllPizzasAsync();

        if (!result.IsSuccess)
        {
            _console.WriteLine("Error al consultar las pizzas:");
            _console.WriteLine(result.ErrorMessage);
            return;
        }

        if (result.Data is null || result.Data.Count == 0)
        {
            _console.WriteLine("El catálogo está vacío.");
            return;
        }

        _console.WriteLine("=== Catálogo de Pizzas ===");
        foreach (var pizza in result.Data)
        {
            _console.WriteLine($"Identificador: {pizza.Id}");
            _console.WriteLine($"Nombre: {pizza.Name}");
            _console.WriteLine($"Descripción: {pizza.Description}");
            _console.WriteLine($"Precio: {pizza.Price}");
            _console.WriteLine($"Tamaño: {pizza.Size}");
            _console.WriteLine();
        }
    }
}
