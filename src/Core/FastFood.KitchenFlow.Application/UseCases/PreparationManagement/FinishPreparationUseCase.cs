using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Application.UseCases.PreparationManagement;

/// <summary>
/// UseCase responsável por finalizar uma preparação (InProgress → Finished).
/// </summary>
public class FinishPreparationUseCase
{
    private readonly IPreparationRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Preparation.</param>
    public FinishPreparationUseCase(IPreparationRepository repository)
    {
        _repository = repository;
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
        if (inputModel.Id == Guid.Empty)
        {
            throw new ArgumentException("Id não pode ser vazio.", nameof(inputModel));
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

        // Criar OutputModel
        var outputModel = new FinishPreparationOutputModel
        {
            Id = preparation.Id,
            OrderId = preparation.OrderId,
            Status = (int)preparation.Status,
            CreatedAt = preparation.CreatedAt
        };

        // Chamar Presenter e retornar Response
        return FinishPreparationPresenter.Present(outputModel);
    }
}
