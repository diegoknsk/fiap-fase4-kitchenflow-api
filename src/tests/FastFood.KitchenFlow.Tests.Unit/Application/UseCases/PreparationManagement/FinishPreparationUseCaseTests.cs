using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.PreparationManagement;

public class FinishPreparationUseCaseTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly FinishPreparationUseCase _useCase;

    public FinishPreparationUseCaseTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _useCase = new FinishPreparationUseCase(_mockRepository.Object, _mockDeliveryRepository.Object);
    }

    [Fact]
    public async Task FinishPreparation_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress
        var preparationId = preparation.Id;
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask);
        
        // Mock: não existe delivery ainda
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync((Delivery?)null);
        _mockDeliveryRepository.Setup(r => r.CreateAsync(It.IsAny<Delivery>()))
            .ReturnsAsync(Guid.NewGuid());

        var inputModel = new FinishPreparationInputModel
        {
            Id = preparationId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<FinishPreparationResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Preparação finalizada com sucesso.");
        result.Content.Should().NotBeNull();
        result.Content!.DeliveryId.Should().NotBeNull();
        result.Content.DeliveryId.Should().NotBe(Guid.Empty);
        
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Once);
        _mockDeliveryRepository.Verify(r => r.GetByPreparationIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.Is<Delivery>(d => 
            d.PreparationId == preparationId && 
            d.OrderId == orderId &&
            d.Status == EnumDeliveryStatus.ReadyForPickup)), Times.Once);
    }

    [Fact]
    public async Task FinishPreparation_WhenIdIsEmpty_ShouldThrowValidationException()
    {
        // Arrange
        var inputModel = new FinishPreparationInputModel
        {
            Id = Guid.Empty
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("Id não pode ser vazio");
    }

    [Fact]
    public async Task FinishPreparation_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync((Preparation?)null);

        var inputModel = new FinishPreparationInputModel
        {
            Id = preparationId
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationNotFoundException>(() => _useCase.ExecuteAsync(inputModel));
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Never);
    }

    [Fact]
    public async Task FinishPreparation_WhenStatusIsNotInProgress_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        // Status é Received, não InProgress
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparation.Id))
            .ReturnsAsync(preparation);

        var inputModel = new FinishPreparationInputModel
        {
            Id = preparation.Id
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().NotBeNullOrEmpty();
        exception.Message.ToLower().Should().Contain("finalizar");
        _mockRepository.Verify(r => r.GetByIdAsync(preparation.Id), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Never);
        _mockDeliveryRepository.Verify(r => r.GetByPreparationIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
    }

    [Fact]
    public async Task FinishPreparation_WhenDeliveryAlreadyExists_ShouldReturnExistingDeliveryId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress
        var preparationId = preparation.Id;
        
        // Delivery já existe (idempotência)
        var existingDelivery = Delivery.Create(preparationId, orderId);
        var existingDeliveryId = existingDelivery.Id;
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask);
        
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync(existingDelivery);

        var inputModel = new FinishPreparationInputModel
        {
            Id = preparationId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content!.DeliveryId.Should().Be(existingDeliveryId);
        
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Preparation>()), Times.Once);
        _mockDeliveryRepository.Verify(r => r.GetByPreparationIdAsync(preparationId), Times.Once);
        // Não deve criar novo delivery (idempotência)
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
    }

    [Fact]
    public async Task FinishPreparation_WhenCreatingDelivery_ShouldCreateWithCorrectProperties()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress
        var preparationId = preparation.Id;
        
        _mockRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask);
        
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync((Delivery?)null);
        
        Delivery? capturedDelivery = null;
        _mockDeliveryRepository.Setup(r => r.CreateAsync(It.IsAny<Delivery>()))
            .Callback<Delivery>(d => capturedDelivery = d)
            .ReturnsAsync(Guid.NewGuid());

        var inputModel = new FinishPreparationInputModel
        {
            Id = preparationId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        capturedDelivery.Should().NotBeNull();
        capturedDelivery!.PreparationId.Should().Be(preparationId);
        capturedDelivery.OrderId.Should().Be(orderId);
        capturedDelivery.Status.Should().Be(EnumDeliveryStatus.ReadyForPickup);
        capturedDelivery.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
