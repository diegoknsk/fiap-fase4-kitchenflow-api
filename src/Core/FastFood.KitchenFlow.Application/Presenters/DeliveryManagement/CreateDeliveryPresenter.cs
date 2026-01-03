using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma CreateDeliveryOutputModel em CreateDeliveryResponse.
/// </summary>
public static class CreateDeliveryPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response adicionando a mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static CreateDeliveryResponse Present(CreateDeliveryOutputModel outputModel)
    {
        return new CreateDeliveryResponse
        {
            Id = outputModel.Id,
            PreparationId = outputModel.PreparationId,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            Message = "Entrega criada com sucesso"
        };
    }
}
