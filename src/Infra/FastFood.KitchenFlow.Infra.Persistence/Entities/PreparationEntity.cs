namespace FastFood.KitchenFlow.Infra.Persistence.Entities;

/// <summary>
/// Entidade de persistência que representa a tabela Preparations no banco de dados.
/// Esta entidade é separada da entidade de domínio Preparation.
/// </summary>
public class PreparationEntity
{
    /// <summary>
    /// Identificador único da preparação (chave primária).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Status atual da preparação (mapeia para EnumPreparationStatus).
    /// 0 = Received, 1 = InProgress, 2 = Finished
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da preparação.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Snapshot JSON imutável do pedido no momento do pagamento.
    /// Será mapeado para o tipo jsonb no PostgreSQL.
    /// </summary>
    public string OrderSnapshot { get; set; } = string.Empty;
}
