namespace FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;

/// <summary>
/// OutputModel retornado após finalização de uma entrega.
/// </summary>
public class FinalizeDeliveryOutputModel
{
    /// <summary>
    /// Identificador único da entrega finalizada.
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

    /// <summary>
    /// Data e hora de finalização da entrega.
    /// </summary>
    public DateTime? FinalizedAt { get; set; }
}
