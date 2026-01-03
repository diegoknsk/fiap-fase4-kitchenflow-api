namespace FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;

/// <summary>
/// InputModel para listagem de entregas prontas para retirada.
/// </summary>
public class GetReadyDeliveriesInputModel
{
    /// <summary>
    /// Número da página (começa em 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
