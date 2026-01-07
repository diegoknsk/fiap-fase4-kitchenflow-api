namespace FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;

/// <summary>
/// OutputModel que representa um item de preparação na listagem.
/// </summary>
public class PreparationItemOutputModel
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

    /// <summary>
    /// Snapshot JSON do pedido (incluído para exibição no display da cozinha).
    /// </summary>
    public string OrderSnapshot { get; set; } = string.Empty;
}
