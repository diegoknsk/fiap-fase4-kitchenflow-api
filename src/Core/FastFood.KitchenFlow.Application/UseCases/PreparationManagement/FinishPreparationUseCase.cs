using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.UseCases.PreparationManagement;

/// <summary>
/// UseCase responsável por finalizar uma preparação (InProgress → Finished).
/// </summary>
public class FinishPreparationUseCase
{
    private readonly IPreparationRepository _repository;
    private readonly IDeliveryRepository _deliveryRepository;

    /// <summary>
    /// Construtor que recebe os repositories via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Preparation.</param>
    /// <param name="deliveryRepository">Repository para acesso a dados de Delivery.</param>
    public FinishPreparationUseCase(IPreparationRepository repository, IDeliveryRepository deliveryRepository)
    {
        _repository = repository;
        _deliveryRepository = deliveryRepository;
    }

    /// <summary>
    /// Executa a finalização de uma preparação.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para finalizar a preparação.</param>
    /// <returns>ApiResponse com os dados da preparação finalizada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="PreparationNotFoundException">Lançada quando a preparação não é encontrada.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite finalizar a preparação.</exception>
    public async Task<ApiResponse<FinishPreparationResponse>> ExecuteAsync(FinishPreparationInputModel inputModel)
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

        // Buscar Preparation
        var preparation = await _repository.GetByIdAsync(inputModel.Id);
        if (preparation == null)
        {
            throw new PreparationNotFoundException(inputModel.Id);
        }

        // Validar status
        if (preparation.Status != EnumPreparationStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Não é possível finalizar a preparação. Status atual: {preparation.Status}. Apenas preparações com status 'InProgress' podem ser finalizadas.");
        }

        // Finalizar Preparation (método de domínio valida e altera status)
        preparation.FinishPreparation();

        // Atualizar no banco
        await _repository.UpdateAsync(preparation);

        // Criar delivery automaticamente (idempotência: verificar se já existe)
        Delivery? delivery = await _deliveryRepository.GetByPreparationIdAsync(preparation.Id);
        Guid? deliveryId = null;

        if (delivery == null)
        {
            // Criar novo delivery
            delivery = Delivery.Create(preparation.Id, preparation.OrderId);
            await _deliveryRepository.CreateAsync(delivery);
            deliveryId = delivery.Id;
        }
        else
        {
            // Usar delivery existente (idempotência)
            deliveryId = delivery.Id;
        }

        // Criar OutputModel
        var outputModel = new FinishPreparationOutputModel
        {
            Id = preparation.Id,
            OrderId = preparation.OrderId,
            Status = (int)preparation.Status,
            CreatedAt = preparation.CreatedAt,
            DeliveryId = deliveryId
        };

        // Chamar Presenter e retornar Response
        return FinishPreparationPresenter.Present(outputModel);
    }
}
