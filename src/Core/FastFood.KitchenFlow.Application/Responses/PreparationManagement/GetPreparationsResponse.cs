namespace FastFood.KitchenFlow.Application.Responses.PreparationManagement;

/// <summary>
/// Response retornado pela API após listagem de preparações com paginação.
/// </summary>
public class GetPreparationsResponse
{
    /// <summary>
    /// Lista de preparações da página atual.
    /// </summary>
    public IEnumerable<PreparationItemResponse> Items { get; set; } = new List<PreparationItemResponse>();

    /// <summary>
    /// Total de registros encontrados (considerando filtros).
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Número da página atual.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de páginas disponíveis.
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Response que representa um item de preparação na listagem.
/// </summary>
public class PreparationItemResponse
{
    /// <summary>
    /// Identificador único da preparação.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Status da preparação (0 = Received, 1 = InProgress, 2 = Finished).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da preparação.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Snapshot JSON do pedido (incluído para exibição no display da cozinha).
    /// </summary>
    public string OrderSnapshot { get; set; } = string.Empty;
}
