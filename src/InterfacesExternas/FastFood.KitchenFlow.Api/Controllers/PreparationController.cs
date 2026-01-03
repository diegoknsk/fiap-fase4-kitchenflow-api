using FastFood.KitchenFlow.Api.Models.PreparationManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
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

    /// <summary>
    /// Construtor que recebe o UseCase via Dependency Injection.
    /// </summary>
    /// <param name="createPreparationUseCase">UseCase para criação de preparações.</param>
    public PreparationController(CreatePreparationUseCase createPreparationUseCase)
    {
        _createPreparationUseCase = createPreparationUseCase;
    }

    /// <summary>
    /// Cria uma nova preparação quando o pagamento for confirmado.
    /// </summary>
    /// <param name="request">Dados do pedido (OrderId e OrderSnapshot).</param>
    /// <returns>Response com os dados da preparação criada.</returns>
    /// <response code="201">Preparação criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="409">Preparação já existe para este pedido (idempotência).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePreparationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePreparation([FromBody] CreatePreparationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Mapear Request para InputModel
            var inputModel = new CreatePreparationInputModel
            {
                OrderId = request.OrderId,
                OrderSnapshot = request.OrderSnapshot
            };

            // Chamar UseCase
            var response = await _createPreparationUseCase.ExecuteAsync(inputModel);

            // Retornar HTTP 201 Created
            return CreatedAtAction(
                nameof(CreatePreparation),
                new { id = response.Id },
                response);
        }
        catch (PreparationAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message, orderId = ex.OrderId });
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
