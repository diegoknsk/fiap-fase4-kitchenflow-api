namespace FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;

/// <summary>
/// OutputModel retornado após criação de uma entrega.
/// </summary>
public class CreateDeliveryOutputModel
{
    /// <summary>
    /// Identificador único da entrega criada.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador da preparação associada.
    /// </summary>
    public Guid PreparationId { get; set; }

    /// <summary>
    /// Identificador do pedido (opcional).
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Status da entrega (1 = ReadyForPickup, 2 = Finalized).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da entrega.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
