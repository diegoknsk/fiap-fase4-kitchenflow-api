using FastFood.KitchenFlow.Api.Controllers;
using FastFood.KitchenFlow.Api.Models.DeliveryManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.InterfacesExternas.Controllers;

public class DeliveryControllerTests
{
    private readonly Mock<CreateDeliveryUseCase> _mockCreateUseCase;
    private readonly Mock<GetReadyDeliveriesUseCase> _mockGetReadyUseCase;
    private readonly Mock<FinalizeDeliveryUseCase> _mockFinalizeUseCase;
    private readonly DeliveryController _controller;

    public DeliveryControllerTests()
    {
        _mockCreateUseCase = new Mock<CreateDeliveryUseCase>(
            Mock.Of<IDeliveryRepository>(),
            Mock.Of<IPreparationRepository>());
        _mockGetReadyUseCase = new Mock<GetReadyDeliveriesUseCase>(
            Mock.Of<IDeliveryRepository>());
        _mockFinalizeUseCase = new Mock<FinalizeDeliveryUseCase>(
            Mock.Of<IDeliveryRepository>());
        
        _controller = new DeliveryController(
            _mockCreateUseCase.Object,
            _mockGetReadyUseCase.Object,
            _mockFinalizeUseCase.Object);
    }

    [Fact]
    public async Task CreateDelivery_WhenValidRequest_ShouldReturn201Created()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var request = new CreateDeliveryRequest
        {
            PreparationId = preparationId
        };

        var response = new CreateDeliveryResponse
        {
            Id = Guid.NewGuid(),
            PreparationId = preparationId,
            OrderId = Guid.NewGuid(),
            Status = 0,
            CreatedAt = DateTime.UtcNow
        };

        var apiResponse = ApiResponse<CreateDeliveryResponse>.Ok(response, "Entrega criada com sucesso.");

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreateDeliveryInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.StatusCode.Should().Be(201);
        createdAtResult.Value.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        var returnedApiResponse = createdAtResult.Value as ApiResponse<CreateDeliveryResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Entrega criada com sucesso.");
        returnedApiResponse.Content.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateDelivery_WhenModelStateInvalid_ShouldReturn400BadRequest()
    {
        // Arrange
        var request = new CreateDeliveryRequest();
        _controller.ModelState.AddModelError("PreparationId", "PreparationId é obrigatório");

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        var apiResponse = badRequestResult.Value as ApiResponse<CreateDeliveryResponse>;
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Message.Should().Be("Dados inválidos.");
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationNotFound_ShouldReturn400BadRequest()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var request = new CreateDeliveryRequest
        {
            PreparationId = preparationId
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreateDeliveryInputModel>()))
            .ThrowsAsync(new PreparationNotFoundException(preparationId));

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        var apiResponse = badRequestResult.Value as ApiResponse<CreateDeliveryResponse>;
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationNotFinished_ShouldReturn400BadRequest()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var request = new CreateDeliveryRequest
        {
            PreparationId = preparationId
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreateDeliveryInputModel>()))
            .ThrowsAsync(new PreparationNotFinishedException(preparationId, 1));

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        var apiResponse = badRequestResult.Value as ApiResponse<CreateDeliveryResponse>;
        apiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDelivery_WhenDeliveryAlreadyExists_ShouldReturn409Conflict()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var request = new CreateDeliveryRequest
        {
            PreparationId = preparationId
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreateDeliveryInputModel>()))
            .ThrowsAsync(new DeliveryAlreadyExistsException(preparationId));

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = result as ConflictObjectResult;
        conflictResult!.StatusCode.Should().Be(409);
        conflictResult.Value.Should().BeOfType<ApiResponse<CreateDeliveryResponse>>();
        var apiResponse = conflictResult.Value as ApiResponse<CreateDeliveryResponse>;
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Message.Should().Contain("já existe");
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenValidRequest_ShouldReturn200OK()
    {
        // Arrange
        var response = new GetReadyDeliveriesResponse
        {
            Items = new List<DeliveryItemResponse>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        var apiResponse = ApiResponse<GetReadyDeliveriesResponse>.Ok(response, "Lista de entregas prontas para retirada retornada com sucesso.");

        _mockGetReadyUseCase.Setup(u => u.ExecuteAsync(It.IsAny<GetReadyDeliveriesInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.GetReadyDeliveries(1, 10);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ApiResponse<GetReadyDeliveriesResponse>>();
        var returnedApiResponse = okResult.Value as ApiResponse<GetReadyDeliveriesResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Lista de entregas prontas para retirada retornada com sucesso.");
    }

    [Fact]
    public async Task FinalizeDelivery_WhenValidRequest_ShouldReturn200OK()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var response = new FinalizeDeliveryResponse
        {
            Id = deliveryId,
            PreparationId = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            Status = 1,
            CreatedAt = DateTime.UtcNow,
            FinalizedAt = DateTime.UtcNow
        };

        var apiResponse = ApiResponse<FinalizeDeliveryResponse>.Ok(response, "Entrega finalizada com sucesso.");

        _mockFinalizeUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinalizeDeliveryInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.FinalizeDelivery(deliveryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ApiResponse<FinalizeDeliveryResponse>>();
        var returnedApiResponse = okResult.Value as ApiResponse<FinalizeDeliveryResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Entrega finalizada com sucesso.");
    }

    [Fact]
    public async Task FinalizeDelivery_WhenDeliveryNotFound_ShouldReturn404NotFound()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();

        _mockFinalizeUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinalizeDeliveryInputModel>()))
            .ThrowsAsync(new DeliveryNotFoundException(deliveryId));

        // Act
        var result = await _controller.FinalizeDelivery(deliveryId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().BeOfType<ApiResponse<FinalizeDeliveryResponse>>();
        var apiResponse = notFoundResult.Value as ApiResponse<FinalizeDeliveryResponse>;
        apiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task FinalizeDelivery_WhenInvalidOperationException_ShouldReturn400BadRequest()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();

        _mockFinalizeUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinalizeDeliveryInputModel>()))
            .ThrowsAsync(new InvalidOperationException("Status inválido"));

        // Act
        var result = await _controller.FinalizeDelivery(deliveryId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }
}
