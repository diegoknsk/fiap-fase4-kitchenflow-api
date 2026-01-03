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

    /// <summary>
    /// Busca uma preparação pelo Id.
    /// </summary>
    /// <param name="id">Identificador da preparação.</param>
    /// <returns>Preparation encontrada ou null se não existir.</returns>
    Task<Preparation?> GetByIdAsync(Guid id);

    /// <summary>
    /// Busca preparações com paginação e filtro opcional por status.
    /// </summary>
    /// <param name="pageNumber">Número da página (começando em 1).</param>
    /// <param name="pageSize">Tamanho da página.</param>
    /// <param name="status">Filtro opcional por status (null = sem filtro).</param>
    /// <returns>Tupla contendo a lista de preparações e o total de registros.</returns>
    Task<(IEnumerable<Preparation> preparations, int totalCount)> GetPagedAsync(int pageNumber, int pageSize, int? status);

    /// <summary>
    /// Atualiza uma preparação existente no banco de dados.
    /// </summary>
    /// <param name="preparation">Entidade de domínio Preparation a ser atualizada.</param>
    /// <returns>Task que representa a operação assíncrona.</returns>
    Task UpdateAsync(Preparation preparation);
}
