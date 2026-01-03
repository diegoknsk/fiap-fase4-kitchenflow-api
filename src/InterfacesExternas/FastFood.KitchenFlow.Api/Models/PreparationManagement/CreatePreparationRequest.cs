using System.ComponentModel.DataAnnotations;

namespace FastFood.KitchenFlow.Api.Models.PreparationManagement;

/// <summary>
/// Request DTO para criação de uma nova preparação.
/// </summary>
public class CreatePreparationRequest
{
    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    [Required(ErrorMessage = "OrderId é obrigatório.")]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Snapshot JSON do pedido no momento do pagamento.
    /// </summary>
    [Required(ErrorMessage = "OrderSnapshot é obrigatório.")]
    public string OrderSnapshot { get; set; } = string.Empty;
}
