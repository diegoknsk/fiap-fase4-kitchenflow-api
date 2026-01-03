using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.PreparationManagement;

public class StartPreparationUseCaseTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly StartPreparationUseCase _useCase;

    public StartPreparationUseCaseTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _useCase = new StartPreparationUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task StartPreparation_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        var preparationId = preparation.Id;
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask);

        var inputModel = new StartPreparationInputModel
        {
            Id = preparationId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<StartPreparationResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Preparação iniciada com sucesso.");
        result.Content.Should().NotBeNull();
        
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Once);
    }

    [Fact]
    public async Task StartPreparation_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new StartPreparationInputModel
        {
            Id = Guid.Empty
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("Id não pode ser vazio");
    }

    [Fact]
    public async Task StartPreparation_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync((Preparation?)null);

        var inputModel = new StartPreparationInputModel
        {
            Id = preparationId
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationNotFoundException>(() => _useCase.ExecuteAsync(inputModel));
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Never);
    }

    [Fact]
    public async Task StartPreparation_WhenStatusIsNotReceived_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparation.Id))
            .ReturnsAsync(preparation);

        var inputModel = new StartPreparationInputModel
        {
            Id = preparation.Id
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().NotBeNullOrEmpty();
        exception.Message.ToLower().Should().Contain("iniciar");
        _mockRepository.Verify(r => r.GetByIdAsync(preparation.Id), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Never);
    }
}
