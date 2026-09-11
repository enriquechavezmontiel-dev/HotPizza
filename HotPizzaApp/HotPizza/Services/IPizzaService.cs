using HotPizza.Entities;

namespace HotPizza.Services;

public interface IPizzaService
{
    Task<OperationResult<Guid>> RegisterPizzaAsync(string name, string description, decimal price, int size);
    Task<OperationResult<List<Pizza>>> GetAllPizzasAsync();
}
