using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Application.UseCases.PreparationManagement;

/// <summary>
/// UseCase responsável por iniciar uma preparação (Received → InProgress).
/// </summary>
public class StartPreparationUseCase
{
    private readonly IPreparationRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Preparation.</param>
    public StartPreparationUseCase(IPreparationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Executa o início de uma preparação.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para iniciar a preparação.</param>
    /// <returns>Response com os dados da preparação iniciada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="PreparationNotFoundException">Lançada quando a preparação não é encontrada.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando o status atual não permite iniciar a preparação.</exception>
    public async Task<StartPreparationResponse> ExecuteAsync(StartPreparationInputModel inputModel)
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
        if (preparation.Status != EnumPreparationStatus.Received)
        {
            throw new InvalidOperationException(
                $"Não é possível iniciar a preparação. Status atual: {preparation.Status}. Apenas preparações com status 'Received' podem ser iniciadas.");
        }

        // Iniciar Preparation (método de domínio valida e altera status)
        preparation.StartPreparation();

        // Atualizar no banco
        await _repository.UpdateAsync(preparation);

        // Criar OutputModel
        var outputModel = new StartPreparationOutputModel
        {
            Id = preparation.Id,
            OrderId = preparation.OrderId,
            Status = (int)preparation.Status,
            CreatedAt = preparation.CreatedAt
        };

        // Chamar Presenter e retornar Response
        return StartPreparationPresenter.Present(outputModel);
    }
}
