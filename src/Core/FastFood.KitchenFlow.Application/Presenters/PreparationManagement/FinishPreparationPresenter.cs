using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma FinishPreparationOutputModel em ApiResponse de FinishPreparationResponse.
/// </summary>
public static class FinishPreparationPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<FinishPreparationResponse> Present(FinishPreparationOutputModel outputModel)
    {
        var response = new FinishPreparationResponse
        {
            Id = outputModel.Id,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            DeliveryId = outputModel.DeliveryId
        };
        return ApiResponse<FinishPreparationResponse>.Ok(response, "Preparação finalizada com sucesso.");
    }
}
