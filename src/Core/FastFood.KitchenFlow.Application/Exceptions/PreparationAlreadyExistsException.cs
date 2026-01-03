namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma preparação já existe para um OrderId específico.
/// Usada para implementar idempotência no endpoint de criação de preparações.
/// </summary>
public class PreparationAlreadyExistsException : InvalidOperationException
{
    /// <summary>
    /// Identificador do pedido que já possui uma preparação.
    /// </summary>
    public Guid OrderId { get; }

    /// <summary>
    /// Construtor que recebe o OrderId que já possui uma preparação.
    /// </summary>
    /// <param name="orderId">Identificador do pedido.</param>
    public PreparationAlreadyExistsException(Guid orderId)
        : base($"Já existe uma preparação para o pedido {orderId}.")
    {
        OrderId = orderId;
    }
}
