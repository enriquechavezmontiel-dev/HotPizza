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
