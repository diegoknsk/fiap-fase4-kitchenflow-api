using FastFood.KitchenFlow.Application.Exceptions;
using FluentAssertions;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WhenMessageProvided_ShouldCreateExceptionWithMessage()
    {
        // Arrange
        var message = "Dados inválidos";

        // Act
        var exception = new ValidationException(message);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.Errors.Should().BeNull();
    }

    [Fact]
    public void Constructor_WhenMessageAndErrorsProvided_ShouldCreateExceptionWithBoth()
    {
        // Arrange
        var message = "Dados inválidos";
        var errors = new Dictionary<string, string[]>
        {
            { "OrderId", new[] { "OrderId não pode ser vazio" } },
            { "OrderSnapshot", new[] { "OrderSnapshot é obrigatório" } }
        };

        // Act
        var exception = new ValidationException(message, errors);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().ContainKey("OrderId");
        exception.Errors.Should().ContainKey("OrderSnapshot");
        exception.Errors!["OrderId"].Should().Contain("OrderId não pode ser vazio");
    }

    [Fact]
    public void Constructor_WhenErrorsIsNull_ShouldCreateExceptionWithNullErrors()
    {
        // Arrange
        var message = "Dados inválidos";

        // Act
        var exception = new ValidationException(message, null);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.Errors.Should().BeNull();
    }

    [Fact]
    public void Constructor_WhenErrorsIsEmpty_ShouldCreateExceptionWithEmptyErrors()
    {
        // Arrange
        var message = "Dados inválidos";
        var errors = new Dictionary<string, string[]>();

        // Act
        var exception = new ValidationException(message, errors);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().BeEmpty();
    }
}
