using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FastFood.KitchenFlow.Application.DTOs;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
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
    /// <returns>ApiResponse com os dados da preparação criada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    /// <exception cref="PreparationAlreadyExistsException">Lançada quando já existe uma preparação para o OrderId (idempotência).</exception>
    public virtual async Task<ApiResponse<CreatePreparationResponse>> ExecuteAsync(CreatePreparationInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel == null)
        {
            throw new Exceptions.ValidationException("Dados de entrada não podem ser nulos.");
        }

        if (inputModel.OrderId == Guid.Empty)
        {
            throw new Exceptions.ValidationException("OrderId não pode ser vazio.");
        }

        if (string.IsNullOrWhiteSpace(inputModel.OrderSnapshot))
        {
            throw new Exceptions.ValidationException("OrderSnapshot não pode ser nulo ou vazio.");
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
            throw new Exceptions.ValidationException($"OrderSnapshot contém JSON inválido: {ex.Message}");
        }

        if (orderSnapshotDto == null)
        {
            throw new Exceptions.ValidationException("OrderSnapshot não pode ser deserializado.");
        }

        // Validar estrutura usando Data Annotations (validação relaxada - OrderSnapshot é apenas referência)
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderSnapshotDto);
        bool isValid = Validator.TryValidateObject(orderSnapshotDto, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults
                .SelectMany(r => r.MemberNames.Select(m => $"{m}: {r.ErrorMessage}"))
                .ToList();
            throw new Exceptions.ValidationException(
                $"OrderSnapshot contém dados inválidos: {string.Join("; ", errors)}");
        }

        // Nota: Validação de correspondência de OrderId removida - OrderSnapshot é apenas referência

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
