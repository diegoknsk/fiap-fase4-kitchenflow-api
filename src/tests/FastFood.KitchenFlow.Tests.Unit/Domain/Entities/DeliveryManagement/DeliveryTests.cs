using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;

namespace FastFood.KitchenFlow.Tests.Unit.Domain.Entities.DeliveryManagement;

public class DeliveryTests
{
    [Fact]
    public void Create_WithValidPreparationId_ShouldCreateDeliveryWithReadyForPickupStatus()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        // Act
        var delivery = Delivery.Create(preparationId);

        // Assert
        Assert.NotEqual(Guid.Empty, delivery.Id);
        Assert.Equal(preparationId, delivery.PreparationId);
        Assert.Equal(EnumDeliveryStatus.ReadyForPickup, delivery.Status);
        Assert.Null(delivery.OrderId);
        Assert.Null(delivery.FinalizedAt);
        Assert.True(delivery.CreatedAt <= DateTime.UtcNow);
        Assert.True(delivery.CreatedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void Create_WithEmptyPreparationId_ShouldThrowException()
    {
        // Arrange
        var emptyPreparationId = Guid.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Delivery.Create(emptyPreparationId));
        Assert.Equal("preparationId", exception.ParamName);
        Assert.Contains("não pode ser vazio", exception.Message);
    }

    [Fact]
    public void Create_WithOptionalOrderId_ShouldCreateSuccessfully()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        // Act
        var delivery = Delivery.Create(preparationId, orderId);

        // Assert
        Assert.Equal(preparationId, delivery.PreparationId);
        Assert.Equal(orderId, delivery.OrderId);
        Assert.Equal(EnumDeliveryStatus.ReadyForPickup, delivery.Status);
    }

    [Fact]
    public void Create_WithNullOrderId_ShouldCreateSuccessfully()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        // Act
        var delivery = Delivery.Create(preparationId, null);

        // Assert
        Assert.Equal(preparationId, delivery.PreparationId);
        Assert.Null(delivery.OrderId);
        Assert.Equal(EnumDeliveryStatus.ReadyForPickup, delivery.Status);
    }

    [Fact]
    public void FinalizeDelivery_WhenStatusIsReadyForPickup_ShouldChangeToFinalized()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId);

        // Act
        delivery.FinalizeDelivery();

        // Assert
        Assert.Equal(EnumDeliveryStatus.Finalized, delivery.Status);
        Assert.NotNull(delivery.FinalizedAt);
        Assert.True(delivery.FinalizedAt <= DateTime.UtcNow);
        Assert.True(delivery.FinalizedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void FinalizeDelivery_WhenStatusIsNotReadyForPickup_ShouldThrowException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId);
        delivery.FinalizeDelivery(); // Muda para Finalized

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => delivery.FinalizeDelivery());
        Assert.Contains("Status atual: Finalized", exception.Message);
        Assert.Contains("Apenas entregas com status 'ReadyForPickup' podem ser finalizadas", exception.Message);
    }

    [Fact]
    public void FinalizeDelivery_ShouldSetFinalizedAt()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId);
        var beforeFinalization = DateTime.UtcNow;

        // Act
        delivery.FinalizeDelivery();

        // Assert
        Assert.NotNull(delivery.FinalizedAt);
        Assert.True(delivery.FinalizedAt >= beforeFinalization);
        Assert.True(delivery.FinalizedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Delivery_ShouldFollowValidStatusTransitionFlow()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var delivery = Delivery.Create(preparationId);

        // Assert - Status inicial
        Assert.Equal(EnumDeliveryStatus.ReadyForPickup, delivery.Status);
        Assert.Null(delivery.FinalizedAt);

        // Act - Transição: ReadyForPickup -> Finalized
        delivery.FinalizeDelivery();
        Assert.Equal(EnumDeliveryStatus.Finalized, delivery.Status);
        Assert.NotNull(delivery.FinalizedAt);
    }
}
