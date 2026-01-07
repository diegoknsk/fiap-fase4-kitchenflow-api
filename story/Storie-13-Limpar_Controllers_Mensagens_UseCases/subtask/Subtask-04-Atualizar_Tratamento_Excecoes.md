# Subtask 04: Atualizar tratamento de exceções nas Controllers

## Descrição
Atualizar o tratamento de exceções nas controllers para usar as novas exceções (`ValidationException`, `InternalServerErrorException`) e remover mensagens hardcoded, garantindo que todas as mensagens venham das exceções.

## Passos de implementação

### 1. Atualizar PreparationController

#### CreatePreparation
- **Adicionar** tratamento de `ValidationException`:
  ```csharp
  catch (ValidationException ex)
  {
      return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
  }
  ```
- **Manter** tratamento de exceções específicas:
  - `PreparationAlreadyExistsException` → `Conflict`
  - `ArgumentException` → `BadRequest`
- **Atualizar** catch genérico de `Exception`:
  ```csharp
  catch (InternalServerErrorException ex)
  {
      return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
  }
  catch (Exception ex)
  {
      // Log do erro (não expor detalhes ao cliente)
      // TODO: Implementar logging adequado
      return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail("Erro interno do servidor."));
  }
  ```

#### GetPreparations
- **Adicionar** tratamento de `ValidationException`
- **Atualizar** catch genérico de `Exception`

#### StartPreparation
- **Adicionar** tratamento de `ValidationException`
- **Manter** tratamento de exceções específicas:
  - `PreparationNotFoundException` → `NotFound`
  - `InvalidOperationException` → `BadRequest`
  - `ArgumentException` → `BadRequest`
- **Atualizar** catch genérico de `Exception`

#### FinishPreparation
- **Adicionar** tratamento de `ValidationException`
- **Manter** tratamento de exceções específicas:
  - `PreparationNotFoundException` → `NotFound`
  - `InvalidOperationException` → `BadRequest`
  - `ArgumentException` → `BadRequest`
- **Atualizar** catch genérico de `Exception`

### 2. Atualizar DeliveryController

#### CreateDelivery
- **Adicionar** tratamento de `ValidationException`
- **Manter** tratamento de exceções específicas:
  - `DeliveryAlreadyExistsException` → `Conflict`
  - `PreparationNotFoundException` → `BadRequest`
  - `PreparationNotFinishedException` → `BadRequest`
  - `ArgumentException` → `BadRequest`
- **Atualizar** catch genérico de `Exception`

#### GetReadyDeliveries
- **Adicionar** tratamento de `ValidationException`
- **Atualizar** catch genérico de `Exception`

#### FinalizeDelivery
- **Adicionar** tratamento de `ValidationException`
- **Manter** tratamento de exceções específicas:
  - `DeliveryNotFoundException` → `NotFound`
  - `InvalidOperationException` → `BadRequest`
  - `ArgumentException` → `BadRequest`
- **Atualizar** catch genérico de `Exception`

### 3. Adicionar using
- Adicionar `using FastFood.KitchenFlow.Application.Exceptions;` em ambas as controllers

## Arquivos a modificar
- `Controllers/PreparationController.cs`
- `Controllers/DeliveryController.cs`

## Ordem de tratamento de exceções
1. **Exceções específicas de validação**: `ValidationException` → `BadRequest`
2. **Exceções específicas de negócio**: `PreparationNotFoundException`, `DeliveryNotFoundException`, etc. → Status apropriado
3. **Exceções genéricas de validação**: `ArgumentException` → `BadRequest`
4. **Exceções de operação inválida**: `InvalidOperationException` → `BadRequest`
5. **Exceções de erro interno**: `InternalServerErrorException` → `500`
6. **Catch genérico**: `Exception` → `500` com mensagem padrão

## Exemplo completo

```csharp
public async Task<IActionResult> CreatePreparation([FromBody] CreatePreparationRequest request)
{
    try
    {
        // Mapear Request para InputModel
        var inputModel = new CreatePreparationInputModel
        {
            OrderId = request.OrderId,
            OrderSnapshot = request.OrderSnapshot
        };

        // Chamar UseCase (já retorna ApiResponse<T>)
        var apiResponse = await _createPreparationUseCase.ExecuteAsync(inputModel);

        // Extrair Id do Content
        Guid id = ExtractIdFromContent(apiResponse.Content);

        // Retornar HTTP 201 Created
        return CreatedAtAction(nameof(CreatePreparation), new { id }, apiResponse);
    }
    catch (ValidationException ex)
    {
        return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
    }
    catch (PreparationAlreadyExistsException ex)
    {
        return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
    }
    catch (InternalServerErrorException ex)
    {
        return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
    }
    catch (Exception ex)
    {
        // Log do erro (não expor detalhes ao cliente)
        // TODO: Implementar logging adequado
        return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail("Erro interno do servidor."));
    }
}
```

## Referências
- Seguir padrão estabelecido na Story 13
- Manter compatibilidade com estrutura existente

## Observações importantes
- **Mensagens**: Todas as mensagens vêm das exceções, não são hardcoded
- **Catch genérico**: Deve usar mensagem padrão "Erro interno do servidor." e fazer log do erro real
- **Logging**: Implementar logging adequado para erros internos (não expor detalhes ao cliente)
- **Ordem**: Tratar exceções mais específicas primeiro

## Validação
- [ ] Todas as controllers têm tratamento de `ValidationException`
- [ ] Todas as controllers têm tratamento de `InternalServerErrorException`
- [ ] Catch genérico de `Exception` usa mensagem padrão
- [ ] Todas as mensagens hardcoded removidas
- [ ] Código compila
- [ ] Testes existentes continuam passando
