# Subtask 02: Atualizar Use Cases para validar InputModels e lançar ValidationException

## Descrição
Atualizar todos os Use Cases para validar InputModels no início do método e lançar `ValidationException` quando os dados forem inválidos, garantindo que todas as validações estejam na camada Application.

## Passos de implementação

### 1. Atualizar CreatePreparationUseCase
- Adicionar validação de `inputModel == null` no início do método
- Substituir validações de `Guid.Empty` e `string.IsNullOrWhiteSpace` para lançar `ValidationException` em vez de `ArgumentException`
- Manter validações específicas de negócio (JSON, Data Annotations, etc.)
- Adicionar `using FastFood.KitchenFlow.Application.Exceptions;`

**Exemplo:**
```csharp
public async Task<ApiResponse<CreatePreparationResponse>> ExecuteAsync(CreatePreparationInputModel inputModel)
{
    // Validar InputModel
    if (inputModel == null)
    {
        throw new ValidationException("Dados de entrada não podem ser nulos.");
    }

    if (inputModel.OrderId == Guid.Empty)
    {
        throw new ValidationException("OrderId não pode ser vazio.");
    }

    if (string.IsNullOrWhiteSpace(inputModel.OrderSnapshot))
    {
        throw new ValidationException("OrderSnapshot não pode ser nulo ou vazio.");
    }

    // ... resto da lógica (validações específicas podem continuar lançando ArgumentException) ...
}
```

### 2. Atualizar GetPreparationsUseCase
- Adicionar validação de `inputModel == null`
- Validar `PageNumber` e `PageSize` (deve ser > 0)
- Lançar `ValidationException` para validações básicas

### 3. Atualizar StartPreparationUseCase
- Adicionar validação de `inputModel == null`
- Validar `Id` (não pode ser `Guid.Empty`)
- Lançar `ValidationException` para validações básicas

### 4. Atualizar FinishPreparationUseCase
- Adicionar validação de `inputModel == null`
- Validar `Id` (não pode ser `Guid.Empty`)
- Lançar `ValidationException` para validações básicas

### 5. Atualizar CreateDeliveryUseCase
- Adicionar validação de `inputModel == null`
- Substituir validação de `Guid.Empty` para lançar `ValidationException`
- Manter validações específicas de negócio

### 6. Atualizar GetReadyDeliveriesUseCase
- Adicionar validação de `inputModel == null`
- Validar `PageNumber` e `PageSize` (deve ser > 0)
- Lançar `ValidationException` para validações básicas

### 7. Atualizar FinalizeDeliveryUseCase
- Adicionar validação de `inputModel == null`
- Validar `Id` (não pode ser `Guid.Empty`)
- Lançar `ValidationException` para validações básicas

## Arquivos a modificar
- `UseCases/PreparationManagement/CreatePreparationUseCase.cs`
- `UseCases/PreparationManagement/GetPreparationsUseCase.cs`
- `UseCases/PreparationManagement/StartPreparationUseCase.cs`
- `UseCases/PreparationManagement/FinishPreparationUseCase.cs`
- `UseCases/DeliveryManagement/CreateDeliveryUseCase.cs`
- `UseCases/DeliveryManagement/GetReadyDeliveriesUseCase.cs`
- `UseCases/DeliveryManagement/FinalizeDeliveryUseCase.cs`

## Regras
1. **Validações básicas**: Usar `ValidationException`
   - `inputModel == null`
   - `Guid.Empty`
   - `string.IsNullOrWhiteSpace`
   - `PageNumber <= 0` ou `PageSize <= 0`

2. **Validações específicas de negócio**: Podem continuar usando `ArgumentException`
   - Validação de JSON
   - Validação de Data Annotations
   - Validação de regras de negócio complexas

3. **Exceções de negócio**: Manter exceções específicas
   - `PreparationNotFoundException`
   - `DeliveryNotFoundException`
   - `PreparationAlreadyExistsException`
   - `DeliveryAlreadyExistsException`
   - `PreparationNotFinishedException`

## Referências
- Seguir padrão estabelecido na Story 13
- Manter compatibilidade com estrutura existente
- Não alterar lógica de negócio, apenas validações iniciais

## Observações importantes
- Não alterar OutputModels ou Presenters
- Apenas adicionar validações iniciais nos Use Cases
- Manter validações específicas de negócio como estão
- Adicionar `using FastFood.KitchenFlow.Application.Exceptions;` em todos os Use Cases

## Validação
- [ ] Todos os Use Cases validam `inputModel == null`
- [ ] Validações básicas lançam `ValidationException`
- [ ] Validações específicas de negócio mantidas
- [ ] Exceções de negócio mantidas
- [ ] Código compila
- [ ] Testes existentes continuam passando
