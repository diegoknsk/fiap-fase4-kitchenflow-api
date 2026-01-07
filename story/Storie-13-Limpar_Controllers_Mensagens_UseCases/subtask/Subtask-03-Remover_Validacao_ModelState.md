# Subtask 03: Remover validação de ModelState das Controllers

## Descrição
Remover a validação manual de `ModelState.IsValid` das controllers, pois a validação será feita nos Use Cases através de `ValidationException`.

## Passos de implementação

### 1. Atualizar PreparationController

#### CreatePreparation
- **Remover**:
  ```csharp
  if (!ModelState.IsValid)
  {
      return BadRequest(ApiResponse<CreatePreparationResponse>.Fail("Dados inválidos."));
  }
  ```
- **Manter**: Mapeamento de Request para InputModel e chamada do Use Case
- **Adicionar**: Tratamento de `ValidationException` no catch

#### GetPreparations
- Não tem validação de ModelState (já está correto)

#### StartPreparation
- Não tem validação de ModelState (já está correto)

#### FinishPreparation
- Não tem validação de ModelState (já está correto)

### 2. Atualizar DeliveryController

#### CreateDelivery
- **Remover**:
  ```csharp
  if (!ModelState.IsValid)
  {
      return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail("Dados inválidos."));
  }
  ```
- **Manter**: Mapeamento de Request para InputModel e chamada do Use Case
- **Adicionar**: Tratamento de `ValidationException` no catch

#### GetReadyDeliveries
- Não tem validação de ModelState (já está correto)

#### FinalizeDelivery
- Não tem validação de ModelState (já está correto)

## Arquivos a modificar
- `Controllers/PreparationController.cs`
- `Controllers/DeliveryController.cs`

## Observações importantes
- **ModelState validation**: A validação de ModelState do ASP.NET Core continuará funcionando automaticamente
- **Use Case validation**: Use Cases agora validam InputModels e lançam `ValidationException`
- **Dupla validação**: Pode haver validação dupla (ModelState + Use Case), mas isso é aceitável e garante robustez
- **Tratamento de exceções**: Controllers devem tratar `ValidationException` e retornar `BadRequest` com mensagem da exceção

## Referências
- Seguir padrão estabelecido na Story 13
- Manter compatibilidade com estrutura existente

## Validação
- [ ] Validação de ModelState removida de `CreatePreparation`
- [ ] Validação de ModelState removida de `CreateDelivery`
- [ ] Controllers não têm mais mensagens hardcoded para validação
- [ ] Código compila
- [ ] Testes existentes continuam passando
