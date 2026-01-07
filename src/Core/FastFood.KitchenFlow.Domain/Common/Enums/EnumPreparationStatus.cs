namespace FastFood.KitchenFlow.Domain.Common.Enums;

/// <summary>
/// Enum que representa os status possíveis de uma preparação de pedido.
/// </summary>
public enum EnumPreparationStatus
{
    /// <summary>
    /// Pedido recebido após pagamento confirmado.
    /// </summary>
    Received = 0,

    /// <summary>
    /// Pedido em preparação na cozinha.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Preparação finalizada, pronto para entrega.
    /// </summary>
    Finished = 2
}
