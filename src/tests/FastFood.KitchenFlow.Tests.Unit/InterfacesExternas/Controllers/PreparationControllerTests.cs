using FastFood.KitchenFlow.Api.Controllers;
using FastFood.KitchenFlow.Api.Models.PreparationManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.InterfacesExternas.Controllers;

public class PreparationControllerTests
{
    private readonly Mock<CreatePreparationUseCase> _mockCreateUseCase;
    private readonly Mock<GetPreparationsUseCase> _mockGetUseCase;
    private readonly Mock<StartPreparationUseCase> _mockStartUseCase;
    private readonly Mock<FinishPreparationUseCase> _mockFinishUseCase;
    private readonly PreparationController _controller;

    public PreparationControllerTests()
    {
        _mockCreateUseCase = new Mock<CreatePreparationUseCase>(Mock.Of<IPreparationRepository>());
        _mockGetUseCase = new Mock<GetPreparationsUseCase>(Mock.Of<IPreparationRepository>());
        _mockStartUseCase = new Mock<StartPreparationUseCase>(Mock.Of<IPreparationRepository>());
        _mockFinishUseCase = new Mock<FinishPreparationUseCase>(
            Mock.Of<IPreparationRepository>(),
            Mock.Of<IDeliveryRepository>());
        
        _controller = new PreparationController(
            _mockCreateUseCase.Object,
            _mockGetUseCase.Object,
            _mockStartUseCase.Object,
            _mockFinishUseCase.Object);
    }

