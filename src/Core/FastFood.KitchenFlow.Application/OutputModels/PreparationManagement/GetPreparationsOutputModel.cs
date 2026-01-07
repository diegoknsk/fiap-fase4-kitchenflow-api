namespace FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;

/// <summary>
/// OutputModel retornado após listagem de preparações com paginação.
/// </summary>
public class GetPreparationsOutputModel
{
    /// <summary>
    /// Lista de preparações da página atual.
    /// </summary>
    public IEnumerable<PreparationItemOutputModel> Items { get; set; } = new List<PreparationItemOutputModel>();

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
