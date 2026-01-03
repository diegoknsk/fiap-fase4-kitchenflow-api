# Subtask 05: Criar testes para PreparationController

## Descrição
Criar testes unitários para o `PreparationController`, focando na validação do contrato HTTP (mapeamento de request/response, tratamento de exceções, códigos HTTP corretos), sem testar a lógica de negócio (que já está testada nos UseCases).

## Passos de implementação
- Criar arquivo `InterfacesExternas/Controllers/PreparationControllerTests.cs`
- Implementar testes para cada endpoint do controller
- Mockar UseCases (não testar lógica de negócio)
- Testar mapeamento de Request → InputModel → UseCase → Response
- Testar tratamento de exceções e códigos HTTP
- Usar Moq para mockar UseCases
- Usar FluentAssertions para assertions

## Endpoints a testar

### POST /api/preparations
1. **CreatePreparation_WhenValidRequest_ShouldReturn201Created**
   - Arrange: Request válido, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 201, Response não nulo

2. **CreatePreparation_WhenModelStateInvalid_ShouldReturn400BadRequest**
   - Arrange: Request inválido (ModelState inválido)
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

3. **CreatePreparation_WhenPreparationAlreadyExists_ShouldReturn409Conflict**
   - Arrange: UseCase lança PreparationAlreadyExistsException
   - Act: Chamar endpoint
   - Assert: StatusCode = 409, mensagem de conflito

4. **CreatePreparation_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

5. **CreatePreparation_WhenUnexpectedException_ShouldReturn500InternalServerError**
   - Arrange: UseCase lança Exception genérica
   - Act: Chamar endpoint
   - Assert: StatusCode = 500

### GET /api/preparations
1. **GetPreparations_WhenValidRequest_ShouldReturn200OK**
   - Arrange: Parâmetros válidos, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 200, Response não nulo

2. **GetPreparations_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

3. **GetPreparations_WhenUnexpectedException_ShouldReturn500InternalServerError**
   - Arrange: UseCase lança Exception genérica
   - Act: Chamar endpoint
   - Assert: StatusCode = 500

### POST /api/preparations/{id}/start
1. **StartPreparation_WhenValidRequest_ShouldReturn200OK**
   - Arrange: ID válido, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 200, Response não nulo

2. **StartPreparation_WhenPreparationNotFound_ShouldReturn404NotFound**
   - Arrange: UseCase lança PreparationNotFoundException
   - Act: Chamar endpoint
   - Assert: StatusCode = 404

3. **StartPreparation_WhenInvalidOperationException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança InvalidOperationException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

4. **StartPreparation_WhenArgumentException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança ArgumentException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

### POST /api/preparations/{id}/finish
1. **FinishPreparation_WhenValidRequest_ShouldReturn200OK**
   - Arrange: ID válido, UseCase mockado retorna response
   - Act: Chamar endpoint
   - Assert: StatusCode = 200, Response não nulo

2. **FinishPreparation_WhenPreparationNotFound_ShouldReturn404NotFound**
   - Arrange: UseCase lança PreparationNotFoundException
   - Act: Chamar endpoint
   - Assert: StatusCode = 404

3. **FinishPreparation_WhenInvalidOperationException_ShouldReturn400BadRequest**
   - Arrange: UseCase lança InvalidOperationException
   - Act: Chamar endpoint
   - Assert: StatusCode = 400

## Exemplo de teste

```csharp
using FastFood.KitchenFlow.Api.Controllers;
using FastFood.KitchenFlow.Api.Models.PreparationManagement;
using FastFood.KitchenFlow.Application.Exceptions;
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
        _mockFinishUseCase = new Mock<FinishPreparationUseCase>(Mock.Of<IPreparationRepository>());
        
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
        var request = new CreatePreparationRequest
        {
            OrderId = orderId,
            OrderSnapshot = "{}"
        };

        var response = new CreatePreparationResponse
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Status = 0,
            CreatedAt = DateTime.UtcNow,
            Message = "Preparação criada com sucesso"
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreatePreparationInputModel>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.StatusCode.Should().Be(201);
        createdAtResult.Value.Should().Be(response);
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
    }

    [Fact]
    public async Task CreatePreparation_WhenPreparationAlreadyExists_ShouldReturn409Conflict()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var request = new CreatePreparationRequest
        {
            OrderId = orderId,
            OrderSnapshot = "{}"
        };

        _mockCreateUseCase.Setup(u => u.ExecuteAsync(It.IsAny<CreatePreparationInputModel>()))
            .ThrowsAsync(new PreparationAlreadyExistsException(orderId));

        // Act
        var result = await _controller.CreatePreparation(request);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = result as ConflictObjectResult;
        conflictResult!.StatusCode.Should().Be(409);
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
- Arquivo de teste criado para PreparationController
- Todos os 4 endpoints testados
- Todos os cenários de cada endpoint testados (sucesso, erros, exceções)
- Testes seguem padrão AAA
- Testes usam Moq para mockar UseCases
- Testes usam FluentAssertions para assertions
- Todos os testes passam
- Cobertura de código para PreparationController >= 85%
