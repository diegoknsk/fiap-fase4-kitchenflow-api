using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma StartPreparationOutputModel em ApiResponse de StartPreparationResponse.
/// </summary>
public static class StartPreparationPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<StartPreparationResponse> Present(StartPreparationOutputModel outputModel)
    {
        var response = new StartPreparationResponse
        {
            Id = outputModel.Id,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt
        };
        return ApiResponse<StartPreparationResponse>.Ok(response, "Preparação iniciada com sucesso.");
    }
}
