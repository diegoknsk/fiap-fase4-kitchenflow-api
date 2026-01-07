namespace FastFood.KitchenFlow.Application.InputModels.PreparationManagement;

/// <summary>
/// InputModel para listagem de preparações com paginação e filtro opcional por status.
/// </summary>
public class GetPreparationsInputModel
{
    /// <summary>
    /// Número da página (começando em 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Filtro opcional por status (0 = Received, 1 = InProgress, 2 = Finished).
    /// Null = sem filtro.
    /// </summary>
    public int? Status { get; set; }
}
