using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma GetPreparationsOutputModel em GetPreparationsResponse.
/// </summary>
public static class GetPreparationsPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static GetPreparationsResponse Present(GetPreparationsOutputModel outputModel)
    {
        return new GetPreparationsResponse
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
    }
}
