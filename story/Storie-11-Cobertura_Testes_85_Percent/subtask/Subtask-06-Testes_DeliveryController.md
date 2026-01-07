# Subtask 06: Criar testes para DeliveryController

## Descrição
Criar testes unitários para o `DeliveryController`, focando na validação do contrato HTTP (mapeamento de request/response, tratamento de exceções, códigos HTTP corretos), sem testar a lógica de negócio (que já está testada nos UseCases).

## Passos de implementação
- Criar arquivo `InterfacesExternas/Controllers/DeliveryControllerTests.cs`
- Implementar testes para cada endpoint do controller
- Mockar UseCases (não testar lógica de negócio)
- Testar mapeamento de Request → InputModel → UseCase → Response
- Testar tratamento de exceções e códigos HTTP
- Usar Moq para mockar UseCases
- Usar FluentAssertions para assertions

## Endpoints a testar

### POST /api/deliveries
1. **CreateDelivery_WhenValidRequest_ShouldReturn201Created**
   - Arrange: Request válido, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 201, Response não nulo

2. **CreateDelivery_WhenModelStateInvalid_ShouldReturn400BadRequest**
   - Arrange: Request inválido (ModelState inválido)
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

3. **CreateDelivery_WhenPreparationNotFound_ShouldReturn404NotFound**
   - Arrange: UseCase lança PreparationNotFoundException
   - Act: Chamar endpoint
   - Assert: StatusCode = 404

4. **CreateDelivery_WhenPreparationNotFinished_ShouldReturn400BadRequest**
   - Arrange: UseCase lança PreparationNotFinishedException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

5. **CreateDelivery_WhenDeliveryAlreadyExists_ShouldReturn409Conflict**
   - Arrange: UseCase lança DeliveryAlreadyExistsException
   - Act: Chamar endpoint
   - Assert: StatusCode = 409

6. **CreateDelivery_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

### GET /api/deliveries/ready
1. **GetReadyDeliveries_WhenValidRequest_ShouldReturn200OK**
   - Arrange: Parâmetros válidos, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 200, Response não nulo

2. **GetReadyDeliveries_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

### POST /api/deliveries/{id}/finalize
1. **FinalizeDelivery_WhenValidRequest_ShouldReturn200OK**
   - Arrange: ID válido, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 200, Response não nulo

2. **FinalizeDelivery_WhenDeliveryNotFound_ShouldReturn404NotFound**
   - Arrange: UseCase lança DeliveryNotFoundException
   - Act: Chamar endpoint
   - Assert: StatusCode = 404

3. **FinalizeDelivery_WhenInvalidOperationException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança InvalidOperationException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

4. **FinalizeDelivery_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

## Exemplo de teste

```csharp
using FastFood.KitchenFlow.Api.Controllers;
using FastFood.KitchenFlow.Api.Models.DeliveryManagement;
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.Models.Common;
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

    // ... outros testes
}
```

## Observações importantes
- **Padrão ApiResponse<T>**: UseCases retornam `ApiResponse<T>`, não Response models simples
- **Foco no contrato HTTP**: Testar apenas mapeamento e códigos HTTP, não lógica de negócio
- **Mocks de UseCases**: Sempre mockar UseCases retornando `ApiResponse<T>`, não testar lógica
- **ModelState**: Testar validação de ModelState (retorna `ApiResponse<T>.Fail()`)
- **Exceções**: Testar tratamento de todas as exceções possíveis (Controller cria `ApiResponse<T>.Fail()`)
- **Códigos HTTP**: Verificar que os códigos HTTP corretos são retornados
- **Estrutura ApiResponse**: Verificar `Success`, `Message` e `Content` nas assertions
- **ToNamedContent**: O `Content` vem formatado com ToNamedContent (Dictionary<string, object>)
- **Nota sobre PreparationNotFound**: No DeliveryController, PreparationNotFound retorna 400 (BadRequest), não 404

## Como testar
- Executar `dotnet test` no projeto de testes
- Verificar que todos os testes passam
- Executar `dotnet test --collect:"XPlat Code Coverage"` para verificar cobertura
- Verificar que o Controller tem cobertura adequada

## Critérios de aceite
- Arquivo de teste criado para DeliveryController
- Todos os 3 endpoints testados
- Todos os cenários de cada endpoint testados (sucesso, erros, exceções)
- Testes seguem padrão AAA
- Testes usam Moq para mockar UseCases
- Testes usam FluentAssertions para assertions
- Todos os testes passam
- Cobertura de código para DeliveryController >= 85%
