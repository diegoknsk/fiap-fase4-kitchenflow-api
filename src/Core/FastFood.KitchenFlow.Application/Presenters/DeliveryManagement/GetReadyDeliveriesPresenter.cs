using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma GetReadyDeliveriesOutputModel em ApiResponse de GetReadyDeliveriesResponse.
/// </summary>
public static class GetReadyDeliveriesPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<GetReadyDeliveriesResponse> Present(GetReadyDeliveriesOutputModel outputModel)
    {
        var response = new GetReadyDeliveriesResponse
        {
            Items = outputModel.Items.Select(item => new DeliveryItemResponse
            {
                Id = item.Id,
                PreparationId = item.PreparationId,
                OrderId = item.OrderId,
                Status = item.Status,
                CreatedAt = item.CreatedAt
            }).ToList(),
            TotalCount = outputModel.TotalCount,
            PageNumber = outputModel.PageNumber,
            PageSize = outputModel.PageSize,
            TotalPages = outputModel.TotalPages
        };
        return ApiResponse<GetReadyDeliveriesResponse>.Ok(response, "Lista de entregas prontas para retirada retornada com sucesso.");
    }
}
