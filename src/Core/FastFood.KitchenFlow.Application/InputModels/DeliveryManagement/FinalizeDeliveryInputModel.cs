namespace FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;

/// <summary>
/// InputModel para finalização de uma entrega.
/// </summary>
public class FinalizeDeliveryInputModel
{
    /// <summary>
    /// Identificador da entrega a ser finalizada.
    /// </summary>
    public Guid Id { get; set; }
}
