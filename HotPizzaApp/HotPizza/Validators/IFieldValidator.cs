namespace HotPizza.Validators;

public interface IFieldValidator<in T>
{
    ValidationResult Validate(T value);
}
