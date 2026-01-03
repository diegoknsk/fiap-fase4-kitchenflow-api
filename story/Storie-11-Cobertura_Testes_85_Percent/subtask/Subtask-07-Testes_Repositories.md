# Subtask 07: Criar testes para Repositories (se necessário)

## Descrição
Criar testes unitários para os Repositories (`PreparationRepository` e `DeliveryRepository`) apenas se necessário para atingir 85% de cobertura de código. Priorizar testes de UseCases e Controllers primeiro.

## Passos de implementação
- Verificar cobertura atual após testes de UseCases e Controllers
- Se cobertura < 85%, identificar se Repositories precisam de testes
- Criar arquivo `Infra.Persistence/Repositories/PreparationRepositoryTests.cs` (se necessário)
- Criar arquivo `Infra.Persistence/Repositories/DeliveryRepositoryTests.cs` (se necessário)
- Implementar testes focando em lógica de mapeamento e transformação
- Usar banco em memória ou mocks do DbContext para testes

## Quando testar Repositories

### Prioridade:
1. **UseCases** (prioridade alta) - Maior impacto na cobertura
2. **Controllers** (prioridade média) - Validação de contrato HTTP
3. **Repositories** (prioridade baixa) - Apenas se necessário para atingir 85%

### Decisão:
- Se após testes de UseCases e Controllers a cobertura >= 85%, **NÃO** criar testes de Repositories
- Se após testes de UseCases e Controllers a cobertura < 85%, avaliar se testes de Repositories são necessários

## Testes para PreparationRepository

### Cenários a testar (se necessário):
1. **CreateAsync_WhenValidPreparation_ShouldPersist**
   - Arrange: Preparation válida, DbContext mockado
   - Act: Chamar CreateAsync
   - Assert: PreparationEntity criada no banco

2. **GetByOrderIdAsync_WhenExists_ShouldReturnPreparation**
   - Arrange: PreparationEntity no banco
   - Act: Chamar GetByOrderIdAsync
   - Assert: Preparation retornada

3. **GetByOrderIdAsync_WhenNotExists_ShouldReturnNull**
   - Arrange: Nenhuma PreparationEntity no banco
   - Act: Chamar GetByOrderIdAsync
   - Assert: null retornado

4. **GetByIdAsync_WhenExists_ShouldReturnPreparation**
   - Arrange: PreparationEntity no banco
   - Act: Chamar GetByIdAsync
   - Assert: Preparation retornada

5. **UpdateAsync_WhenValidPreparation_ShouldUpdate**
   - Arrange: PreparationEntity no banco, Preparation modificada
   - Act: Chamar UpdateAsync
   - Assert: PreparationEntity atualizada

## Testes para DeliveryRepository

### Cenários a testar (se necessário):
1. **CreateAsync_WhenValidDelivery_ShouldPersist**
   - Arrange: Delivery válida, DbContext mockado
   - Act: Chamar CreateAsync
   - Assert: DeliveryEntity criada no banco

2. **GetByPreparationIdAsync_WhenExists_ShouldReturnDelivery**
   - Arrange: DeliveryEntity no banco
   - Act: Chamar GetByPreparationIdAsync
   - Assert: Delivery retornada

3. **GetReadyDeliveriesAsync_WhenExists_ShouldReturnList**
   - Arrange: DeliveryEntities com status ReadyForPickup no banco
   - Act: Chamar GetReadyDeliveriesAsync
   - Assert: Lista de Deliveries retornada

4. **GetByIdAsync_WhenExists_ShouldReturnDelivery**
   - Arrange: DeliveryEntity no banco
   - Act: Chamar GetByIdAsync
   - Assert: Delivery retornada

5. **UpdateAsync_WhenValidDelivery_ShouldUpdate**
   - Arrange: DeliveryEntity no banco, Delivery modificada
   - Act: Chamar UpdateAsync
   - Assert: DeliveryEntity atualizada

## Exemplo de teste (usando banco em memória)

```csharp
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FastFood.KitchenFlow.Infra.Persistence;
using FastFood.KitchenFlow.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Persistence.Repositories;

public class PreparationRepositoryTests : IDisposable
{
    private readonly KitchenFlowDbContext _dbContext;
    private readonly PreparationRepository _repository;

    public PreparationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<KitchenFlowDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new KitchenFlowDbContext(options);
        _repository = new PreparationRepository(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_WhenValidPreparation_ShouldPersist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var preparation = Preparation.Create(orderId, "{}");

        // Act
        await _repository.CreateAsync(preparation);
        await _dbContext.SaveChangesAsync();

        // Assert
        var persisted = await _dbContext.Preparations
            .FirstOrDefaultAsync(p => p.Id == preparation.Id);
        persisted.Should().NotBeNull();
        persisted!.OrderId.Should().Be(orderId);
    }

    [Fact]
    public async Task GetByOrderIdAsync_WhenExists_ShouldReturnPreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var preparation = Preparation.Create(orderId, "{}");
        await _repository.CreateAsync(preparation);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByOrderIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result!.OrderId.Should().Be(orderId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
```

## Observações importantes
- **Banco em memória**: Usar `UseInMemoryDatabase` para testes isolados
- **Dispose**: Sempre fazer dispose do DbContext após testes
- **Isolamento**: Cada teste deve usar um banco em memória diferente
- **Foco**: Testar mapeamento e transformação, não lógica de negócio
- **Apenas se necessário**: Criar testes apenas se cobertura < 85% após testes de UseCases e Controllers

## Como testar
- Executar `dotnet test` no projeto de testes
- Verificar que todos os testes passam
- Executar `dotnet test --collect:"XPlat Code Coverage"` para verificar cobertura
- Verificar se a cobertura atingiu 85%

## Critérios de aceite
- Testes de Repositories criados apenas se necessário para atingir 85%
- Testes focam em mapeamento e transformação
- Testes usam banco em memória para isolamento
- Todos os testes passam
- Cobertura de código >= 85% após todos os testes
