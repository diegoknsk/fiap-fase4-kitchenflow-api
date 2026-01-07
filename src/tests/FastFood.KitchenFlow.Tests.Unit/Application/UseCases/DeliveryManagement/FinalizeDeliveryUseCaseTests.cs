using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.DeliveryManagement;

public class FinalizeDeliveryUseCaseTests
{
    private readonly Mock<IDeliveryRepository> _mockRepository;
    private readonly FinalizeDeliveryUseCase _useCase;

    public FinalizeDeliveryUseCaseTests()
    {
        _mockRepository = new Mock<IDeliveryRepository>();
        _useCase = new FinalizeDeliveryUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task FinalizeDelivery_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId, orderId);
        // Status já é ReadyForPickup por padrão
        var deliveryId = delivery.Id;
        
        _mockRepository.Setup(r => r.GetByIdAsync(deliveryId))
            .ReturnsAsync(delivery);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Delivery>()))
            .Returns(Task.CompletedTask);

        var inputModel = new FinalizeDeliveryInputModel
        {
            Id = deliveryId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<FinalizeDeliveryResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Entrega finalizada com sucesso.");
        result.Content.Should().NotBeNull();
        
        _mockRepository.Verify(r => r.GetByIdAsync(deliveryId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>()), Times.Once);
    }

    [Fact]
    public async Task FinalizeDelivery_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new FinalizeDeliveryInputModel
        {
            Id = Guid.Empty
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("Id não pode ser vazio");
    }

    [Fact]
    public async Task FinalizeDelivery_WhenDeliveryNotFound_ShouldThrowDeliveryNotFoundException()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.GetByIdAsync(deliveryId))
            .ReturnsAsync((Delivery?)null);

        var inputModel = new FinalizeDeliveryInputModel
        {
            Id = deliveryId
        };

        // Act & Assert
        await Assert.ThrowsAsync<DeliveryNotFoundException>(() => _useCase.ExecuteAsync(inputModel));
        _mockRepository.Verify(r => r.GetByIdAsync(deliveryId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>()), Times.Never);
    }

    [Fact]
    public async Task FinalizeDelivery_WhenStatusIsNotReadyForPickup_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId, orderId);
        delivery.FinalizeDelivery(); // Muda para Finalized
        
        _mockRepository.Setup(r => r.GetByIdAsync(delivery.Id))
            .ReturnsAsync(delivery);

        var inputModel = new FinalizeDeliveryInputModel
        {
            Id = delivery.Id
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().NotBeNullOrEmpty();
        exception.Message.ToLower().Should().Contain("finalizar");
        _mockRepository.Verify(r => r.GetByIdAsync(delivery.Id), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>()), Times.Never);
    }
}
