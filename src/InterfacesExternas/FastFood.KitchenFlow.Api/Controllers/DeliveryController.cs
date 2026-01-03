using System.Text.Json;
using FastFood.KitchenFlow.Api.Models.DeliveryManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.KitchenFlow.Api.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Delivery.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly CreateDeliveryUseCase _createDeliveryUseCase;
    private readonly GetReadyDeliveriesUseCase _getReadyDeliveriesUseCase;
    private readonly FinalizeDeliveryUseCase _finalizeDeliveryUseCase;

    /// <summary>
    /// Construtor que recebe os UseCases via Dependency Injection.
    /// </summary>
    /// <param name="createDeliveryUseCase">UseCase para criação de entregas.</param>
    /// <param name="getReadyDeliveriesUseCase">UseCase para listagem de entregas prontas.</param>
    /// <param name="finalizeDeliveryUseCase">UseCase para finalização de entregas.</param>
    public DeliveryController(
        CreateDeliveryUseCase createDeliveryUseCase,
        GetReadyDeliveriesUseCase getReadyDeliveriesUseCase,
        FinalizeDeliveryUseCase finalizeDeliveryUseCase)
    {
        _createDeliveryUseCase = createDeliveryUseCase;
        _getReadyDeliveriesUseCase = getReadyDeliveriesUseCase;
        _finalizeDeliveryUseCase = finalizeDeliveryUseCase;
    }

    /// <summary>
    /// Cria uma nova entrega quando uma preparação for finalizada.
    /// </summary>
    /// <param name="request">Dados da entrega (PreparationId e OrderId opcional).</param>
    /// <returns>ApiResponse com os dados da entrega criada.</returns>
    /// <response code="201">Entrega criada com sucesso.</response>
    /// <response code="400">Dados inválidos, Preparation não encontrada ou não está finalizada.</response>
    /// <response code="409">Entrega já existe para esta Preparation (idempotência).</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateDeliveryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail("Dados inválidos."));
        }

        try
        {
            // Mapear Request para InputModel
            var inputModel = new CreateDeliveryInputModel
            {
                PreparationId = request.PreparationId,
                OrderId = request.OrderId
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _createDeliveryUseCase.ExecuteAsync(inputModel);

            // Extrair o Id do Content para o CreatedAtAction
            Guid id = Guid.Empty;
            if (apiResponse.Content is Dictionary<string, object> contentDict &&
                contentDict.TryGetValue("createDelivery", out var contentObj))
            {
                var json = JsonSerializer.Serialize(contentObj);
                var response = JsonSerializer.Deserialize<CreateDeliveryResponse>(json);
                id = response?.Id ?? Guid.Empty;
            }

            // Retornar HTTP 201 Created
            return CreatedAtAction(
                nameof(CreateDelivery),
                new { id },
                apiResponse);
        }
        catch (DeliveryAlreadyExistsException ex)
        {
            return Conflict(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
        }
        catch (PreparationNotFoundException ex)
        {
            return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
        }
        catch (PreparationNotFinishedException ex)
        {
            return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<CreateDeliveryResponse>.Fail("Erro interno do servidor."));
        }
    }

    /// <summary>
    /// Lista entregas prontas para retirada (status ReadyForPickup) com paginação.
    /// </summary>
    /// <param name="pageNumber">Número da página (default: 1).</param>
    /// <param name="pageSize">Tamanho da página (default: 10).</param>
    /// <returns>ApiResponse com a lista de entregas prontas para retirada.</returns>
    /// <response code="200">Lista de entregas prontas para retirada.</response>
    [HttpGet("ready")]
    [ProducesResponseType(typeof(ApiResponse<GetReadyDeliveriesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadyDeliveries(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            // Criar InputModel
            var inputModel = new GetReadyDeliveriesInputModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _getReadyDeliveriesUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(apiResponse);
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<GetReadyDeliveriesResponse>.Fail("Erro interno do servidor."));
        }
    }

    /// <summary>
    /// Finaliza uma entrega (ReadyForPickup → Finalized).
    /// </summary>
    /// <param name="id">Identificador da entrega a ser finalizada.</param>
    /// <returns>ApiResponse com os dados da entrega finalizada.</returns>
    /// <response code="200">Entrega finalizada com sucesso.</response>
    /// <response code="400">Entrega não encontrada ou status inválido.</response>
    /// <response code="404">Entrega não encontrada.</response>
    [HttpPost("{id}/finalize")]
    [ProducesResponseType(typeof(ApiResponse<FinalizeDeliveryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinalizeDelivery([FromRoute] Guid id)
    {
        try
        {
            // Criar InputModel
            var inputModel = new FinalizeDeliveryInputModel
            {
                Id = id
            };

            // Chamar UseCase (já retorna ApiResponse<T>)
            var apiResponse = await _finalizeDeliveryUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(apiResponse);
        }
        catch (DeliveryNotFoundException ex)
        {
            return NotFound(ApiResponse<FinalizeDeliveryResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<FinalizeDeliveryResponse>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<FinalizeDeliveryResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<FinalizeDeliveryResponse>.Fail("Erro interno do servidor."));
        }
    }
}
