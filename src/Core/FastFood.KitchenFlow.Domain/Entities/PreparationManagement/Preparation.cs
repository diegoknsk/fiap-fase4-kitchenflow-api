using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Domain.Entities.PreparationManagement;

/// <summary>
/// Entidade de domínio que representa a preparação de um pedido na cozinha.
/// </summary>
public class Preparation
{
    /// <summary>
    /// Identificador único da preparação.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid OrderId { get; private set; }

    /// <summary>
    /// Status atual da preparação.
    /// </summary>
    public EnumPreparationStatus Status { get; private set; }

    /// <summary>
    /// Data e hora de criação da preparação.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Snapshot JSON imutável do pedido no momento do pagamento.
    /// Armazenado como string no domínio (será mapeado para jsonb no banco).
    /// </summary>
    public string OrderSnapshot { get; private set; }

    /// <summary>
    /// Construtor privado sem parâmetros para uso do EF Core.
    /// </summary>
    private Preparation()
    {
        // Construtor privado para EF Core
        OrderSnapshot = string.Empty;
    }

    /// <summary>
    /// Factory method para criar uma nova preparação.
    /// </summary>
    /// <param name="orderId">Identificador do pedido.</param>
    /// <param name="orderSnapshot">Snapshot JSON do pedido no momento do pagamento.</param>
    /// <returns>Nova instância de Preparation com status Received.</returns>
    /// <exception cref="ArgumentException">Lançada quando orderId é vazio ou orderSnapshot é nulo/vazio.</exception>
    public static Preparation Create(Guid orderId, string orderSnapshot)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("OrderId não pode ser vazio.", nameof(orderId));
        }

        if (string.IsNullOrWhiteSpace(orderSnapshot))
        {
            throw new ArgumentException("OrderSnapshot não pode ser nulo ou vazio.", nameof(orderSnapshot));
        }

        return new Preparation
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Status = EnumPreparationStatus.Received,
            CreatedAt = DateTime.UtcNow,
            OrderSnapshot = orderSnapshot
        };
    }

    /// <summary>
    /// Inicia a preparação do pedido, alterando o status de Received para InProgress.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite a transição para InProgress.</exception>
    public void StartPreparation()
    {
        if (Status != EnumPreparationStatus.Received)
        {
            throw new InvalidOperationException(
                $"Não é possível iniciar a preparação. Status atual: {Status}. Apenas preparações com status 'Received' podem ser iniciadas.");
        }

        Status = EnumPreparationStatus.InProgress;
    }

    /// <summary>
    /// Finaliza a preparação do pedido, alterando o status de InProgress para Finished.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite a transição para Finished.</exception>
    public void FinishPreparation()
    {
        if (Status != EnumPreparationStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Não é possível finalizar a preparação. Status atual: {Status}. Apenas preparações com status 'InProgress' podem ser finalizadas.");
        }

        Status = EnumPreparationStatus.Finished;
    }
}
