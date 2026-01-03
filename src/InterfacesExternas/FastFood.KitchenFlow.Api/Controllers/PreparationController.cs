using System.Text.Json;
using FastFood.KitchenFlow.Api.Models.PreparationManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.KitchenFlow.Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Preparation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PreparationController : ControllerBase
{
    private readonly CreatePreparationUseCase _createPreparationUseCase;
    private readonly GetPreparationsUseCase _getPreparationsUseCase;
    private readonly StartPreparationUseCase _startPreparationUseCase;
    private readonly FinishPreparationUseCase _finishPreparationUseCase;

    /// <summary>
    /// Construtor que recebe os UseCases via Dependency Injection.
    /// </summary>
    /// <param name="createPreparationUseCase">UseCase para criação de preparações.</param>
    /// <param name="getPreparationsUseCase">UseCase para listagem de preparações.</param>
    /// <param name="startPreparationUseCase">UseCase para iniciar preparações.</param>
    /// <param name="finishPreparationUseCase">UseCase para finalizar preparações.</param>
    public PreparationController(
        CreatePreparationUseCase createPreparationUseCase,
        GetPreparationsUseCase getPreparationsUseCase,
        StartPreparationUseCase startPreparationUseCase,
        FinishPreparationUseCase finishPreparationUseCase)
    {
        _createPreparationUseCase = createPreparationUseCase;
        _getPreparationsUseCase = getPreparationsUseCase;
        _startPreparationUseCase = startPreparationUseCase;
        _finishPreparationUseCase = finishPreparationUseCase;
    }

    /// <summary>
    /// Cria uma nova preparação quando o pagamento for confirmado.
    /// </summary>
    /// <param name="request">Dados do pedido (OrderId e OrderSnapshot).</param>
    /// <returns>ApiResponse com os dados da preparação criada.</returns>
    /// <response code="201">Preparação criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="409">Preparação já existe para este pedido (idempotência).</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreatePreparationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePreparation([FromBody] CreatePreparationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CreatePreparationResponse>.Fail("Dados inválidos."));
        }

        try
        {
            // Mapear Request para InputModel
            var inputModel = new CreatePreparationInputModel
            {
                OrderId = request.OrderId,
                OrderSnapshot = request.OrderSnapshot
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _createPreparationUseCase.ExecuteAsync(inputModel);

            // Extrair o Id do Content para o CreatedAtAction
            Guid id = Guid.Empty;
            if (apiResponse.Content is Dictionary<string, object> contentDict &&
                contentDict.TryGetValue("createPreparation", out var contentObj))
            {
                var json = JsonSerializer.Serialize(contentObj);
                var response = JsonSerializer.Deserialize<CreatePreparationResponse>(json);
                id = response?.Id ?? Guid.Empty;
            }

            // Retornar HTTP 201 Created
            return CreatedAtAction(
                nameof(CreatePreparation),
                new { id },
                apiResponse);
        }
        catch (PreparationAlreadyExistsException ex)
        {
            return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail("Erro interno do servidor."));
        }
    }

    /// <summary>
    /// Lista preparações com paginação e filtro opcional por status.
    /// </summary>
    /// <param name="pageNumber">Número da página (default: 1).</param>
    /// <param name="pageSize">Tamanho da página (default: 10).</param>
    /// <param name="status">Filtro opcional por status (0=Received, 1=InProgress, 2=Finished).</param>
    /// <returns>ApiResponse com a lista paginada de preparações.</returns>
    /// <response code="200">Lista de preparações retornada com sucesso.</response>
    /// <response code="400">Parâmetros de paginação inválidos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetPreparationsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPreparations(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? status = null)
    {
        try
        {
            // Criar InputModel
            var inputModel = new GetPreparationsInputModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _getPreparationsUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(apiResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<GetPreparationsResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<GetPreparationsResponse>.Fail("Erro interno do servidor."));
        }
    }

    /// <summary>
    /// Inicia uma preparação (Received → InProgress).
    /// </summary>
    /// <param name="id">Identificador da preparação.</param>
    /// <returns>ApiResponse com os dados da preparação iniciada.</returns>
    /// <response code="200">Preparação iniciada com sucesso.</response>
    /// <response code="400">Status inválido para iniciar a preparação.</response>
    /// <response code="404">Preparação não encontrada.</response>
    [HttpPost("{id}/start")]
    [ProducesResponseType(typeof(ApiResponse<StartPreparationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartPreparation([FromRoute] Guid id)
    {
        try
        {
            // Criar InputModel
            var inputModel = new StartPreparationInputModel
            {
                Id = id
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _startPreparationUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(apiResponse);
        }
        catch (PreparationNotFoundException ex)
        {
            return NotFound(ApiResponse<StartPreparationResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StartPreparationResponse>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<StartPreparationResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<StartPreparationResponse>.Fail("Erro interno do servidor."));
        }
    }

    /// <summary>
    /// Finaliza uma preparação (InProgress → Finished).
    /// </summary>
    /// <param name="id">Identificador da preparação.</param>
    /// <returns>ApiResponse com os dados da preparação finalizada.</returns>
    /// <response code="200">Preparação finalizada com sucesso.</response>
    /// <response code="400">Status inválido para finalizar a preparação.</response>
    /// <response code="404">Preparação não encontrada.</response>
    [HttpPost("{id}/finish")]
    [ProducesResponseType(typeof(ApiResponse<FinishPreparationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinishPreparation([FromRoute] Guid id)
    {
        try
        {
            // Criar InputModel
            var inputModel = new FinishPreparationInputModel
            {
                Id = id
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _finishPreparationUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(apiResponse);
        }
        catch (PreparationNotFoundException ex)
        {
            return NotFound(ApiResponse<FinishPreparationResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<FinishPreparationResponse>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<FinishPreparationResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<FinishPreparationResponse>.Fail("Erro interno do servidor."));
        }
    }
}
