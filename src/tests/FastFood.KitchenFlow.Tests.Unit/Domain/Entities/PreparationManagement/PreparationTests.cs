using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;

namespace FastFood.KitchenFlow.Tests.Unit.Domain.Entities.PreparationManagement;

public class PreparationTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreatePreparationWithReceivedStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";

        // Act
        var preparation = Preparation.Create(orderId, orderSnapshot);

        // Assert
        Assert.NotEqual(Guid.Empty, preparation.Id);
        Assert.Equal(orderId, preparation.OrderId);
        Assert.Equal(EnumPreparationStatus.Received, preparation.Status);
        Assert.NotNull(preparation.OrderSnapshot);
        Assert.Equal(orderSnapshot, preparation.OrderSnapshot);
        Assert.True(preparation.CreatedAt <= DateTime.UtcNow);
        Assert.True(preparation.CreatedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void Create_WithEmptyOrderId_ShouldThrowException()
    {
        // Arrange
        var emptyOrderId = Guid.Empty;
        var orderSnapshot = "{\"orderId\":\"\",\"items\":[]}";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Preparation.Create(emptyOrderId, orderSnapshot));
        Assert.Equal("orderId", exception.ParamName);
        Assert.Contains("não pode ser vazio", exception.Message);
    }

    [Fact]
    public void Create_WithNullOrderSnapshot_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        string? orderSnapshot = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Preparation.Create(orderId, orderSnapshot!));
        Assert.Equal("orderSnapshot", exception.ParamName);
        Assert.Contains("não pode ser nulo ou vazio", exception.Message);
    }

    [Fact]
    public void Create_WithEmptyOrderSnapshot_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Preparation.Create(orderId, orderSnapshot));
        Assert.Equal("orderSnapshot", exception.ParamName);
        Assert.Contains("não pode ser nulo ou vazio", exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceOrderSnapshot_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "   ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Preparation.Create(orderId, orderSnapshot));
        Assert.Equal("orderSnapshot", exception.ParamName);
        Assert.Contains("não pode ser nulo ou vazio", exception.Message);
    }

    [Fact]
    public void StartPreparation_WhenStatusIsReceived_ShouldChangeToInProgress()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);

        // Act
        preparation.StartPreparation();

        // Assert
        Assert.Equal(EnumPreparationStatus.InProgress, preparation.Status);
    }

    [Fact]
    public void StartPreparation_WhenStatusIsNotReceived_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => preparation.StartPreparation());
        Assert.Contains("Status atual: InProgress", exception.Message);
        Assert.Contains("Apenas preparações com status 'Received' podem ser iniciadas", exception.Message);
    }

    [Fact]
    public void FinishPreparation_WhenStatusIsInProgress_ShouldChangeToFinished()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress

        // Act
        preparation.FinishPreparation();

        // Assert
        Assert.Equal(EnumPreparationStatus.Finished, preparation.Status);
    }

    [Fact]
    public void FinishPreparation_WhenStatusIsNotInProgress_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        // Status é Received

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => preparation.FinishPreparation());
        Assert.Contains("Status atual: Received", exception.Message);
        Assert.Contains("Apenas preparações com status 'InProgress' podem ser finalizadas", exception.Message);
    }

    [Fact]
    public void FinishPreparation_WhenStatusIsFinished_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        preparation.StartPreparation(); // Muda para InProgress
        preparation.FinishPreparation(); // Muda para Finished

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => preparation.FinishPreparation());
        Assert.Contains("Status atual: Finished", exception.Message);
        Assert.Contains("Apenas preparações com status 'InProgress' podem ser finalizadas", exception.Message);
    }

    [Fact]
    public void Preparation_ShouldFollowValidStatusTransitionFlow()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = "{\"orderId\":\"" + orderId + "\",\"items\":[]}";
        var preparation = Preparation.Create(orderId, orderSnapshot);

        // Assert - Status inicial
        Assert.Equal(EnumPreparationStatus.Received, preparation.Status);

        // Act - Transição 1: Received -> InProgress
        preparation.StartPreparation();
        Assert.Equal(EnumPreparationStatus.InProgress, preparation.Status);

        // Act - Transição 2: InProgress -> Finished
        preparation.FinishPreparation();
        Assert.Equal(EnumPreparationStatus.Finished, preparation.Status);
    }
}
