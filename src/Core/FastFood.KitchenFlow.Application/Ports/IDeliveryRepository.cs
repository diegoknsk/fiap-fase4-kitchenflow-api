using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Ports;

/// <summary>
/// Port (interface) que define o contrato para acesso a dados de Delivery.
/// Esta interface será implementada pela camada Infra.Persistence.
/// </summary>
public interface IDeliveryRepository
{
    /// <summary>
    /// Cria uma nova entrega no banco de dados.
    /// </summary>
    /// <param name="delivery">Entidade de domínio Delivery a ser persistida.</param>
    /// <returns>Id da entrega criada.</returns>
    Task<Guid> CreateAsync(Delivery delivery);

    /// <summary>
    /// Busca uma entrega pelo PreparationId.
    /// Usado para verificar idempotência (evitar criar duplicados).
    /// </summary>
    /// <param name="preparationId">Identificador da preparação.</param>
    /// <returns>Delivery encontrada ou null se não existir.</returns>
    Task<Delivery?> GetByPreparationIdAsync(Guid preparationId);

    /// <summary>
    /// Busca entregas prontas para retirada (status ReadyForPickup) com paginação.
    /// </summary>
    /// <param name="pageNumber">Número da página (começa em 1).</param>
    /// <param name="pageSize">Tamanho da página.</param>
    /// <returns>Tupla contendo a lista de deliveries e o total de registros.</returns>
    Task<(List<Delivery> deliveries, int totalCount)> GetReadyDeliveriesAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Busca uma entrega pelo Id.
    /// </summary>
    /// <param name="id">Identificador da entrega.</param>
    /// <returns>Delivery encontrada ou null se não existir.</returns>
    Task<Delivery?> GetByIdAsync(Guid id);

    /// <summary>
    /// Atualiza uma entrega existente no banco de dados.
    /// </summary>
    /// <param name="delivery">Entidade de domínio Delivery a ser atualizada.</param>
    Task UpdateAsync(Delivery delivery);
}
