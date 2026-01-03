namespace FastFood.KitchenFlow.Infra.Persistence.Entities;

/// <summary>
/// Entidade de persistência que representa a tabela Deliveries no banco de dados.
/// Esta entidade é separada da entidade de domínio Delivery.
/// </summary>
public class DeliveryEntity
{
    /// <summary>
    /// Identificador único da entrega (chave primária).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador da preparação associada (FK obrigatória para Preparations.Id).
    /// </summary>
    public Guid PreparationId { get; set; }

    /// <summary>
    /// Identificador do pedido (opcional, apenas para facilitar consulta).
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Status atual da entrega (mapeia para EnumDeliveryStatus).
    /// 1 = ReadyForPickup, 2 = Finalized
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da entrega.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data e hora de finalização da entrega (nullable).
    /// </summary>
    public DateTime? FinalizedAt { get; set; }
}
