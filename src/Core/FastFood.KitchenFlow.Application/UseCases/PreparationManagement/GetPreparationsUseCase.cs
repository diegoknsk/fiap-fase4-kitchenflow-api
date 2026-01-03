using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;

namespace FastFood.KitchenFlow.Application.UseCases.PreparationManagement;

/// <summary>
/// UseCase responsável por listar preparações com paginação e filtro opcional por status.
/// </summary>
public class GetPreparationsUseCase
{
    private readonly IPreparationRepository _repository;

    /// <summary>
    /// Construtor que recebe o repository via Dependency Injection.
    /// </summary>
    /// <param name="repository">Repository para acesso a dados de Preparation.</param>
    public GetPreparationsUseCase(IPreparationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Executa a listagem de preparações com paginação.
    /// </summary>
    /// <param name="inputModel">Dados de entrada para listagem (paginação e filtros).</param>
    /// <returns>ApiResponse com a lista paginada de preparações.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados de entrada são inválidos.</exception>
    public async Task<ApiResponse<GetPreparationsResponse>> ExecuteAsync(GetPreparationsInputModel inputModel)
    {
        // Validar InputModel
        if (inputModel.PageNumber < 1)
        {
            throw new ArgumentException("PageNumber deve ser maior ou igual a 1.", nameof(inputModel));
        }

        if (inputModel.PageSize < 1 || inputModel.PageSize > 100)
        {
            throw new ArgumentException("PageSize deve estar entre 1 e 100.", nameof(inputModel));
        }

        // Buscar preparações com paginação
        var (preparations, totalCount) = await _repository.GetPagedAsync(
            inputModel.PageNumber,
            inputModel.PageSize,
            inputModel.Status);

        // Mapear para OutputModel
        var items = preparations.Select(p => new PreparationItemOutputModel
        {
            Id = p.Id,
            OrderId = p.OrderId,
            Status = (int)p.Status,
            CreatedAt = p.CreatedAt,
            OrderSnapshot = p.OrderSnapshot
        }).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)inputModel.PageSize);

        var outputModel = new GetPreparationsOutputModel
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = inputModel.PageNumber,
            PageSize = inputModel.PageSize,
            TotalPages = totalPages
        };

        // Chamar Presenter e retornar Response
        return GetPreparationsPresenter.Present(outputModel);
    }
}
