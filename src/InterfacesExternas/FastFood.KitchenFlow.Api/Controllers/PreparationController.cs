using System.Collections.Generic;
using FastFood.KitchenFlow.Api.Models.PreparationManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using Microsoft.AspNetCore.Authorization;
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
        try
        {
            var inputModel = new CreatePreparationInputModel
            {
                OrderId = request.OrderId,
                OrderSnapshot = request.OrderSnapshot
            };

            var result = await _createPreparationUseCase.ExecuteAsync(inputModel);
            if (result.Success)
            {
                // Extrair o ID do Content para usar no CreatedAtAction
                Guid? preparationId = null;
                if (result.Content is Dictionary<string, object> contentDict && 
                    contentDict.ContainsKey("createPreparation") &&
                    contentDict["createPreparation"] is CreatePreparationResponse response)
                {
                    preparationId = response.Id;
                }
                
                return CreatedAtAction(
                    nameof(GetPreparations), 
                    new { id = preparationId ?? Guid.Empty }, 
                    result);
            }
            return BadRequest(result);
        }
        catch (PreparationAlreadyExistsException ex)
        {
            return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
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
    //[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]
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
            var inputModel = new GetPreparationsInputModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status
            };

            var result = await _getPreparationsUseCase.ExecuteAsync(inputModel);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception)
        {
            return BadRequest(ApiResponse<GetPreparationsResponse>.Fail("Erro ao processar a requisição."));
        }
    }

    /// <summary>
    /// Inicia uma preparação (Received → InProgress).
    /// Busca automaticamente a preparação mais antiga com status Received e a inicia.
    /// Equivalente ao TakeNext do monolito.
    /// </summary>
    /// <returns>ApiResponse com os dados da preparação iniciada.</returns>
    /// <response code="200">Preparação iniciada com sucesso.</response>
    /// <response code="400">Status inválido para iniciar a preparação.</response>
    /// <response code="404">Nenhuma preparação disponível com status Received.</response>
    //[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]
    [HttpPost("take-next")]
    [ProducesResponseType(typeof(ApiResponse<StartPreparationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartPreparation()
    {
        try
        {
            var inputModel = new StartPreparationInputModel();
            var result = await _startPreparationUseCase.ExecuteAsync(inputModel);
            return result.Success ? Ok(result) : NotFound(result);
        }
        catch (PreparationNotFoundException)
        {
            return NotFound(ApiResponse<StartPreparationResponse>.Fail("Nenhuma preparação disponível com status Received."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StartPreparationResponse>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<StartPreparationResponse>.Fail(ex.Message));
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
    //[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]
    [HttpPost("{id}/finish")]
    [ProducesResponseType(typeof(ApiResponse<FinishPreparationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinishPreparation([FromRoute] Guid id)
    {
        try
        {
            var inputModel = new FinishPreparationInputModel
            {
                Id = id
            };

            var result = await _finishPreparationUseCase.ExecuteAsync(inputModel);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (PreparationNotFoundException ex)
        {
            return NotFound(ApiResponse<FinishPreparationResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<FinishPreparationResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return BadRequest(ApiResponse<FinishPreparationResponse>.Fail("Erro ao processar a requisição."));
        }
    }

}
