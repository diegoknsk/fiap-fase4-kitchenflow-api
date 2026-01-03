using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;

/// <summary>
/// UseCase responsável por criar uma nova entrega quando uma preparação for finalizada.
/// </summary>
public class CreateDeliveryUseCase
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IPreparationRepository _preparationRepository;

    /// <summary>
    /// Construtor que recebe os repositories via Dependency Injection.
    /// </summary>
    /// <param name="deliveryRepository">Repository para acesso a dados de Delivery.</param>
    /// <param name="preparationRepository">Repository para acesso a dados de Preparation.</param>
    public CreateDeliveryUseCase(
        IDeliveryRepository deliveryRepository,
        IPreparationRepository preparationRepository)
    {
        _deliveryRepository = deliveryRepository;
        _preparationRepository = preparationRepository;
    }

    /// <summary>
    /// Executa a criação de uma nova entrega.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para criação da entrega.</param>
    /// <returns>Response com os dados da entrega criada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="PreparationNotFoundException">Lançada quando a preparação não é encontrada.</exception>
    /// <exception cref="PreparationNotFinishedException">Lançada quando a preparação não está com status Finished.</exception>
    /// <exception cref="DeliveryAlreadyExistsException">Lançada quando já existe uma entrega para a preparação (idempotência).</exception>
    public async Task<CreateDeliveryResponse> ExecuteAsync(CreateDeliveryInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel.PreparationId == Guid.Empty)
        {
            throw new ArgumentException("PreparationId não pode ser vazio.", nameof(inputModel));
        }

        // Verificar se a Preparation existe
        var preparation = await _preparationRepository.GetByIdAsync(inputModel.PreparationId);
        if (preparation == null)
        {
            throw new PreparationNotFoundException(inputModel.PreparationId);
        }

        // Verificar se a Preparation está com status Finished
        if (preparation.Status != EnumPreparationStatus.Finished)
        {
            throw new PreparationNotFinishedException(inputModel.PreparationId, (int)preparation.Status);
        }

        // Verificar idempotência (não criar duplicado)
        var existingDelivery = await _deliveryRepository.GetByPreparationIdAsync(inputModel.PreparationId);
        if (existingDelivery != null)
        {
            // Lançar exceção para indicar conflito (409 Conflict)
            throw new DeliveryAlreadyExistsException(inputModel.PreparationId);
        }

        // Criar entidade de domínio
        var delivery = Delivery.Create(inputModel.PreparationId, inputModel.OrderId);

        // Persistir
        await _deliveryRepository.CreateAsync(delivery);

        // Criar OutputModel
        var createOutputModel = new CreateDeliveryOutputModel
        {
            Id = delivery.Id,
            PreparationId = delivery.PreparationId,
            OrderId = delivery.OrderId,
            Status = (int)delivery.Status,
            CreatedAt = delivery.CreatedAt
        };

        // Chamar Presenter e retornar Response
        return CreateDeliveryPresenter.Present(createOutputModel);
    }
}
