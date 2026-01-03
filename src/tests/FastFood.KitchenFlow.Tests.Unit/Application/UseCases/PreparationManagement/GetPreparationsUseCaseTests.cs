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

public class GetPreparationsUseCaseTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly GetPreparationsUseCase _useCase;

    public GetPreparationsUseCaseTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _useCase = new GetPreparationsUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task GetPreparations_WhenValidInput_ShouldReturnList()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        var preparations = new List<Preparation> { preparation };
        
        _mockRepository.Setup(r => r.GetPagedAsync(1, 10, null))
            .ReturnsAsync((preparations, 1));

        var inputModel = new GetPreparationsInputModel
        {
            PageNumber = 1,
            PageSize = 10,
            Status = null
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<GetPreparationsResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Lista de preparações retornada com sucesso.");
        result.Content.Should().NotBeNull();
        
        _mockRepository.Verify(r => r.GetPagedAsync(1, 10, null), Times.Once);
    }

    [Fact]
    public async Task GetPreparations_WhenPageNumberIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new GetPreparationsInputModel
        {
            PageNumber = 0,
            PageSize = 10,
            Status = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageNumber deve ser maior ou igual a 1");
    }

    [Fact]
    public async Task GetPreparations_WhenPageSizeIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new GetPreparationsInputModel
        {
            PageNumber = 1,
            PageSize = 0,
            Status = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageSize deve estar entre 1 e 100");
    }

    [Fact]
    public async Task GetPreparations_WhenPageSizeExceedsMax_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new GetPreparationsInputModel
        {
            PageNumber = 1,
            PageSize = 101,
            Status = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("PageSize deve estar entre 1 e 100");
    }

    [Fact]
    public async Task GetPreparations_WhenFilteredByStatus_ShouldReturnFilteredList()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        var preparations = new List<Preparation> { preparation };
        
        _mockRepository.Setup(r => r.GetPagedAsync(1, 10, (int)EnumPreparationStatus.Received))
            .ReturnsAsync((preparations, 1));

        var inputModel = new GetPreparationsInputModel
        {
            PageNumber = 1,
            PageSize = 10,
            Status = (int)EnumPreparationStatus.Received
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _mockRepository.Verify(r => r.GetPagedAsync(1, 10, (int)EnumPreparationStatus.Received), Times.Once);
    }
}
