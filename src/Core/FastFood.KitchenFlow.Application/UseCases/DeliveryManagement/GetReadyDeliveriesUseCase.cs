using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
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
    /// <returns>Response com a lista de entregas prontas para retirada.</returns>
    public async Task<GetReadyDeliveriesResponse> ExecuteAsync(GetReadyDeliveriesInputModel inputModel)
    {
        // Validar parâmetros de paginação
        if (inputModel.PageNumber < 1)
        {
            inputModel.PageNumber = 1;
        }

        if (inputModel.PageSize < 1)
        {
            inputModel.PageSize = 10;
        }

        if (inputModel.PageSize > 100)
        {
            inputModel.PageSize = 100;
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
