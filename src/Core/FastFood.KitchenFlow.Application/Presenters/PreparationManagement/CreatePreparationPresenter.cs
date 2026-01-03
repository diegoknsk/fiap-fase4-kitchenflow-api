using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma CreatePreparationOutputModel em ApiResponse de CreatePreparationResponse.
/// </summary>
public static class CreatePreparationPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<CreatePreparationResponse> Present(CreatePreparationOutputModel outputModel)
    {
        var response = new CreatePreparationResponse
        {
            Id = outputModel.Id,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt
        };
        return ApiResponse<CreatePreparationResponse>.Ok(response, "Preparação criada com sucesso.");
    }
}
