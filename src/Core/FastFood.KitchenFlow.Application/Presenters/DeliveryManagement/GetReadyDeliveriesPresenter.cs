using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma GetReadyDeliveriesOutputModel em GetReadyDeliveriesResponse.
/// </summary>
public static class GetReadyDeliveriesPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static GetReadyDeliveriesResponse Present(GetReadyDeliveriesOutputModel outputModel)
    {
        return new GetReadyDeliveriesResponse
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
    }
}
