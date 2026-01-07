namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma preparação não está com status Finished.
/// </summary>
public class PreparationNotFinishedException : InvalidOperationException
{
    /// <summary>
    /// Identificador da preparação.
    /// </summary>
    public Guid PreparationId { get; }

    /// <summary>
    /// Status atual da preparação.
    /// </summary>
    public int CurrentStatus { get; }

    /// <summary>
    /// Construtor que recebe o PreparationId e o status atual.
    /// </summary>
    /// <param name="preparationId">Identificador da preparação.</param>
    /// <param name="currentStatus">Status atual da preparação.</param>
    public PreparationNotFinishedException(Guid preparationId, int currentStatus)
        : base($"Preparação {preparationId} não está finalizada. Status atual: {currentStatus}. Apenas preparações com status 'Finished' podem ter entregas criadas.")
    {
        PreparationId = preparationId;
        CurrentStatus = currentStatus;
    }
}
