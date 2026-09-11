namespace HotPizza.Validators;

public class RequiredTextFieldValidator : IFieldValidator<string>
{
    private readonly string _errorMessage;

    public RequiredTextFieldValidator(string errorMessage)
    {
        _errorMessage = errorMessage;
    }

    public ValidationResult Validate(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return new ValidationResult { IsValid = true };
        }

        return new ValidationResult
        {
            IsValid = false,
            Errors = new List<string> { _errorMessage }
        };
    }
}
