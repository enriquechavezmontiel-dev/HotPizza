namespace HotPizza.Validators;

public class SizeFieldValidator : IFieldValidator<int>
{
    public ValidationResult Validate(int value)
    {
        if (value is 20 or 30 or 40)
        {
            return new ValidationResult { IsValid = true };
        }

        return new ValidationResult
        {
            IsValid = false,
            Errors = new List<string> { "El tamaño debe ser 20, 30 o 40 cm." }
        };
    }
}
