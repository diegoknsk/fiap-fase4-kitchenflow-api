# Subtask 04: Adicionar testes para Controllers não cobertos

## Descrição
Criar testes unitários adicionais para Controllers que têm baixa cobertura, focando em cenários de erro não cobertos, validações de entrada e diferentes status codes.

## Passos de implementação
- Revisar lista de Controllers com baixa cobertura (Subtask 02)
- Para cada Controller identificado:
  - Revisar testes existentes
  - Identificar cenários não cobertos
  - Criar testes para cenários faltantes:
    - Cenários de erro não cobertos
    - Validações de entrada não cobertas
    - Diferentes status codes não cobertos
    - Edge cases não cobertos
- Executar testes após cada Controller
- Verificar cobertura após cada Controller
- Documentar cobertura alcançada

## Controllers a Cobrir

### PreparationController
- [ ] `POST /api/preparations`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]
- [ ] `GET /api/preparations`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]
- [ ] `POST /api/preparations/{id}/start`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]
- [ ] `POST /api/preparations/{id}/finish`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]

### DeliveryController
- [ ] `POST /api/deliveries`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]
- [ ] `GET /api/deliveries/ready`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]
- [ ] `POST /api/deliveries/{id}/finalize`:
  - [ ] Cenários de erro não cobertos: [lista]
  - [ ] Validações de entrada não cobertas: [lista]
  - [ ] Status codes não cobertos: [lista]

## Padrão de Testes

### Estrutura AAA (Arrange, Act, Assert)
```csharp
[Fact]
public async Task [Endpoint]_[Cenário]_[ResultadoEsperado]()
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
- `[Endpoint]_[Cenário]_[ResultadoEsperado]`
- Exemplo: `CreatePreparation_WhenUseCaseThrowsException_ShouldReturnApiResponseFail`

### Verificações
- Verificar `ApiResponse<T>` retornado
- Verificar status code HTTP
- Verificar `Success`, `Message` e `Content`
- Verificar chamadas de mocks (UseCases)
- Verificar exceções tratadas corretamente

## Cenários Comuns a Testar

### Cenários de Erro
- [ ] UseCase lança exceção → Controller retorna `ApiResponse<T>.Fail()`
- [ ] Validação de entrada falha → Controller retorna 400 Bad Request
- [ ] Recurso não encontrado → Controller retorna 404 Not Found
- [ ] Conflito (ex: idempotência) → Controller retorna 409 Conflict

### Validações de Entrada
- [ ] Request nulo → 400 Bad Request
- [ ] Campos obrigatórios ausentes → 400 Bad Request
- [ ] Campos com formato inválido → 400 Bad Request
- [ ] Valores fora do range → 400 Bad Request

### Status Codes
- [ ] 200 OK (sucesso)
- [ ] 201 Created (criação)
- [ ] 400 Bad Request (validação)
- [ ] 404 Not Found (não encontrado)
- [ ] 409 Conflict (conflito)
- [ ] 500 Internal Server Error (erro interno)

## Critérios de aceite
- [ ] Testes adicionais criados para todos os Controllers com baixa cobertura
- [ ] Cenários de erro não cobertos testados
- [ ] Validações de entrada não cobertas testadas
- [ ] Diferentes status codes não cobertos testados
- [ ] Edge cases não cobertos testados
- [ ] Todos os testes passam
- [ ] Cobertura de Controllers >= 85%
- [ ] Testes seguem padrão AAA
- [ ] Testes seguem nomenclatura descritiva
- [ ] Testes usam mocks para UseCases
- [ ] Testes são independentes

## Observações importantes
- **Focar em contrato HTTP**: Controllers validam contrato HTTP, não lógica de negócio
- **Mockar UseCases**: Sempre mockar UseCases, não testar lógica de negócio
- **Status codes**: Verificar que status codes corretos são retornados
- **Tratamento de exceções**: Verificar que exceções são tratadas corretamente
- **Verificar incrementalmente**: Verificar cobertura após cada Controller
