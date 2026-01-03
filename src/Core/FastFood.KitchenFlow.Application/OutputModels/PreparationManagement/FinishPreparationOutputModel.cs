namespace FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;

/// <summary>
/// OutputModel retornado após finalizar uma preparação.
/// </summary>
public class FinishPreparationOutputModel
{
    /// <summary>
    /// Identificador único da preparação.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Status da preparação (0 = Received, 1 = InProgress, 2 = Finished).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da preparação.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
