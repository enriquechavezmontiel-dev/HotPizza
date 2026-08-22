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
