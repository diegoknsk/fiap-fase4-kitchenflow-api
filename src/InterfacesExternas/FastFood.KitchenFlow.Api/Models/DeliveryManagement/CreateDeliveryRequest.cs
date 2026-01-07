using System.ComponentModel.DataAnnotations;

namespace FastFood.KitchenFlow.Api.Models.DeliveryManagement;

/// <summary>
/// Request DTO para criação de uma nova entrega.
/// </summary>
public class CreateDeliveryRequest
{
    /// <summary>
    /// Identificador da preparação associada (obrigatório).
    /// </summary>
    [Required(ErrorMessage = "PreparationId é obrigatório.")]
    public Guid PreparationId { get; set; }

    /// <summary>
    /// Identificador do pedido (opcional, apenas para facilitar consulta).
    /// </summary>
    public Guid? OrderId { get; set; }
}
