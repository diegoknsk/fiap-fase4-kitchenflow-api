namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma preparação não é encontrada.
/// </summary>
public class PreparationNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Identificador da preparação não encontrada.
    /// </summary>
    public Guid PreparationId { get; }

    /// <summary>
    /// Construtor que recebe o PreparationId não encontrado.
    /// </summary>
    /// <param name="preparationId">Identificador da preparação.</param>
    public PreparationNotFoundException(Guid preparationId)
        : base($"Preparação {preparationId} não encontrada.")
    {
        PreparationId = preparationId;
    }
}
