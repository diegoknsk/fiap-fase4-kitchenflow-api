using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Ports;

/// <summary>
/// Port (interface) que define o contrato para acesso a dados de Preparation.
/// Esta interface será implementada pela camada Infra.Persistence.
/// </summary>
public interface IPreparationRepository
{
    /// <summary>
    /// Cria uma nova preparação no banco de dados.
    /// </summary>
    /// <param name="preparation">Entidade de domínio Preparation a ser persistida.</param>
    /// <returns>Id da preparação criada.</returns>
    Task<Guid> CreateAsync(Preparation preparation);

    /// <summary>
    /// Busca uma preparação pelo OrderId.
    /// Usado para verificar idempotência (evitar criar duplicados).
    /// </summary>
    /// <param name="orderId">Identificador do pedido.</param>
    /// <returns>Preparation encontrada ou null se não existir.</returns>
    Task<Preparation?> GetByOrderIdAsync(Guid orderId);
}
