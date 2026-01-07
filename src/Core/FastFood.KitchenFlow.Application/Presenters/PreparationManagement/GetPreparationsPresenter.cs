using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma GetPreparationsOutputModel em ApiResponse de GetPreparationsResponse.
/// </summary>
public static class GetPreparationsPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<GetPreparationsResponse> Present(GetPreparationsOutputModel outputModel)
    {
        var response = new GetPreparationsResponse
        {
            Items = outputModel.Items.Select(item => new PreparationItemResponse
            {
                Id = item.Id,
                OrderId = item.OrderId,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                OrderSnapshot = item.OrderSnapshot
            }),
            TotalCount = outputModel.TotalCount,
            PageNumber = outputModel.PageNumber,
            PageSize = outputModel.PageSize,
            TotalPages = outputModel.TotalPages
        };
        return ApiResponse<GetPreparationsResponse>.Ok(response, "Lista de preparações retornada com sucesso.");
    }
}
