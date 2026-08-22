namespace HotPizza.Validators;

public interface IValidator<T>
{
    ValidationResult Validate(T entity);
}
