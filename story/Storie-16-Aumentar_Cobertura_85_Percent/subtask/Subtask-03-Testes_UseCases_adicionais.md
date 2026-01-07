# Subtask 03: Adicionar testes para UseCases não cobertos

## Descrição
Criar testes unitários adicionais para UseCases que têm baixa cobertura, focando em cenários não cobertos, edge cases e validações de negócio.

## Passos de implementação
- Revisar lista de UseCases com baixa cobertura (Subtask 02)
- Para cada UseCase identificado:
  - Revisar testes existentes
  - Identificar cenários não cobertos
  - Criar testes para cenários faltantes:
    - Cenários de sucesso não cobertos
    - Edge cases não cobertos
    - Validações de negócio não cobertas
    - Cenários de erro não cobertos
- Executar testes após cada UseCase
- Verificar cobertura após cada UseCase
- Documentar cobertura alcançada

## UseCases a Cobrir

### CreatePreparationUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### GetPreparationsUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### StartPreparationUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### FinishPreparationUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### CreateDeliveryUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### GetReadyDeliveriesUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

### FinalizeDeliveryUseCase
- [ ] Cenários de sucesso não cobertos:
  - [ ] [Cenário específico]
- [ ] Edge cases não cobertos:
  - [ ] [Edge case específico]
- [ ] Validações não cobertas:
  - [ ] [Validação específica]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário de erro específico]

## Padrão de Testes

### Estrutura AAA (Arrange, Act, Assert)
```csharp
[Fact]
public async Task [UseCase]_[Cenário]_[ResultadoEsperado]()
{
    // Arrange
    // ... setup ...
    
    // Act
    // ... execução ...
    
    // Assert
    // ... verificações ...
}
```

### Nomenclatura
- `[UseCase]_[Cenário]_[ResultadoEsperado]`
- Exemplo: `CreatePreparation_WhenOrderSnapshotIsInvalid_ShouldReturnApiResponseFail`

### Verificações
- Verificar `ApiResponse<T>` retornado
- Verificar `Success`, `Message` e `Content`
- Verificar chamadas de mocks (se aplicável)
- Verificar exceções lançadas (se aplicável)

## Critérios de aceite
- [ ] Testes adicionais criados para todos os UseCases com baixa cobertura
- [ ] Cenários de sucesso não cobertos testados
- [ ] Edge cases não cobertos testados
- [ ] Validações de negócio não cobertas testadas
- [ ] Cenários de erro não cobertos testados
- [ ] Todos os testes passam
- [ ] Cobertura de UseCases >= 90%
- [ ] Testes seguem padrão AAA
- [ ] Testes seguem nomenclatura descritiva
- [ ] Testes usam mocks para dependências externas
- [ ] Testes são independentes

## Observações importantes
- **Priorizar por impacto**: Focar primeiro nos UseCases que têm maior impacto na cobertura
- **Revisar testes existentes**: Pode haver cenários não cobertos mesmo com testes existentes
- **Edge cases**: Muitas vezes são os cenários não cobertos
- **Validações**: Testar todas as validações de negócio
- **Verificar incrementalmente**: Verificar cobertura após cada UseCase
