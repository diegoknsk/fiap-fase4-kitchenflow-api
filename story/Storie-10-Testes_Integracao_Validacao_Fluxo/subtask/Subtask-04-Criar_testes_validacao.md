# Subtask 04: Criar testes de validação de fluxo

## Descrição
Criar testes adicionais que validam regras de negócio específicas, transições de status e relacionamentos entre Preparation e Delivery.

## Passos de implementação
- Criar classe `PreparationDeliveryFlowTests.cs`:
  - Teste: `ShouldNotCreateDeliveryWhenPreparationNotFinished`
  - Teste: `ShouldCreateDeliveryWhenPreparationFinished`
  - Teste: `ShouldNotFinalizeDeliveryWhenStatusInvalid`
  - Teste: `ShouldNotStartPreparationWhenStatusInvalid`
  - Teste: `ShouldNotFinishPreparationWhenStatusInvalid`
- Criar classe `StatusTransitionTests.cs`:
  - Teste: `Preparation_ReceivedToInProgress_ShouldSucceed`
  - Teste: `Preparation_InProgressToFinished_ShouldSucceed`
  - Teste: `Preparation_ReceivedToFinished_ShouldFail`
  - Teste: `Delivery_ReadyForPickupToFinalized_ShouldSucceed`
  - Teste: `Delivery_FinalizedToReadyForPickup_ShouldFail`
- Criar classe `IdempotencyTests.cs`:
  - Teste: `ShouldReturnExistingPreparationWhenOrderIdExists`
  - Teste: `ShouldReturnExistingDeliveryWhenPreparationIdExists`
- Criar classe `ValidationTests.cs`:
  - Teste: `ShouldValidateOrderSnapshotStructure`
  - Teste: `ShouldValidateRequiredFields`
  - Teste: `ShouldValidateDataTypes`
- Organizar testes usando `[Fact]` ou `[Theory]`
- Adicionar nomes descritivos

## Referências
- **xUnit**: Padrão de testes
- **Validações**: Testar regras de negócio específicas

## Observações importantes
- **Foco em regras**: Testar regras de negócio específicas
- **Cenários de erro**: Testar que erros são retornados corretamente
- **Isolamento**: Cada teste deve ser independente

## Como testar
- Executar `dotnet test` no projeto de testes
- Verificar que todos os testes passam
- Verificar cobertura de código

## Critérios de aceite
- Testes de validação criados:
  - PreparationDeliveryFlowTests
  - StatusTransitionTests
  - IdempotencyTests
  - ValidationTests
- Testes cobrem:
  - Transições de status válidas e inválidas
  - Relacionamento Preparation → Delivery
  - Validações de dados
  - Idempotência
- Todos os testes passam (`dotnet test` executa com sucesso)
- Testes seguem padrão AAA (Arrange-Act-Assert)
- Testes têm nomes descritivos
- Cobertura adequada das regras de negócio
