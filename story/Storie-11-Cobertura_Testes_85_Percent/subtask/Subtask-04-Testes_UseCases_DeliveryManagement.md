# Subtask 04: Criar testes para UseCases de DeliveryManagement

## Descrição
Criar testes unitários completos para todos os UseCases de DeliveryManagement (CreateDeliveryUseCase, GetReadyDeliveriesUseCase, FinalizeDeliveryUseCase), cobrindo casos de sucesso, validações, exceções e edge cases.

## Passos de implementação
- Criar arquivo `Application/UseCases/DeliveryManagement/CreateDeliveryUseCaseTests.cs`
- Criar arquivo `Application/UseCases/DeliveryManagement/GetReadyDeliveriesUseCaseTests.cs`
- Criar arquivo `Application/UseCases/DeliveryManagement/FinalizeDeliveryUseCaseTests.cs`
- Implementar testes seguindo padrão AAA (Arrange, Act, Assert)
- Usar Moq para mockar dependências (repositories)
- Usar FluentAssertions para assertions
- Cobrir todos os cenários de cada UseCase

## Testes para CreateDeliveryUseCase

### Cenários a testar:
1. **CreateDelivery_WhenValidInput_ShouldReturnSuccess**
   - Arrange: PreparationId válido, Preparation com status Finished, repositories mockados
   - Act: Executar UseCase
   - Assert: Response não nulo, Status = ReadyForPickup, repository.CreateAsync chamado

2. **CreateDelivery_WhenPreparationIdIsEmpty_ShouldThrowArgumentException**
   - Arrange: PreparationId = Guid.Empty
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

3. **CreateDelivery_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException**
   - Arrange: Repository retorna null
   - Act: Executar UseCase
   - Assert: PreparationNotFoundException lançada

4. **CreateDelivery_WhenPreparationIsNotFinished_ShouldThrowPreparationNotFinishedException**
   - Arrange: Preparation com status Received ou InProgress
   - Act: Executar UseCase
   - Assert: PreparationNotFinishedException lançada

5. **CreateDelivery_WhenDeliveryAlreadyExists_ShouldThrowDeliveryAlreadyExistsException**
   - Arrange: Repository retorna Delivery existente
   - Act: Executar UseCase
   - Assert: DeliveryAlreadyExistsException lançada

## Testes para GetReadyDeliveriesUseCase

### Cenários a testar:
1. **GetReadyDeliveries_WhenValidInput_ShouldReturnList**
   - Arrange: Parâmetros válidos, repository mockado com entregas prontas
   - Act: Executar UseCase
   - Assert: Response não nulo, lista contém apenas entregas com status ReadyForPickup

2. **GetReadyDeliveries_WhenPageNumberIsInvalid_ShouldThrowArgumentException**
   - Arrange: PageNumber <= 0
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

3. **GetReadyDeliveries_WhenPageSizeIsInvalid_ShouldThrowArgumentException**
   - Arrange: PageSize <= 0 ou > 100
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

4. **GetReadyDeliveries_WhenNoReadyDeliveries_ShouldReturnEmptyList**
   - Arrange: Repository retorna lista vazia
   - Act: Executar UseCase
   - Assert: Lista vazia retornada

## Testes para FinalizeDeliveryUseCase

### Cenários a testar:
1. **FinalizeDelivery_WhenValidInput_ShouldReturnSuccess**
   - Arrange: Delivery com status ReadyForPickup, repository mockado
   - Act: Executar UseCase
   - Assert: Response não nulo, Status = Finalized, FinalizedAt preenchido, repository.UpdateAsync chamado

2. **FinalizeDelivery_WhenDeliveryIdIsEmpty_ShouldThrowArgumentException**
   - Arrange: DeliveryId = Guid.Empty
   - Act: Executar UseCase
   - Assert: ArgumentException lançada

3. **FinalizeDelivery_WhenDeliveryNotFound_ShouldThrowDeliveryNotFoundException**
   - Arrange: Repository retorna null
   - Act: Executar UseCase
   - Assert: DeliveryNotFoundException lançada

4. **FinalizeDelivery_WhenStatusIsNotReadyForPickup_ShouldThrowInvalidOperationException**
   - Arrange: Delivery com status Finalized
   - Act: Executar UseCase
   - Assert: InvalidOperationException lançada

## Exemplo de teste (CreateDeliveryUseCase)

```csharp
using FastFood.KitchenFlow.Application.Exceptions;
using FastFood.KitchenFlow.Application.InputModels.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.UseCases.DeliveryManagement;

public class CreateDeliveryUseCaseTests
{
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly Mock<IPreparationRepository> _mockPreparationRepository;
    private readonly CreateDeliveryUseCase _useCase;

    public CreateDeliveryUseCaseTests()
    {
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _mockPreparationRepository = new Mock<IPreparationRepository>();
        _useCase = new CreateDeliveryUseCase(
            _mockDeliveryRepository.Object,
            _mockPreparationRepository.Object);
    }

    [Fact]
    public async Task CreateDelivery_WhenValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var preparation = Preparation.Create(orderId, "{}");
        preparation.FinishPreparation(); // Status = Finished

        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);
        _mockDeliveryRepository.Setup(r => r.GetByPreparationIdAsync(preparationId))
            .ReturnsAsync((Delivery?)null);
        _mockDeliveryRepository.Setup(r => r.CreateAsync(It.IsAny<Delivery>()))
            .Returns(Task.CompletedTask);

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId,
            OrderId = orderId
        };

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.PreparationId.Should().Be(preparationId);
        result.Status.Should().Be((int)EnumDeliveryStatus.ReadyForPickup);
        _mockDeliveryRepository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Once);
    }

    [Fact]
    public async Task CreateDelivery_WhenPreparationNotFound_ShouldThrowPreparationNotFoundException()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        _mockPreparationRepository.Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync((Preparation?)null);

        var inputModel = new CreateDeliveryInputModel
        {
            PreparationId = preparationId
        };

        // Act & Assert
        await Assert.ThrowsAsync<PreparationNotFoundException>(
            () => _useCase.ExecuteAsync(inputModel));
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
- 3 arquivos de teste criados (um para cada UseCase)
- Todos os cenários de cada UseCase testados
- Testes seguem padrão AAA
- Testes usam Moq para mocks
- Testes usam FluentAssertions para assertions
- Todos os testes passam
- Cobertura de código para UseCases de DeliveryManagement >= 90%
