namespace HotPizza.Validators;

public class PriceFieldValidator : IFieldValidator<decimal>
{
    public ValidationResult Validate(decimal value)
    {
        if (value > 0)
        {
            return new ValidationResult { IsValid = true };
        }

        return new ValidationResult
        {
            IsValid = false,
            Errors = new List<string> { "El precio debe ser mayor a 0." }
        };
    }
}
