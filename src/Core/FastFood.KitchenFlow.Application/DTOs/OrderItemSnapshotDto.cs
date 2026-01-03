using System.ComponentModel.DataAnnotations;

namespace FastFood.KitchenFlow.Application.DTOs;

/// <summary>
/// DTO que representa um item do pedido no snapshot.
/// </summary>
public class OrderItemSnapshotDto
{
    /// <summary>
    /// Identificador do produto.
    /// </summary>
    [Required(ErrorMessage = "ProductId é obrigatório.")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    [Required(ErrorMessage = "ProductName é obrigatório.")]
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade do item.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity deve ser maior que zero.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Preço unitário do produto.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "UnitPrice deve ser maior ou igual a zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Preço final do item (incluindo ingredientes adicionais).
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "FinalPrice deve ser maior ou igual a zero.")]
    public decimal FinalPrice { get; set; }

    /// <summary>
    /// Lista de ingredientes customizados do item (opcional).
    /// </summary>
    public List<OrderIngredientSnapshotDto>? Ingredients { get; set; }

    /// <summary>
    /// Observações do cliente sobre o item (opcional).
    /// </summary>
    public string? Observation { get; set; }
}
