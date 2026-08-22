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
