using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;

/// <summary>
/// UseCase responsável por finalizar uma entrega.
/// </summary>
public class FinalizeDeliveryUseCase
{
    private readonly IDeliveryRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Delivery.</param>
    public FinalizeDeliveryUseCase(IDeliveryRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Executa a finalização de uma entrega.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para finalização da entrega.</param>
    /// <returns>ApiResponse com os dados da entrega finalizada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="DeliveryNotFoundException">Lançada quando a entrega não é encontrada.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite a finalização.</exception>
    public async Task<ApiResponse<FinalizeDeliveryResponse>> ExecuteAsync(FinalizeDeliveryInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel == null)
        {
            throw new ValidationException("Dados de entrada não podem ser nulos.");
        }

        if (inputModel.Id == Guid.Empty)
        {
            throw new ValidationException("Id não pode ser vazio.");
        }

        // Buscar entrega
        var delivery = await _repository.GetByIdAsync(inputModel.Id);
        if (delivery == null)
        {
            throw new DeliveryNotFoundException(inputModel.Id);
        }

        // Validar status (deve ser ReadyForPickup)
        if (delivery.Status != EnumDeliveryStatus.ReadyForPickup)
        {
            throw new InvalidOperationException(
                $"Não é possível finalizar a entrega. Status atual: {delivery.Status}. Apenas entregas com status 'ReadyForPickup' podem ser finalizadas.");
        }

        // Finalizar entrega (método de domínio)
        delivery.FinalizeDelivery();

        // Atualizar no banco
        await _repository.UpdateAsync(delivery);

        // Criar OutputModel
        var outputModel = new FinalizeDeliveryOutputModel
        {
            Id = delivery.Id,
            PreparationId = delivery.PreparationId,
            OrderId = delivery.OrderId,
            Status = (int)delivery.Status,
            CreatedAt = delivery.CreatedAt,
            FinalizedAt = delivery.FinalizedAt
        };

        // Chamar Presenter e retornar Response
        return FinalizeDeliveryPresenter.Present(outputModel);
    }
}
