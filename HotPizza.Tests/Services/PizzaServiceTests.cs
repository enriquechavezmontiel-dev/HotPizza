using HotPizza.Entities;
using HotPizza.Repositories;
using HotPizza.Services;
using HotPizza.Validators;
using Moq;
using Xunit;

namespace HotPizza.Tests.Services;

public class PizzaServiceTests
{
    [Fact]
    public async Task RegisterPizzaAsync_ValidData_ReturnsSuccessWithId()
    {
        var mockRepo = new Mock<IPizzaRepository>();
        var mockValidator = new Mock<IValidator<Pizza>>();
        var expectedId = Guid.NewGuid();

        mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
            .Returns(new ValidationResult { IsValid = true });

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Pizza>()))
            .ReturnsAsync(expectedId);

        var service = new PizzaService(mockRepo.Object, mockValidator.Object);

        var result = await service.RegisterPizzaAsync("Test", "Description", 10.99m, 30);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task RegisterPizzaAsync_InvalidData_ReturnsFailure()
    {
        var mockRepo = new Mock<IPizzaRepository>();
        var mockValidator = new Mock<IValidator<Pizza>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
            .Returns(new ValidationResult 
            { 
                IsValid = false, 
                Errors = new List<string> { "Error de validación" } 
            });

        var service = new PizzaService(mockRepo.Object, mockValidator.Object);

        var result = await service.RegisterPizzaAsync("", "", -1, -1);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ErrorMessage);
        Assert.Contains("Error de validación", result.ErrorMessage);
    }

    [Fact]
    public async Task RegisterPizzaAsync_RepositoryThrowsException_ReturnsFailure()
    {
        var mockRepo = new Mock<IPizzaRepository>();
        var mockValidator = new Mock<IValidator<Pizza>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
            .Returns(new ValidationResult { IsValid = true });

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Pizza>()))
            .ThrowsAsync(new Exception("Database error"));

        var service = new PizzaService(mockRepo.Object, mockValidator.Object);

        var result = await service.RegisterPizzaAsync("Test", "Description", 10.99m, 30);

        Assert.False(result.IsSuccess);
        Assert.Contains("Error al registrar", result.ErrorMessage);
    }

    [Fact]
    public async Task RegisterPizzaAsync_MultipleValidationErrors_ReturnsAllErrors()
    {
        var mockRepo = new Mock<IPizzaRepository>();
        var mockValidator = new Mock<IValidator<Pizza>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<Pizza>()))
            .Returns(new ValidationResult 
            { 
                IsValid = false, 
                Errors = new List<string> 
                { 
                    "El nombre es obligatorio.",
                    "El precio debe ser mayor a 0."
                } 
            });

        var service = new PizzaService(mockRepo.Object, mockValidator.Object);

        var result = await service.RegisterPizzaAsync("", "", 0, 30);

        Assert.False(result.IsSuccess);
        Assert.Contains("nombre", result.ErrorMessage);
        Assert.Contains("precio", result.ErrorMessage);
    }
}
