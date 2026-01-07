using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.DeliveryManagement;

public class GetReadyDeliveriesUseCaseTests
{
    private readonly Mock<IDeliveryRepository> _mockRepository;
    private readonly GetReadyDeliveriesUseCase _useCase;

    public GetReadyDeliveriesUseCaseTests()
    {
        _mockRepository = new Mock<IDeliveryRepository>();
        _useCase = new GetReadyDeliveriesUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenValidInput_ShouldReturnList()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId, orderId);
        var deliveries = new List<Delivery> { delivery };
        
        _mockRepository.Setup(r => r.GetReadyDeliveriesAsync(1, 10))
            .ReturnsAsync((deliveries, 1));

        var inputModel = new GetReadyDeliveriesInputModel
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<GetReadyDeliveriesResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Lista de entregas prontas para retirada retornada com sucesso.");
        result.Content.Should().NotBeNull();
        
        _mockRepository.Verify(r => r.GetReadyDeliveriesAsync(1, 10), Times.Once);
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenPageNumberIsLessThanOne_ShouldThrowValidationException()
    {
        // Arrange
        var inputModel = new GetReadyDeliveriesInputModel
        {
            PageNumber = 0,
            PageSize = 10
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageNumber deve ser maior ou igual a 1");
        _mockRepository.Verify(r => r.GetReadyDeliveriesAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenPageSizeIsLessThanOne_ShouldThrowValidationException()
    {
        // Arrange
        var inputModel = new GetReadyDeliveriesInputModel
        {
            PageNumber = 1,
            PageSize = 0
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageSize deve ser maior ou igual a 1");
        _mockRepository.Verify(r => r.GetReadyDeliveriesAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenPageSizeExceedsMax_ShouldThrowValidationException()
    {
        // Arrange
        var inputModel = new GetReadyDeliveriesInputModel
        {
            PageNumber = 1,
            PageSize = 101
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageSize não pode ser maior que 100");
        _mockRepository.Verify(r => r.GetReadyDeliveriesAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}
