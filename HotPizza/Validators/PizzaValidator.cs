using HotPizza.Entities;

namespace HotPizza.Validators;

public class PizzaValidator : IValidator<Pizza>
{
    private readonly IFieldValidator<string> _nameValidator;
    private readonly IFieldValidator<string> _descriptionValidator;
    private readonly IFieldValidator<decimal> _priceValidator;
    private readonly IFieldValidator<int> _sizeValidator;

    public PizzaValidator(
        NameFieldValidator nameValidator,
        DescriptionFieldValidator descriptionValidator,
        PriceFieldValidator priceValidator,
        SizeFieldValidator sizeValidator)
    {
        _nameValidator = nameValidator;
        _descriptionValidator = descriptionValidator;
        _priceValidator = priceValidator;
        _sizeValidator = sizeValidator;
    }

    public ValidationResult Validate(Pizza pizza)
    {
        var result = new ValidationResult { IsValid = true };

        var fieldResults = new[]
        {
            _nameValidator.Validate(pizza.Name),
            _descriptionValidator.Validate(pizza.Description),
            _priceValidator.Validate(pizza.Price),
            _sizeValidator.Validate(pizza.Size)
        };

        foreach (var fieldResult in fieldResults.Where(fieldResult => !fieldResult.IsValid))
        {
            result.IsValid = false;
            result.Errors.AddRange(fieldResult.Errors);
        }

        return result;
    }
}
