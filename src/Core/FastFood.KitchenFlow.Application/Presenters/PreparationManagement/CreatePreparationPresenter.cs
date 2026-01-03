using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.Presenters.PreparationManagement;

/// <summary>
/// Presenter que transforma CreatePreparationOutputModel em CreatePreparationResponse.
/// </summary>
public static class CreatePreparationPresenter
{
    /// <summary>
    /// Transforma o OutputModel em Response adicionando a mensagem de sucesso.
    /// </summary>
    /// <param name="outputModel">OutputModel retornado pelo UseCase.</param>
    /// <returns>Response pronto para ser retornado pela API.</returns>
    public static CreatePreparationResponse Present(CreatePreparationOutputModel outputModel)
    {
        return new CreatePreparationResponse
        {
            Id = outputModel.Id,
            OrderId = outputModel.OrderId,
            Status = outputModel.Status,
            CreatedAt = outputModel.CreatedAt,
            Message = "Preparação criada com sucesso"
        };
    }
}
