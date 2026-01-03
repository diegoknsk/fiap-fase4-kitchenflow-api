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
            CreatedAt = DateTime.UtcNow,
            Message = "Entrega criada com sucesso"
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreateDeliveryInputModel>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateDelivery(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.StatusCode.Should().Be(201);
        createdAtResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationNotFound_ShouldReturn404NotFound()
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
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(404);
    }

    // ... outros testes
}
```

## Observações importantes
- **Foco no contrato HTTP**: Testar apenas mapeamento e códigos HTTP, não lógica de negócio
- **Mocks de UseCases**: Sempre mockar UseCases, não testar lógica
- **ModelState**: Testar validação de ModelState
- **Exceções**: Testar tratamento de todas as exceções possíveis
- **Códigos HTTP**: Verificar que os códigos HTTP corretos são retornados

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
