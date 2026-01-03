using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FastFood.KitchenFlow.Application.DTOs;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;

namespace FastFood.KitchenFlow.Application.UseCases.PreparationManagement;

/// <summary>
/// UseCase responsável por criar uma nova preparação quando o pagamento for confirmado.
/// </summary>
public class CreatePreparationUseCase
{
    private readonly IPreparationRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Preparation.</param>
    public CreatePreparationUseCase(IPreparationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Executa a criação de uma nova preparação.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para criação da preparação.</param>
    /// <returns>Response com os dados da preparação criada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="PreparationAlreadyExistsException">Lançada quando já existe uma preparação para o OrderId (idempotência).</exception>
    public async Task<CreatePreparationResponse> ExecuteAsync(CreatePreparationInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel.OrderId == Guid.Empty)
        {
            throw new ArgumentException("OrderId não pode ser vazio.", nameof(inputModel));
        }

        if (string.IsNullOrWhiteSpace(inputModel.OrderSnapshot))
        {
            throw new ArgumentException("OrderSnapshot não pode ser nulo ou vazio.", nameof(inputModel));
        }

        // Validar estrutura do OrderSnapshot (deserializar e validar)
        OrderSnapshotDto? orderSnapshotDto;
        try
        {
            orderSnapshotDto = JsonSerializer.Deserialize<OrderSnapshotDto>(
                inputModel.OrderSnapshot,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new ArgumentException($"OrderSnapshot contém JSON inválido: {ex.Message}", nameof(inputModel));
        }

        if (orderSnapshotDto == null)
        {
            throw new ArgumentException("OrderSnapshot não pode ser deserializado.", nameof(inputModel));
        }

        // Validar estrutura usando Data Annotations
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderSnapshotDto);
        bool isValid = Validator.TryValidateObject(orderSnapshotDto, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults
                .SelectMany(r => r.MemberNames.Select(m => $"{m}: {r.ErrorMessage}"))
                .ToList();
            throw new ArgumentException(
                $"OrderSnapshot contém dados inválidos: {string.Join("; ", errors)}",
                nameof(inputModel));
        }

        // Validar que OrderId do snapshot corresponde ao OrderId do input
        if (orderSnapshotDto.OrderId != inputModel.OrderId)
        {
            throw new ArgumentException(
                $"OrderId do OrderSnapshot ({orderSnapshotDto.OrderId}) não corresponde ao OrderId informado ({inputModel.OrderId}).",
                nameof(inputModel));
        }

        // Verificar idempotência (não criar duplicado)
        var existingPreparation = await _repository.GetByOrderIdAsync(inputModel.OrderId);
        if (existingPreparation != null)
        {
            // Lançar exceção para indicar conflito (409 Conflict)
            throw new PreparationAlreadyExistsException(inputModel.OrderId);
        }

        // Criar entidade de domínio
        var preparation = Preparation.Create(inputModel.OrderId, inputModel.OrderSnapshot);

        // Persistir
        await _repository.CreateAsync(preparation);

        // Criar OutputModel
        var createOutputModel = new CreatePreparationOutputModel
        {
            Id = preparation.Id,
            OrderId = preparation.OrderId,
            Status = (int)preparation.Status,
            CreatedAt = preparation.CreatedAt
        };

        // Chamar Presenter e retornar Response
        return CreatePreparationPresenter.Present(createOutputModel);
    }
}
