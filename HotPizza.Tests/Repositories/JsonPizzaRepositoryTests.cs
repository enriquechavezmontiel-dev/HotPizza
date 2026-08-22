using HotPizza.Entities;
using HotPizza.Repositories;
using Xunit;

namespace HotPizza.Tests.Repositories;

public class JsonPizzaRepositoryTests : IDisposable
{

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

    [Fact]
    public async Task AddAsync_MultiplePizzas_PersistsAll()
    {
        if (File.Exists("pizzas.json"))
            File.Delete("pizzas.json");

        var repository = new JsonPizzaRepository();

        var pizza1 = new Pizza
        {
            Name = "Pizza 1",
            Description = "Description 1",
            Price = 10.00m,
            Size = 25
        };

        var pizza2 = new Pizza
        {
            Name = "Pizza 2",
            Description = "Description 2",
            Price = 15.00m,
            Size = 30
        };

        await repository.AddAsync(pizza1);
        await repository.AddAsync(pizza2);

        var pizzas = await repository.GetAllAsync();

        Assert.Equal(2, pizzas.Count);
    }

    public void Dispose()
    {
        if (File.Exists("pizzas.json"))
            File.Delete("pizzas.json");
    }
}
