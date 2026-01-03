# Subtask 06: Refatorar Controllers - DeliveryController

## Descrição
Refatorar o `DeliveryController` para usar `ApiResponse<T>` nas respostas e remover mensagens de negócio hardcoded, deixando apenas o tratamento de exceções.

## Passos de implementação

### 1. Atualizar métodos do Controller
- **CreateDelivery**:
  ```csharp
  public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequest request)
  {
      if (!ModelState.IsValid)
      {
          return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail("Dados inválidos."));
      }

      try
      {
          var inputModel = new CreateDeliveryInputModel { ... };
          var apiResponse = await _createDeliveryUseCase.ExecuteAsync(inputModel);
          return CreatedAtAction(nameof(CreateDelivery), new { id = ... }, apiResponse);
      }
      catch (DeliveryAlreadyExistsException ex)
      {
          return Conflict(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
      }
      catch (PreparationNotFoundException ex)
      {
          return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
      }
      catch (PreparationNotFinishedException ex)
      {
          return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
      }
      catch (ArgumentException ex)
      {
          return BadRequest(ApiResponse<CreateDeliveryResponse>.Fail(ex.Message));
      }
      catch (Exception ex)
      {
          return StatusCode(500, ApiResponse<CreateDeliveryResponse>.Fail("Erro interno do servidor."));
      }
  }
  ```

- **GetReadyDeliveries**: Similar, usar `Ok(apiResponse)`
- **FinalizeDelivery**: Similar, usar `Ok(apiResponse)`

### 2. Remover mensagens hardcoded
- Remover todas as mensagens de sucesso dos controllers
- Manter apenas mensagens de erro nas exceções
- Usar `ApiResponse<T>.Fail()` para erros

### 3. Adicionar using
- Adicionar `using FastFood.KitchenFlow.Application.Models.Common;`

### 4. Atualizar ProducesResponseType
- Manter `ProducesResponseType(typeof(ApiResponse<CreateDeliveryResponse>), ...)`
- Ou usar `ProducesResponseType(typeof(ApiResponse<>), ...)` se necessário

## Arquivos a modificar
- `Controllers/DeliveryController.cs`

## Tratamento de exceções
- **DeliveryAlreadyExistsException**: `Conflict(ApiResponse<T>.Fail(ex.Message))`
- **DeliveryNotFoundException**: `NotFound(ApiResponse<T>.Fail(ex.Message))`
- **PreparationNotFoundException**: `BadRequest(ApiResponse<T>.Fail(ex.Message))`
- **PreparationNotFinishedException**: `BadRequest(ApiResponse<T>.Fail(ex.Message))`
- **InvalidOperationException**: `BadRequest(ApiResponse<T>.Fail(ex.Message))`
- **ArgumentException**: `BadRequest(ApiResponse<T>.Fail(ex.Message))`
- **Exception genérica**: `StatusCode(500, ApiResponse<T>.Fail("Erro interno do servidor."))`

## Referências
- Seguir padrão estabelecido na Story 12
- Manter tratamento de exceções existente
- Mensagens de erro vêm das exceções (não hardcoded)

## Observações importantes
- Controllers não devem criar mensagens de sucesso (vêm dos Presenters)
- Controllers apenas tratam exceções e retornam `ApiResponse<T>`
- Mensagens de erro podem vir das exceções ou ser genéricas
- Manter todos os códigos HTTP corretos (201, 200, 400, 404, 409, 500)

## Validação
- [ ] Todos os métodos do controller atualizados
- [ ] Todas as respostas usam `ApiResponse<T>`
- [ ] Mensagens de negócio removidas dos controllers
- [ ] Tratamento de exceções mantido
- [ ] Códigos HTTP corretos
- [ ] Código compila
- [ ] Documentação XML atualizada
- [ ] Testes manuais: verificar que respostas estão no formato correto
