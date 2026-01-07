namespace FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;

/// <summary>
/// InputModel para criação de uma nova entrega.
/// </summary>
public class CreateDeliveryInputModel
{
    /// <summary>
    /// Identificador da preparação associada (obrigatório).
    /// </summary>
    public Guid PreparationId { get; set; }

    /// <summary>
    /// Identificador do pedido (opcional, apenas para facilitar consulta).
    /// </summary>
    public Guid? OrderId { get; set; }
}
