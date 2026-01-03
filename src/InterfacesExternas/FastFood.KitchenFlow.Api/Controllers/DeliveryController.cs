using FastFood.KitchenFlow.Api.Models.DeliveryManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
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
    /// <returns>Response com os dados da entrega criada.</returns>
    /// <response code="201">Entrega criada com sucesso.</response>
    /// <response code="400">Dados inválidos, Preparation não encontrada ou não está finalizada.</response>
    /// <response code="409">Entrega já existe para esta Preparation (idempotência).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateDeliveryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Mapear Request para InputModel
            var inputModel = new CreateDeliveryInputModel
            {
                PreparationId = request.PreparationId,
                OrderId = request.OrderId
            };

            // Chamar UseCase
            var response = await _createDeliveryUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 201 Created
            return CreatedAtAction(
                nameof(CreateDelivery),
                new { id = response.Id },
                response);
        }
        catch (DeliveryAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message, preparationId = ex.PreparationId });
        }
        catch (PreparationNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message, preparationId = ex.PreparationId });
        }
        catch (PreparationNotFinishedException ex)
        {
            return BadRequest(new { message = ex.Message, preparationId = ex.PreparationId, currentStatus = ex.CurrentStatus });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Lista entregas prontas para retirada (status ReadyForPickup) com paginação.
    /// </summary>
    /// <param name="pageNumber">Número da página (default: 1).</param>
    /// <param name="pageSize">Tamanho da página (default: 10).</param>
    /// <returns>Response com a lista de entregas prontas para retirada.</returns>
    /// <response code="200">Lista de entregas prontas para retirada.</response>
    [HttpGet("ready")]
    [ProducesResponseType(typeof(GetReadyDeliveriesResponse), StatusCodes.Status200OK)]
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

            // Chamar UseCase
            var response = await _getReadyDeliveriesUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Finaliza uma entrega (ReadyForPickup → Finalized).
    /// </summary>
    /// <param name="id">Identificador da entrega a ser finalizada.</param>
    /// <returns>Response com os dados da entrega finalizada.</returns>
    /// <response code="200">Entrega finalizada com sucesso.</response>
    /// <response code="400">Entrega não encontrada ou status inválido.</response>
    /// <response code="404">Entrega não encontrada.</response>
    [HttpPost("{id}/finalize")]
    [ProducesResponseType(typeof(FinalizeDeliveryResponse), StatusCodes.Status200OK)]
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

            // Chamar UseCase
            var response = await _finalizeDeliveryUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 200 OK
            return Ok(response);
        }
        catch (DeliveryNotFoundException ex)
        {
            return NotFound(new { message = ex.Message, deliveryId = ex.DeliveryId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }
}
