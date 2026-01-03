namespace FastFood.KitchenFlow.Domain.Common.Enums;

/// <summary>
/// Enum que representa os status possíveis de uma entrega de pedido.
/// </summary>
public enum EnumDeliveryStatus
{
    /// <summary>
    /// Pronto para retirada.
    /// </summary>
    ReadyForPickup = 1,

    /// <summary>
    /// Entrega finalizada.
    /// </summary>
    Finalized = 2
}
