# Subtask 04: Refatorar Responses e Presenters - DeliveryManagement

## Descrição
Refatorar todos os Responses e Presenters do contexto DeliveryManagement para usar `ApiResponse<T>`, movendo mensagens de negócio para os Presenters.

## Passos de implementação

### 1. Atualizar Responses (remover Message se existir)
- **CreateDeliveryResponse**: Remover propriedade `Message` (se existir)
- **GetReadyDeliveriesResponse**: Verificar se tem Message, remover se existir
- **FinalizeDeliveryResponse**: Remover Message se existir
- Manter apenas propriedades de dados (Id, PreparationId, OrderId, Status, etc.)

### 2. Atualizar Presenters para retornar ApiResponse<T>
- **CreateDeliveryPresenter**:
  ```csharp
  public static ApiResponse<CreateDeliveryResponse> Present(CreateDeliveryOutputModel outputModel)
  {
      var response = new CreateDeliveryResponse
      {
          Id = outputModel.Id,
          PreparationId = outputModel.PreparationId,
          OrderId = outputModel.OrderId,
          Status = outputModel.Status,
          CreatedAt = outputModel.CreatedAt
      };
      return ApiResponse<CreateDeliveryResponse>.Ok(response, "Entrega criada com sucesso.");
  }
  ```

- **GetReadyDeliveriesPresenter**: Mensagem: "Lista de entregas prontas para retirada retornada com sucesso."
- **FinalizeDeliveryPresenter**: Mensagem: "Entrega finalizada com sucesso."

### 3. Atualizar Use Cases para retornar ApiResponse<T>
- **CreateDeliveryUseCase**: 
  ```csharp
  // ANTES: retornava CreateDeliveryResponse
  public async Task<CreateDeliveryResponse> ExecuteAsync(...)
  
  // DEPOIS: retorna ApiResponse<CreateDeliveryResponse>
  public async Task<ApiResponse<CreateDeliveryResponse>> ExecuteAsync(...)
  {
      // ... lógica ...
      return CreateDeliveryPresenter.Present(outputModel);
  }
  ```

- **GetReadyDeliveriesUseCase**: Alterar retorno para `ApiResponse<GetReadyDeliveriesResponse>`
- **FinalizeDeliveryUseCase**: Alterar retorno para `ApiResponse<FinalizeDeliveryResponse>`
- **IMPORTANTE**: Use Cases devem retornar `ApiResponse<T>`, não Response models simples

### 4. Adicionar using
- Adicionar `using FastFood.KitchenFlow.Application.Models.Common;` em todos os Presenters
- Adicionar `using FastFood.KitchenFlow.Application.Models.Common;` em todos os Use Cases

## Arquivos a modificar
- `Responses/DeliveryManagement/CreateDeliveryResponse.cs`
- `Responses/DeliveryManagement/GetReadyDeliveriesResponse.cs`
- `Responses/DeliveryManagement/FinalizeDeliveryResponse.cs`
- `Presenters/DeliveryManagement/CreateDeliveryPresenter.cs`
- `Presenters/DeliveryManagement/GetReadyDeliveriesPresenter.cs`
- `Presenters/DeliveryManagement/FinalizeDeliveryPresenter.cs`
- `UseCases/DeliveryManagement/*.cs` (verificar tipos de retorno)

## Mensagens de negócio
- **CreateDelivery**: "Entrega criada com sucesso."
- **GetReadyDeliveries**: "Lista de entregas prontas para retirada retornada com sucesso."
- **FinalizeDelivery**: "Entrega finalizada com sucesso."

## Referências
- Seguir padrão estabelecido na Story 12
- Manter compatibilidade com estrutura existente

## Observações importantes
- Não alterar OutputModels (eles permanecem iguais)
- Apenas Presenters e Responses são alterados
- Use Cases continuam chamando Presenters normalmente
- Controllers serão atualizados na próxima subtask

## Validação
- [ ] Todos os Responses atualizados (Message removida)
- [ ] Todos os Presenters retornam `ApiResponse<T>`
- [ ] Mensagens de negócio definidas nos Presenters
- [ ] Use Cases retornam `ApiResponse<T>` corretamente
- [ ] Código compila
- [ ] Documentação XML atualizada
