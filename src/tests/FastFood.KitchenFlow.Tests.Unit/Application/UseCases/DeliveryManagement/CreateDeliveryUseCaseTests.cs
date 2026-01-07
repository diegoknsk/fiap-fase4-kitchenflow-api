using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.DeliveryManagement;

public class CreateDeliveryUseCaseTests
{
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly Mock<IPreparationRepository> _mockPreparationRepository;
    private readonly CreateDeliveryUseCase _useCase;

    public CreateDeliveryUseCaseTests()
    {
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _mockPreparationRepository = new Mock<IPreparationRepository>();
        _useCase = new CreateDeliveryUseCase(_mockDeliveryRepository.Object, _mockPreparationRepository.Object);
    }

    [Fact]
    public async Task CreateDelivery_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation();
        preparation.FinishPreparation(); // Status = Finished
        var preparationId = preparation.Id;
        
        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync((Delivery?)null);
        _mockDeliveryRepository.Setup(r => r.CreateAsync(It.IsAny<Delivery>()))
            .ReturnsAsync(Guid.NewGuid());

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId,
            OrderId = orderId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Entrega criada com sucesso.");
        result.Content.Should().NotBeNull();
        
        _mockPreparationRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.GetByPreparationIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Once);
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = Guid.Empty,
            OrderId = Guid.NewGuid()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PreparationId não pode ser vazio");
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        
        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync((Preparation?)null);

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId,
            OrderId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationNotFoundException>(() => _useCase.ExecuteAsync(inputModel));
        _mockPreparationRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationIsNotFinished_ShouldThrowPreparationNotFinishedException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Status = InProgress, não Finished
        var preparationId = preparation.Id;
        
        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId,
            OrderId = orderId
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationNotFinishedException>(() => _useCase.ExecuteAsync(inputModel));
        _mockPreparationRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
    }

    [Fact]
    public async Task CreateDelivery_WhenDeliveryAlreadyExists_ShouldThrowDeliveryAlreadyExistsException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation();
        preparation.FinishPreparation(); // Status = Finished
        var preparationId = preparation.Id;
        var existingDelivery = Delivery.Create(preparationId, orderId);
        
        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync(existingDelivery);

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId,
            OrderId = orderId
        };

        // Act & Assert
        await Assert.ThrowsAsync<DeliveryAlreadyExistsException>(() => _useCase.ExecuteAsync(inputModel));
        _mockPreparationRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.GetByPreparationIdAsync(preparationId), Times.Once);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
    }
}
