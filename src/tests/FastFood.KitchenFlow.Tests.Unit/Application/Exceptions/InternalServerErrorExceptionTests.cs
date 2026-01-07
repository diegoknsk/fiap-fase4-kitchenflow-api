using FastFood.KitchenFlow.Application.Exceptions;
using FluentAssertions;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.Exceptions;

public class InternalServerErrorExceptionTests
{
    [Fact]
    public void Constructor_WhenMessageProvided_ShouldCreateExceptionWithMessage()
    {
        // Arrange
        var message = "Erro interno do servidor";

        // Act
        var exception = new InternalServerErrorException(message);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void Constructor_WhenMessageAndInnerExceptionProvided_ShouldCreateExceptionWithBoth()
    {
        // Arrange
        var message = "Erro interno do servidor";
        var innerException = new InvalidOperationException("Erro interno");

        // Act
        var exception = new InternalServerErrorException(message, innerException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void Constructor_WhenInnerExceptionIsNull_ShouldCreateExceptionWithNullInnerException()
    {
        // Arrange
        var message = "Erro interno do servidor";

        // Act
        var exception = new InternalServerErrorException(message, null!);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
    }
}
