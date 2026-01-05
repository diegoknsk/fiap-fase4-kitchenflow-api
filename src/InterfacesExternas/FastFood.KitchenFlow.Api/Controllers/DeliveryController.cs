using FastFood.KitchenFlow.Api.Models.DeliveryManagement;
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
        try
        {
            var inputModel = new CreateDeliveryInputModel
            {
                PreparationId = request.PreparationId,
                OrderId = request.OrderId
            };

            var result = await _createDeliveryUseCase.ExecuteAsync(inputModel);
            return result.Success ? StatusCode(201, result) : BadRequest(result);
        }
        catch (Exception)
        {
            return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail("Erro ao processar a requisição."));
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
            var inputModel = new GetReadyDeliveriesInputModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _getReadyDeliveriesUseCase.ExecuteAsync(inputModel);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception)
        {
            return BadRequest(ApiResponse<GetReadyDeliveriesResponse>.Fail("Erro ao processar a requisição."));
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
            var inputModel = new FinalizeDeliveryInputModel
            {
                Id = id
            };

            var result = await _finalizeDeliveryUseCase.ExecuteAsync(inputModel);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception)
        {
            return BadRequest(ApiResponse<FinalizeDeliveryResponse>.Fail("Erro ao processar a requisição."));
        }
    }

}
