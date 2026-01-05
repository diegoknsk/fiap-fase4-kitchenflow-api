using System.ComponentModel.DataAnnotations;

namespace FastFood.KitchenFlow.Application.DTOs;

/// <summary>
/// DTO que representa o snapshot completo de um pedido no momento do pagamento.
/// Este snapshot contém todas as informações necessárias para exibição na cozinha,
/// referência para entrega e auditoria.
/// </summary>
public class OrderSnapshotDto
{
    /// <summary>
    /// Identificador do pedido (opcional - usado apenas como referência).
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Código do pedido (opcional).
    /// </summary>
    public string? OrderCode { get; set; }

    /// <summary>
    /// Identificador do cliente (opcional).
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Valor total do pedido.
    /// </summary>
    [Required(ErrorMessage = "TotalPrice é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "TotalPrice deve ser maior ou igual a zero.")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Data e hora de criação do pedido.
    /// </summary>
    [Required(ErrorMessage = "CreatedAt é obrigatório.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Lista de itens do pedido.
    /// </summary>
    [Required(ErrorMessage = "Items é obrigatório.")]
    [MinLength(1, ErrorMessage = "Items deve conter pelo menos um item.")]
    public List<OrderItemSnapshotDto> Items { get; set; } = new();

    /// <summary>
    /// Identificador do pagamento.
    /// </summary>
    [Required(ErrorMessage = "PaymentId é obrigatório.")]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Data e hora do pagamento.
    /// </summary>
    [Required(ErrorMessage = "PaidAt é obrigatório.")]
    public DateTime PaidAt { get; set; }
}
