namespace FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;

/// <summary>
/// OutputModel retornado após listagem de entregas prontas para retirada.
/// </summary>
public class GetReadyDeliveriesOutputModel
{
    /// <summary>
    /// Lista de entregas prontas para retirada.
    /// </summary>
    public List<DeliveryItemOutputModel> Items { get; set; } = new();

    /// <summary>
    /// Total de registros encontrados.
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
    /// Total de páginas.
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Item individual de entrega na listagem.
/// </summary>
public class DeliveryItemOutputModel
{
    /// <summary>
    /// Identificador único da entrega.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador da preparação associada.
    /// </summary>
    public Guid PreparationId { get; set; }

    /// <summary>
    /// Identificador do pedido (opcional).
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Status da entrega (1 = ReadyForPickup, 2 = Finalized).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Data e hora de criação da entrega.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
