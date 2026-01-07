using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma CreateDeliveryOutputModel em ApiResponse de CreateDeliveryResponse.
/// </summary>
public static class CreateDeliveryPresenter
{
    /// <summary>
    /// Transforma o OutputModel em ApiResponse com mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>ApiResponse pronto para ser retornado pela API.</returns>
    public static ApiResponse<CreateDeliveryResponse> Present(CreateDeliveryOutputModel outputModel)
    {
        var response = new CreateDeliveryResponse
        {
            Id = outputModel.Id,
            PreparationId = outputModel.PreparationId,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt
        };
        return ApiResponse<CreateDeliveryResponse>.Ok(response, "Entrega criada com sucesso.");
    }
}
