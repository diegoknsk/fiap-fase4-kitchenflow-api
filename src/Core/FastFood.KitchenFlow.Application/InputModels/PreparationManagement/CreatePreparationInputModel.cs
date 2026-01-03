namespace FastFood.KitchenFlow.Application.InputModels.PreparationManagement;

/// <summary>
/// InputModel para criação de uma nova preparação.
/// </summary>
public class CreatePreparationInputModel
{
    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Snapshot JSON do pedido no momento do pagamento.
    /// </summary>
    public string OrderSnapshot { get; set; } = string.Empty;
}
