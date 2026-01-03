using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;

/// <summary>
/// Entidade de domínio que representa a entrega de um pedido.
/// </summary>
public class Delivery
{
    /// <summary>
    /// Identificador único da entrega.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador da preparação associada (FK obrigatória).
    /// </summary>
    public Guid PreparationId { get; private set; }

    /// <summary>
    /// Identificador do pedido (opcional, apenas para facilitar consulta).
    /// </summary>
    public Guid? OrderId { get; private set; }

    /// <summary>
    /// Status atual da entrega.
    /// </summary>
    public EnumDeliveryStatus Status { get; private set; }

    /// <summary>
    /// Data e hora de criação da entrega.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Data e hora de finalização da entrega (nullable).
    /// </summary>
    public DateTime? FinalizedAt { get; private set; }

    /// <summary>
    /// Construtor privado sem parâmetros para uso do EF Core.
    /// </summary>
    private Delivery()
    {
        // Construtor privado para EF Core
    }

    /// <summary>
    /// Factory method para criar uma nova entrega.
    /// </summary>
    /// <param name="preparationId">Identificador da preparação associada (obrigatório).</param>
    /// <param name="orderId">Identificador do pedido (opcional, apenas para facilitar consulta).</param>
    /// <returns>Nova instância de Delivery com status ReadyForPickup.</returns>
    /// <exception cref="ArgumentException">Lançada quando preparationId é vazio.</exception>
    public static Delivery Create(Guid preparationId, Guid? orderId = null)
    {
        if (preparationId == Guid.Empty)
        {
            throw new ArgumentException("PreparationId não pode ser vazio.", nameof(preparationId));
        }

        return new Delivery
        {
            Id = Guid.NewGuid(),
            PreparationId = preparationId,
            OrderId = orderId,
            Status = EnumDeliveryStatus.ReadyForPickup,
            CreatedAt = DateTime.UtcNow,
            FinalizedAt = null
        };
    }

    /// <summary>
    /// Finaliza a entrega, alterando o status de ReadyForPickup para Finalized.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite a transição para Finalized.</exception>
    public void FinalizeDelivery()
    {
        if (Status != EnumDeliveryStatus.ReadyForPickup)
        {
            throw new InvalidOperationException(
                $"Não é possível finalizar a entrega. Status atual: {Status}. Apenas entregas com status 'ReadyForPickup' podem ser finalizadas.");
        }

        Status = EnumDeliveryStatus.Finalized;
        FinalizedAt = DateTime.UtcNow;
    }
}
