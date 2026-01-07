namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma entrega já existe para uma Preparation específica.
/// Usada para implementar idempotência no endpoint de criação de entregas.
/// </summary>
public class DeliveryAlreadyExistsException : InvalidOperationException
{
    /// <summary>
    /// Identificador da preparação que já possui uma entrega.
    /// </summary>
    public Guid PreparationId { get; }

    /// <summary>
    /// Construtor que recebe o PreparationId que já possui uma entrega.
    /// </summary>
    /// <param name="preparationId">Identificador da preparação.</param>
    public DeliveryAlreadyExistsException(Guid preparationId)
        : base($"Já existe uma entrega para a preparação {preparationId}.")
    {
        PreparationId = preparationId;
    }
}
