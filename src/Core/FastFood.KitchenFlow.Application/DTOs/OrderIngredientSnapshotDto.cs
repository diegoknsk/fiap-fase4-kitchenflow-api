using System.ComponentModel.DataAnnotations;

namespace FastFood.KitchenFlow.Application.DTOs;

/// <summary>
/// DTO que representa um ingrediente de um item do pedido no snapshot.
/// </summary>
public class OrderIngredientSnapshotDto
{
    /// <summary>
    /// Nome do ingrediente.
    /// </summary>
    [Required(ErrorMessage = "Nome do ingrediente é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Preço do ingrediente.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Preço do ingrediente deve ser maior ou igual a zero.")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantidade do ingrediente.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantidade do ingrediente deve ser maior que zero.")]
    public int Quantity { get; set; }
}