    [Fact]
    public async Task CreatePreparation_WhenValidRequest_ShouldReturn201Created()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var request = new CreatePreparationRequest
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        var response = new CreatePreparationResponse
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Status = 0,
            CreatedAt = DateTime.UtcNow
        };

        var apiResponse = ApiResponse<CreatePreparationResponse>.Ok(response, "Preparação criada com sucesso.");

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreatePreparationInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.StatusCode.Should().Be(201);
        createdAtResult.Value.Should().BeOfType<ApiResponse<CreatePreparationResponse>>();
        var returnedApiResponse = createdAtResult.Value as ApiResponse<CreatePreparationResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Preparação criada com sucesso.");
        returnedApiResponse.Content.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePreparation_WhenModelStateInvalid_ShouldReturn400BadRequest()
    {
        // Arrange
        var request = new CreatePreparationRequest();
        _controller.ModelState.AddModelError("OrderId", "OrderId é obrigatório");

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ApiResponse<CreatePreparationResponse>>();
        var apiResponse = badRequestResult.Value as ApiResponse<CreatePreparationResponse>;
        apiResponse!.Success.Should().BeFalse();
        // O controller não valida ModelState, então retorna erro genérico do catch
    }

    [Fact]
    public async Task CreatePreparation_WhenPreparationAlreadyExists_ShouldReturn409Conflict()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = $$"""{"orderId":"{{orderId}}","orderCode":"ORD-001","totalPrice":50.00,"createdAt":"2024-01-01T00:00:00Z","items":[{"productId":"{{Guid.NewGuid()}}","quantity":1,"price":50.00}],"paymentId":"{{Guid.NewGuid()}}","paidAt":"2024-01-01T00:00:00Z"}""";
        var request = new CreatePreparationRequest
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreatePreparationInputModel>()))
            .ThrowsAsync(new PreparationAlreadyExistsException(orderId));

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = result as ConflictObjectResult;
        conflictResult!.StatusCode.Should().Be(409);
        conflictResult.Value.Should().BeOfType<ApiResponse<CreatePreparationResponse>>();
        var apiResponse = conflictResult.Value as ApiResponse<CreatePreparationResponse>;
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Message.Should().ContainEquivalentOf("já existe");
    }

    [Fact]
    public async Task CreatePreparation_WhenArgumentException_ShouldReturn400BadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var request = new CreatePreparationRequest
        {
            OrderId = orderId,
            OrderSnapshot = "invalid json"
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreatePreparationInputModel>()))
            .ThrowsAsync(new ArgumentException("OrderSnapshot contém JSON inválido"));

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ApiResponse<CreatePreparationResponse>>();
        var apiResponse = badRequestResult.Value as ApiResponse<CreatePreparationResponse>;
        apiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task GetPreparations_WhenValidRequest_ShouldReturn200OK()
    {
        // Arrange
        var response = new GetPreparationsResponse
        {
            Items = new List<PreparationItemResponse>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        var apiResponse = ApiResponse<GetPreparationsResponse>.Ok(response, "Lista de preparações retornada com sucesso.");

        _mockGetUseCase.Setup(u => u.ExecuteAsync(It.IsAny<GetPreparationsInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.GetPreparations(1, 10, null);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ApiResponse<GetPreparationsResponse>>();
        var returnedApiResponse = okResult.Value as ApiResponse<GetPreparationsResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Lista de preparações retornada com sucesso.");
    }

    [Fact]
    public async Task GetPreparations_WhenArgumentException_ShouldReturn400BadRequest()
    {
        // Arrange
        _mockGetUseCase.Setup(u => u.ExecuteAsync(It.IsAny<GetPreparationsInputModel>()))
            .ThrowsAsync(new ArgumentException("PageNumber deve ser maior ou igual a 1"));

        // Act
        var result = await _controller.GetPreparations(0, 10, null);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task StartPreparation_WhenValidRequest_ShouldReturn200OK()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var response = new StartPreparationResponse
        {
            Id = preparationId,
            OrderId = Guid.NewGuid(),
            Status = 1,
            CreatedAt = DateTime.UtcNow
        };

        var apiResponse = ApiResponse<StartPreparationResponse>.Ok(response, "Preparação iniciada com sucesso.");

        _mockStartUseCase.Setup(u => u.ExecuteAsync(It.IsAny<StartPreparationInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.StartPreparation();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ApiResponse<StartPreparationResponse>>();
        var returnedApiResponse = okResult.Value as ApiResponse<StartPreparationResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Preparação iniciada com sucesso.");
    }

    [Fact]
    public async Task StartPreparation_WhenPreparationNotFound_ShouldReturn404NotFound()
    {
        // Arrange
        _mockStartUseCase.Setup(u => u.ExecuteAsync(It.IsAny<StartPreparationInputModel>()))
            .ThrowsAsync(new PreparationNotFoundException(Guid.Empty));

        // Act
        var result = await _controller.StartPreparation();

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().BeOfType<ApiResponse<StartPreparationResponse>>();
        var apiResponse = notFoundResult.Value as ApiResponse<StartPreparationResponse>;
        apiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task StartPreparation_WhenInvalidOperationException_ShouldReturn400BadRequest()
    {
        // Arrange
        _mockStartUseCase.Setup(u => u.ExecuteAsync(It.IsAny<StartPreparationInputModel>()))
            .ThrowsAsync(new InvalidOperationException("Status inválido"));

        // Act
        var result = await _controller.StartPreparation();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task FinishPreparation_WhenValidRequest_ShouldReturn200OK()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var response = new FinishPreparationResponse
        {
            Id = preparationId,
            OrderId = Guid.NewGuid(),
            Status = 2,
            CreatedAt = DateTime.UtcNow
        };

        var apiResponse = ApiResponse<FinishPreparationResponse>.Ok(response, "Preparação finalizada com sucesso.");

        _mockFinishUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinishPreparationInputModel>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _controller.FinishPreparation(preparationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ApiResponse<FinishPreparationResponse>>();
        var returnedApiResponse = okResult.Value as ApiResponse<FinishPreparationResponse>;
        returnedApiResponse!.Success.Should().BeTrue();
        returnedApiResponse.Message.Should().Be("Preparação finalizada com sucesso.");
    }

    [Fact]
    public async Task FinishPreparation_WhenPreparationNotFound_ShouldReturn404NotFound()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        _mockFinishUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinishPreparationInputModel>()))
            .ThrowsAsync(new PreparationNotFoundException(preparationId));

        // Act
        var result = await _controller.FinishPreparation(preparationId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().BeOfType<ApiResponse<FinishPreparationResponse>>();
        var apiResponse = notFoundResult.Value as ApiResponse<FinishPreparationResponse>;
        apiResponse!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task FinishPreparation_WhenInvalidOperationException_ShouldReturn400BadRequest()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        _mockFinishUseCase.Setup(u => u.ExecuteAsync(It.IsAny<FinishPreparationInputModel>()))
            .ThrowsAsync(new InvalidOperationException("Status inválido"));

        // Act
        var result = await _controller.FinishPreparation(preparationId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }
}
