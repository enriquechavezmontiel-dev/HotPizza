using HotPizza.Validators;
using Xunit;

namespace HotPizza.Tests.Validators;

public class FieldValidatorTests
{
    [Fact]
    public void NameFieldValidator_WhitespaceValue_ReturnsError()
    {
        var validator = new NameFieldValidator();

        var result = validator.Validate("   ");

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("nombre"));
    }

    [Fact]
    public void DescriptionFieldValidator_EmptyValue_ReturnsError()
    {
        var validator = new DescriptionFieldValidator();

        var result = validator.Validate("");

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("descripción"));
    }

    [Fact]
    public void DescriptionFieldValidator_WhitespaceValue_ReturnsError()
    {
        var validator = new DescriptionFieldValidator();

        var result = validator.Validate("   ");

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("descripción"));
    }

    [Fact]
    public void PriceFieldValidator_NonPositiveValue_ReturnsError()
    {
        var validator = new PriceFieldValidator();

        var result = validator.Validate(0);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("precio"));
    }

    [Fact]
    public void PriceFieldValidator_NegativeValue_ReturnsError()
    {
        var validator = new PriceFieldValidator();

        var result = validator.Validate(-1);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("precio"));
    }

    [Fact]
    public void SizeFieldValidator_UnsupportedValue_ReturnsError()
    {
        var validator = new SizeFieldValidator();

        var result = validator.Validate(25);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("tamaño"));
    }
}
