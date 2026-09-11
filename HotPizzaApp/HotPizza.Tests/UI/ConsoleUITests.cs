using HotPizza.Entities;
using HotPizza.Services;
using HotPizza.UI;
using HotPizza.Validators;
using Moq;
using Xunit;

namespace HotPizza.Tests.UI;

public class ConsoleUITests
{
    [Fact]
    public async Task RunAsync_InvalidFieldsThenValidFields_RetriesAndRegistersOnce()
    {
        var console = new TestConsoleAdapter(
            "1", "", "Margarita", "   ", "Tomate y mozzarella", "not-a-price", "0", "12.50", "25", "30", "3");
        var service = new Mock<IPizzaService>();

        service.Setup(pizzaService => pizzaService.RegisterPizzaAsync(
                "Margarita", "Tomate y mozzarella", 12.50m, 30))
            .ReturnsAsync(new OperationResult<Guid> { IsSuccess = true, Data = Guid.NewGuid() });

        var ui = new ConsoleUI(
            service.Object,
            console,
            new NameFieldValidator(),
            new DescriptionFieldValidator(),
            new PriceFieldValidator(),
            new SizeFieldValidator());

        await ui.RunAsync();

        service.Verify(pizzaService => pizzaService.RegisterPizzaAsync(
            "Margarita", "Tomate y mozzarella", 12.50m, 30), Times.Once);
        Assert.Contains(console.Messages, message => message.Contains("El nombre es obligatorio."));
        Assert.Contains(console.Messages, message => message.Contains("La descripción es obligatoria."));
        Assert.Contains(console.Messages, message => message.Contains("El precio debe ser un número válido."));
        Assert.Contains(console.Messages, message => message.Contains("El precio debe ser mayor a 0."));
        Assert.Contains(console.Messages, message => message.Contains("El tamaño debe ser 20, 30 o 40 cm."));
    }

    [Fact]
    public async Task RunAsync_EmptyCatalog_DisplaysEmptyCatalogMessage()
    {
        var console = new TestConsoleAdapter("2", "3");
        var service = new Mock<IPizzaService>();
        service.Setup(pizzaService => pizzaService.GetAllPizzasAsync())
            .ReturnsAsync(new OperationResult<List<Pizza>>
            {
                IsSuccess = true,
                Data = new List<Pizza>()
            });

        var ui = CreateConsoleUI(service.Object, console);

        await ui.RunAsync();

        Assert.Contains("El catálogo está vacío.", console.Messages);
        service.Verify(pizzaService => pizzaService.GetAllPizzasAsync(), Times.Once);
    }

    [Fact]
    public async Task RunAsync_CatalogWithPizza_DisplaysAllPizzaFields()
    {
        var pizza = new Pizza
        {
            Id = Guid.NewGuid(),
            Name = "Margarita",
            Description = "Tomate y mozzarella",
            Price = 12.50m,
            Size = 30
        };
        var console = new TestConsoleAdapter("2", "3");
        var service = new Mock<IPizzaService>();
        service.Setup(pizzaService => pizzaService.GetAllPizzasAsync())
            .ReturnsAsync(new OperationResult<List<Pizza>>
            {
                IsSuccess = true,
                Data = new List<Pizza> { pizza }
            });

        var ui = CreateConsoleUI(service.Object, console);

        await ui.RunAsync();

        Assert.Contains(console.Messages, message => message.Contains(pizza.Id.ToString()));
        Assert.Contains(console.Messages, message => message.Contains(pizza.Name));
        Assert.Contains(console.Messages, message => message.Contains(pizza.Description));
        Assert.Contains(console.Messages, message => message.Contains(pizza.Price.ToString()));
        Assert.Contains(console.Messages, message => message.Contains(pizza.Size.ToString()));
    }

    [Fact]
    public async Task RunAsync_CatalogQueryFails_DisplaysErrorMessage()
    {
        var console = new TestConsoleAdapter("2", "3");
        var service = new Mock<IPizzaService>();
        service.Setup(pizzaService => pizzaService.GetAllPizzasAsync())
            .ReturnsAsync(new OperationResult<List<Pizza>>
            {
                IsSuccess = false,
                ErrorMessage = "Error de consulta"
            });

        var ui = CreateConsoleUI(service.Object, console);

        await ui.RunAsync();

        Assert.Contains("Error al consultar las pizzas:", console.Messages);
        Assert.Contains("Error de consulta", console.Messages);
    }

    [Fact]
    public async Task RunAsync_RegistrationFails_DisplaysErrorMessage()
    {
        var console = new TestConsoleAdapter("1", "Margarita", "Tomate y mozzarella", "12.50", "30", "3");
        var service = new Mock<IPizzaService>();
        service.Setup(pizzaService => pizzaService.RegisterPizzaAsync(
                "Margarita", "Tomate y mozzarella", 12.50m, 30))
            .ReturnsAsync(new OperationResult<Guid>
            {
                IsSuccess = false,
                ErrorMessage = "Error de registro"
            });

        var ui = CreateConsoleUI(service.Object, console);

        await ui.RunAsync();

        Assert.Contains("Error al registrar la pizza:", console.Messages);
        Assert.Contains("Error de registro", console.Messages);
    }

    private static ConsoleUI CreateConsoleUI(IPizzaService service, IConsoleAdapter console)
    {
        return new ConsoleUI(
            service,
            console,
            new NameFieldValidator(),
            new DescriptionFieldValidator(),
            new PriceFieldValidator(),
            new SizeFieldValidator());
    }

    private sealed class TestConsoleAdapter : IConsoleAdapter
    {
        private readonly Queue<string> _inputs;

        public TestConsoleAdapter(params string[] inputs)
        {
            _inputs = new Queue<string>(inputs);
        }

        public List<string> Messages { get; } = new();

        public string ReadLine()
        {
            return _inputs.Dequeue();
        }

        public void Write(string message)
        {
            Messages.Add(message);
        }

        public void WriteLine(string message = "")
        {
            Messages.Add(message);
        }
    }
}
