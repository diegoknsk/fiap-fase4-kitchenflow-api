using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;

/// <summary>
/// Presenter que transforma FinalizeDeliveryOutputModel em FinalizeDeliveryResponse.
/// </summary>
public static class FinalizeDeliveryPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response adicionando a mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static FinalizeDeliveryResponse Present(FinalizeDeliveryOutputModel outputModel)
    {
        return new FinalizeDeliveryResponse
        {
            Id = outputModel.Id,
            PreparationId = outputModel.PreparationId,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            FinalizedAt = outputModel.FinalizedAt,
            Message = "Entrega finalizada com sucesso"
        };
    }
}
