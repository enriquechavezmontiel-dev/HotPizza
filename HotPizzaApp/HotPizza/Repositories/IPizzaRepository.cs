using HotPizza.Entities;

namespace HotPizza.Repositories;

public interface IPizzaRepository
{
    Task<Guid> AddAsync(Pizza pizza);
    Task<List<Pizza>> GetAllAsync();
}
