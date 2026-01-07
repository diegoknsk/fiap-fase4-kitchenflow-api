using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.DeliveryManagement;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;

namespace FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;

/// <summary>
/// UseCase responsável por listar entregas prontas para retirada com paginação.
/// </summary>
public class GetReadyDeliveriesUseCase
{
    private readonly IDeliveryRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Delivery.</param>
    public GetReadyDeliveriesUseCase(IDeliveryRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Executa a listagem de entregas prontas para retirada.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para listagem (paginação).</param>
    /// <returns>ApiResponse com a lista de entregas prontas para retirada.</returns>
    public virtual async Task<ApiResponse<GetReadyDeliveriesResponse>> ExecuteAsync(GetReadyDeliveriesInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel == null)
        {
            throw new ValidationException("Dados de entrada não podem ser nulos.");
        }

        // Validar parâmetros de paginação
        if (inputModel.PageNumber < 1)
        {
            throw new ValidationException("PageNumber deve ser maior ou igual a 1.");
        }

        if (inputModel.PageSize < 1)
        {
            throw new ValidationException("PageSize deve ser maior ou igual a 1.");
        }

        if (inputModel.PageSize > 100)
        {
            throw new ValidationException("PageSize não pode ser maior que 100.");
        }

        // Buscar entregas prontas para retirada
        var (deliveries, totalCount) = await _repository.GetReadyDeliveriesAsync(
            inputModel.PageNumber,
            inputModel.PageSize);

        // Calcular total de páginas
        var totalPages = (int)Math.Ceiling(totalCount / (double)inputModel.PageSize);

        // Criar OutputModel
        var outputModel = new GetReadyDeliveriesOutputModel
        {
            Items = deliveries.Select(d => new DeliveryItemOutputModel
            {
                Id = d.Id,
                PreparationId = d.PreparationId,
                OrderId = d.OrderId,
                Status = (int)d.Status,
                CreatedAt = d.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageNumber = inputModel.PageNumber,
            PageSize = inputModel.PageSize,
            TotalPages = totalPages
        };

        // Chamar Presenter e retornar Response
        return GetReadyDeliveriesPresenter.Present(outputModel);
    }
}
