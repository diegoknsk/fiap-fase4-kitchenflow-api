using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.PreparationManagement;

public class CreatePreparationUseCaseTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly CreatePreparationUseCase _useCase;

    public CreatePreparationUseCaseTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _useCase = new CreatePreparationUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task CreatePreparation_WhenValidInput_ShouldReturnApiResponseSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        
        _mockRepository.Setup(r => r.GetByOrderIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Preparation?)null);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Preparation>()))
            .ReturnsAsync(Guid.NewGuid());

        var inputModel = new CreatePreparationInputModel
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<CreatePreparationResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Preparação criada com sucesso.");
        result.Content.Should().NotBeNull();
        
        // Verificar conteúdo (está em Dictionary<string, object> devido ao ToNamedContent)
        result.Content.Should().BeOfType<Dictionary<string, object>>();
        var contentDict = result.Content as Dictionary<string, object>;
        contentDict.Should().ContainKey("createPreparation");
        
        _mockRepository.Verify(r => r.GetByOrderIdAsync(orderId), Times.Once);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Preparation>()), Times.Once);
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var orderSnapshot = """{"orderId":"00000000-0000-0000-0000-000000000000","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"00000000-0000-0000-0000-000000000001","quantity":1,"price":50.00}],"paymentId":"00000000-0000-0000-0000-000000000002","paidAt":"2024-01-01T00:00:00Z"}""";
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = Guid.Empty,
            OrderSnapshot = orderSnapshot
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("OrderId não pode ser vazio");
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderSnapshotIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = Guid.NewGuid(),
            OrderSnapshot = null!
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("OrderSnapshot não pode ser nulo ou vazio");
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderSnapshotIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = Guid.NewGuid(),
            OrderSnapshot = string.Empty
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("OrderSnapshot não pode ser nulo ou vazio");
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderSnapshotIsInvalidJson_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = Guid.NewGuid(),
            OrderSnapshot = "{ invalid json }"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("OrderSnapshot contém JSON inválido");
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderSnapshotIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001"}"""; // Sem campos obrigatórios
        
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("OrderSnapshot contém dados inválidos");
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderIdDoesNotMatch_ShouldThrowArgumentException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var differentOrderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{differentOrderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
        exception.Message.Should().Contain("não corresponde");
    }

    [Fact]
    public async Task CreatePreparation_WhenPreparationAlreadyExists_ShouldThrowPreparationAlreadyExistsException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var existingPreparation = Preparation.Create(orderId, orderSnapshot);
        
        _mockRepository.Setup(r => r.GetByOrderIdAsync(orderId))
            .ReturnsAsync(existingPreparation);

        var inputModel = new CreatePreparationInputModel
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationAlreadyExistsException>(() => _useCase.ExecuteAsync(inputModel));
        _mockRepository.Verify(r => r.GetByOrderIdAsync(orderId), Times.Once);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Preparation>()), Times.Never);
    }
}
