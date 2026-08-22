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
