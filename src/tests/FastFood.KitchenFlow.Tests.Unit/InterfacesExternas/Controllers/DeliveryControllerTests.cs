using FastFood.KitchenFlow.Api.Controllers;
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
    private readonly Mock<GetReadyDeliveriesUseCase> _mockGetReadyUseCase;
    private readonly Mock<FinalizeDeliveryUseCase> _mockFinalizeUseCase;
    private readonly DeliveryController _controller;

    public DeliveryControllerTests()
    {
        _mockGetReadyUseCase = new Mock<GetReadyDeliveriesUseCase>(
            Mock.Of<IDeliveryRepository>());
        _mockFinalizeUseCase = new Mock<FinalizeDeliveryUseCase>(
            Mock.Of<IDeliveryRepository>());
        
        _controller = new DeliveryController(
            _mockGetReadyUseCase.Object,
            _mockFinalizeUseCase.Object);
    }

    // Nota: Os testes do método CreateDelivery foram removidos pois o endpoint foi removido na Story 14.
    // O delivery agora é criado automaticamente quando a preparação é finalizada.

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

    [Fact]
    public async Task GetReadyDeliveries_WhenUseCaseReturnsFailure_ShouldReturn400BadRequest()
    {
        // Arrange
        var apiResponse = ApiResponse<GetReadyDeliveriesResponse>.Fail("Erro ao buscar entregas");
        _mockGetReadyUseCase.Setup(u => u.ExecuteAsync(It.IsAny<GetReadyDeliveriesInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.GetReadyDeliveries(1, 10);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        var returnedApiResponse = badRequestResult.Value as ApiResponse<GetReadyDeliveriesResponse>;
        returnedApiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task GetReadyDeliveries_WhenGenericException_ShouldReturn400BadRequest()
    {
        // Arrange
        _mockGetReadyUseCase.Setup(u => u.ExecuteAsync(It.IsAny<GetReadyDeliveriesInputModel>()))
            .ThrowsAsync(new Exception("Erro genérico"));

        // Act
        var result = await _controller.GetReadyDeliveries(1, 10);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        var returnedApiResponse = badRequestResult.Value as ApiResponse<GetReadyDeliveriesResponse>;
        returnedApiResponse!.Success.Should().BeFalse();
        returnedApiResponse.Message.Should().Be("Erro ao processar a requisição.");
    }

    [Fact]
    public async Task FinalizeDelivery_WhenUseCaseReturnsFailure_ShouldReturn400BadRequest()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var apiResponse = ApiResponse<FinalizeDeliveryResponse>.Fail("Erro ao finalizar entrega");
        _mockFinalizeUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinalizeDeliveryInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.FinalizeDelivery(deliveryId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        var returnedApiResponse = badRequestResult.Value as ApiResponse<FinalizeDeliveryResponse>;
        returnedApiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task FinalizeDelivery_WhenGenericException_ShouldReturn400BadRequest()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        _mockFinalizeUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinalizeDeliveryInputModel>()))
            .ThrowsAsync(new Exception("Erro genérico"));

        // Act
        var result = await _controller.FinalizeDelivery(deliveryId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        var returnedApiResponse = badRequestResult.Value as ApiResponse<FinalizeDeliveryResponse>;
        returnedApiResponse!.Success.Should().BeFalse();
    }
}
