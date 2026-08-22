using HotPizza.Entities;
using HotPizza.Validators;
using Xunit;

namespace HotPizza.Tests.Validators;

public class PizzaValidatorTests
{
    private readonly PizzaValidator _validator = new();

    [Fact]
    public void Validate_ValidPizza_ReturnsSuccess()
    {
        var pizza = new Pizza
        {
            Name = "Margherita",
            Description = "Classic pizza",
            Price = 10.99m,
            Size = 30
        };

        var result = _validator.Validate(pizza);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_EmptyName_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "",
            Description = "Test",
            Price = 10,
            Size = 30
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("nombre"));
    }

    [Fact]
    public void Validate_EmptyDescription_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "Test",
            Description = "",
            Price = 10,
            Size = 30
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("descripción"));
    }

    [Fact]
    public void Validate_NegativePrice_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "Test",
            Description = "Test",
            Price = -5,
            Size = 30
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("precio"));
    }

    [Fact]
    public void Validate_ZeroPrice_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "Test",
            Description = "Test",
            Price = 0,
            Size = 30
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("precio"));
    }

    [Fact]
    public void Validate_NegativeSize_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "Test",
            Description = "Test",
            Price = 10,
            Size = -10
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("tamaño"));
    }

    [Fact]
    public void Validate_ZeroSize_ReturnsError()
    {
        var pizza = new Pizza
        {
            Name = "Test",
            Description = "Test",
            Price = 10,
            Size = 0
        };

        var result = _validator.Validate(pizza);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("tamaño"));
    }
}
