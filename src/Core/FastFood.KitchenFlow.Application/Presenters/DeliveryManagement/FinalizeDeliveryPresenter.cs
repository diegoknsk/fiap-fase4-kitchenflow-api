using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma FinalizeDeliveryOutputModel em ApiResponse de FinalizeDeliveryResponse.
/// </summary>
public static class FinalizeDeliveryPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<FinalizeDeliveryResponse> Present(FinalizeDeliveryOutputModel outputModel)
    {
        var response = new FinalizeDeliveryResponse
        {
            Id = outputModel.Id,
            PreparationId = outputModel.PreparationId,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            FinalizedAt = outputModel.FinalizedAt
        };
        return ApiResponse<FinalizeDeliveryResponse>.Ok(response, "Entrega finalizada com sucesso.");
    }
}
