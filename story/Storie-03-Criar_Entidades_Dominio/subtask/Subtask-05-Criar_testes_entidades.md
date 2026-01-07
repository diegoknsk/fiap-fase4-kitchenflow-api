# Subtask 05: Criar testes unitários para entidades

## Descrição
Criar testes unitários completos para as entidades `Preparation` e `Delivery` no projeto `FastFood.KitchenFlow.Tests.Unit`, cobrindo criação, validações, transições de status válidas e inválidas.

## Passos de implementação
- Criar estrutura de pastas no projeto de testes:
  - `Domain/Entities/PreparationManagement/` (para testes de Preparation)
  - `Domain/Entities/DeliveryManagement/` (para testes de Delivery)
- Criar classe de teste `PreparationTests.cs`:
  - Teste: `Create_WithValidParameters_ShouldCreatePreparationWithReceivedStatus()`
  - Teste: `Create_WithEmptyOrderId_ShouldThrowException()`
  - Teste: `Create_WithNullOrderSnapshot_ShouldThrowException()`
  - Teste: `Create_WithEmptyOrderSnapshot_ShouldThrowException()`
  - Teste: `StartPreparation_WhenStatusIsReceived_ShouldChangeToInProgress()`
  - Teste: `StartPreparation_WhenStatusIsNotReceived_ShouldThrowException()`
  - Teste: `FinishPreparation_WhenStatusIsInProgress_ShouldChangeToFinished()`
  - Teste: `FinishPreparation_WhenStatusIsNotInProgress_ShouldThrowException()`
- Criar classe de teste `DeliveryTests.cs`:
  - Teste: `Create_WithValidPreparationId_ShouldCreateDeliveryWithReadyForPickupStatus()`
  - Teste: `Create_WithEmptyPreparationId_ShouldThrowException()`
  - Teste: `Create_WithOptionalOrderId_ShouldCreateSuccessfully()`
  - Teste: `FinalizeDelivery_WhenStatusIsReadyForPickup_ShouldChangeToFinalized()`
  - Teste: `FinalizeDelivery_WhenStatusIsNotReadyForPickup_ShouldThrowException()`
  - Teste: `FinalizeDelivery_ShouldSetFinalizedAt()`
- Usar framework de testes xUnit (padrão do projeto)
- Organizar testes usando `[Fact]` ou `[Theory]` conforme necessário
- Adicionar nomes descritivos e organizados em Arrange-Act-Assert (AAA)

## Como testar
- Executar `dotnet test` no projeto `FastFood.KitchenFlow.Tests.Unit`
- Verificar que todos os testes passam
- Verificar cobertura de código (se ferramenta disponível)
- Executar testes individualmente para validar cada cenário

## Critérios de aceite
- Classe `PreparationTests.cs` criada com testes:
  - Criação com parâmetros válidos
  - Validações de parâmetros obrigatórios (orderId, orderSnapshot)
  - Transições de status válidas (Received → InProgress → Finished)
  - Transições de status inválidas (devem lançar exceção)
- Classe `DeliveryTests.cs` criada com testes:
  - Criação com parâmetros válidos
  - Validação de preparationId obrigatório
  - Suporte a orderId opcional
  - Transição de status válida (ReadyForPickup → Finalized)
  - Transição de status inválida (deve lançar exceção)
  - Validação de que FinalizedAt é definido
- Todos os testes passam (`dotnet test` executa com sucesso)
- Testes seguem padrão AAA (Arrange-Act-Assert)
- Testes têm nomes descritivos e claros
- Cobertura adequada das entidades (mínimo 80% recomendado)
- Estrutura de pastas organizada no projeto de testes

## Observações
- **Padrão de testes**: Seguir padrão do projeto de referência (OrderHub ou Auth)
- **Cobertura**: Focar em testar lógica de negócio, validações e transições de status
- **Isolamento**: Cada teste deve ser independente e não depender de outros testes
- **Nomenclatura**: Usar nomes descritivos que expliquem o comportamento esperado
