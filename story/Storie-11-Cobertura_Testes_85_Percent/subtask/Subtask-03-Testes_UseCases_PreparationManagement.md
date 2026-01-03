# Subtask 03: Criar testes para UseCases de PreparationManagement

## Descrição
Criar testes unitários completos para todos os UseCases de PreparationManagement (CreatePreparationUseCase, GetPreparationsUseCase, StartPreparationUseCase, FinishPreparationUseCase), cobrindo casos de sucesso, validações, exceções e edge cases.

## Passos de implementação
- Criar arquivo `Application/UseCases/PreparationManagement/CreatePreparationUseCaseTests.cs`
- Criar arquivo `Application/UseCases/PreparationManagement/GetPreparationsUseCaseTests.cs`
- Criar arquivo `Application/UseCases/PreparationManagement/StartPreparationUseCaseTests.cs`
- Criar arquivo `Application/UseCases/PreparationManagement/FinishPreparationUseCaseTests.cs`
- Implementar testes seguindo padrão AAA (Arrange, Act, Assert)
- Usar Moq para mockar dependências (repositories)
- Usar FluentAssertions para assertions
- Cobrir todos os cenários de cada UseCase

## Testes para CreatePreparationUseCase

### Cenários a testar:
1. **CreatePreparation_WhenValidInput_ShouldReturnSuccess**
   - Arrange: OrderId válido, OrderSnapshot válido, repository mockado
   - Act: Executar UseCase
   - Assert: Response não nulo, Status = Received, repository.CreateAsync chamado

2. **CreatePreparation_WhenOrderIdIsEmpty_ShouldThrowArgumentException**
   - Arrange: OrderId = Guid.Empty
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

3. **CreatePreparation_WhenOrderSnapshotIsNull_ShouldThrowArgumentException**
   - Arrange: OrderSnapshot = null
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

4. **CreatePreparation_WhenOrderSnapshotIsEmpty_ShouldThrowArgumentException**
   - Arrange: OrderSnapshot = ""
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

5. **CreatePreparation_WhenOrderSnapshotIsInvalidJson_ShouldThrowArgumentException**
   - Arrange: OrderSnapshot = "{ invalid json }"
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

6. **CreatePreparation_WhenOrderSnapshotIsInvalid_ShouldThrowArgumentException**
   - Arrange: OrderSnapshot com dados inválidos (sem OrderId, etc.)
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

7. **CreatePreparation_WhenOrderIdDoesNotMatch_ShouldThrowArgumentException**
   - Arrange: OrderId diferente do OrderId no OrderSnapshot
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

8. **CreatePreparation_WhenPreparationAlreadyExists_ShouldThrowPreparationAlreadyExistsException**
   - Arrange: Repository retorna Preparation existente
   - Act: Executar UseCase
   - Assert: PreparationAlreadyExistsException lançada

## Testes para GetPreparationsUseCase

### Cenários a testar:
1. **GetPreparations_WhenValidInput_ShouldReturnList**
   - Arrange: Parâmetros válidos, repository mockado com dados
   - Act: Executar UseCase
   - Assert: Response não nulo, lista não vazia

2. **GetPreparations_WhenPageNumberIsInvalid_ShouldThrowArgumentException**
   - Arrange: PageNumber <= 0
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

3. **GetPreparations_WhenPageSizeIsInvalid_ShouldThrowArgumentException**
   - Arrange: PageSize <= 0 ou > 100
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

4. **GetPreparations_WhenFilteredByStatus_ShouldReturnFilteredList**
   - Arrange: Status = Received, repository mockado
   - Act: Executar UseCase
   - Assert: Apenas preparações com status Received retornadas

## Testes para StartPreparationUseCase

### Cenários a testar:
1. **StartPreparation_WhenValidInput_ShouldReturnSuccess**
   - Arrange: Preparation com status Received, repository mockado
   - Act: Executar UseCase
   - Assert: Response não nulo, Status = InProgress, repository.UpdateAsync chamado

2. **StartPreparation_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException**
   - Arrange: Repository retorna null
   - Act: Executar UseCase
   - Assert: PreparationNotFoundException lançada

3. **StartPreparation_WhenStatusIsNotReceived_ShouldThrowInvalidOperationException**
   - Arrange: Preparation com status InProgress ou Finished
   - Act: Executar UseCase
   - Assert: InvalidOperationException lançada

## Testes para FinishPreparationUseCase

### Cenários a testar:
1. **FinishPreparation_WhenValidInput_ShouldReturnSuccess**
   - Arrange: Preparation com status InProgress, repository mockado
   - Act: Executar UseCase
   - Assert: Response não nulo, Status = Finished, repository.UpdateAsync chamado

2. **FinishPreparation_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException**
   - Arrange: Repository retorna null
   - Act: Executar UseCase
   - Assert: PreparationNotFoundException lançada

3. **FinishPreparation_WhenStatusIsNotInProgress_ShouldThrowInvalidOperationException**
   - Arrange: Preparation com status Received ou Finished
   - Act: Executar UseCase
   - Assert: InvalidOperationException lançada

## Exemplo de teste (CreatePreparationUseCase)

```csharp
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.PreparationManagement;

public class CreatePreparationUseCaseTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly CreatePreparationUseCase _useCase;

    public CreatePreparationUseCaseTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _useCase = new CreatePreparationUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task CreatePreparation_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"{orderId}","orderCode":"ORD-001","totalPrice":50.00,"items":[]}""";
        
        _mockRepository.Setup(r => r.GetByOrderIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Preparation?)null);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask);

        var inputModel = new CreatePreparationInputModel
        {
            OrderId = orderId,
            OrderSnapshot = orderSnapshot
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.OrderId.Should().Be(orderId);
        result.Status.Should().Be((int)EnumPreparationStatus.Received);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Preparation>()), Times.Once);
    }

    [Fact]
    public async Task CreatePreparation_WhenOrderIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = Guid.Empty,
            OrderSnapshot = """{"orderId":"00000000-0000-0000-0000-000000000000"}"""
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(inputModel));
    }

    // ... outros testes
}
```

## Observações importantes
- **Mocks**: Sempre mockar dependências externas (repositories)
- **Isolamento**: Cada teste deve ser independente
- **Nomenclatura**: Seguir padrão `[Método]_[Cenário]_[ResultadoEsperado]`
- **AAA**: Sempre usar Arrange, Act, Assert
- **FluentAssertions**: Usar para assertions mais legíveis
- **Cobertura**: Cobrir todos os caminhos de código (sucesso, falha, edge cases)

## Como testar
- Executar `dotnet test` no projeto de testes
- Verificar que todos os testes passam
- Executar `dotnet test --collect:"XPlat Code Coverage"` para verificar cobertura
- Verificar que cada UseCase tem cobertura adequada

## Critérios de aceite
- 4 arquivos de teste criados (um para cada UseCase)
- Todos os cenários de cada UseCase testados
- Testes seguem padrão AAA
- Testes usam Moq para mocks
- Testes usam FluentAssertions para assertions
- Todos os testes passam
- Cobertura de código para UseCases de PreparationManagement >= 90%
