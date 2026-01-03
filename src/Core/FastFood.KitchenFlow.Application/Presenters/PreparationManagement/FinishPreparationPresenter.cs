using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma FinishPreparationOutputModel em FinishPreparationResponse.
/// </summary>
public static class FinishPreparationPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response adicionando a mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static FinishPreparationResponse Present(FinishPreparationOutputModel outputModel)
    {
        return new FinishPreparationResponse
        {
            Id = outputModel.Id,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            Message = "Preparação finalizada com sucesso"
        };
    }
}
