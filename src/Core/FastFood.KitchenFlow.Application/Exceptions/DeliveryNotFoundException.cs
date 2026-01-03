namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma entrega não é encontrada.
/// </summary>
public class DeliveryNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Identificador da entrega não encontrada.
    /// </summary>
    public Guid DeliveryId { get; }

    /// <summary>
    /// Construtor que recebe o DeliveryId não encontrado.
    /// </summary>
    /// <param name="deliveryId">Identificador da entrega.</param>
    public DeliveryNotFoundException(Guid deliveryId)
        : base($"Entrega {deliveryId} não encontrada.")
    {
        DeliveryId = deliveryId;
    }
}
