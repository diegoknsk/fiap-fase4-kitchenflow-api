# Subtask 03: Refatorar Responses e Presenters - PreparationManagement

## Descrição
Refatorar todos os Responses e Presenters do contexto PreparationManagement para usar `ApiResponse<T>`, movendo mensagens de negócio para os Presenters.

## Passos de implementação

### 1. Atualizar Responses (remover Message se existir)
- **CreatePreparationResponse**: Remover propriedade `Message` (se existir)
- **GetPreparationsResponse**: Verificar se tem Message, remover se existir
- **StartPreparationResponse**: Remover Message se existir
- **FinishPreparationResponse**: Remover Message se existir
- Manter apenas propriedades de dados (Id, OrderId, Status, etc.)

### 2. Atualizar Presenters para retornar ApiResponse<T>
- **CreatePreparationPresenter**:
  ```csharp
  public static ApiResponse<CreatePreparationResponse> Present(CreatePreparationOutputModel outputModel)
  {
      var response = new CreatePreparationResponse
      {
          Id = outputModel.Id,
          OrderId = outputModel.OrderId,
          Status = outputModel.Status,
          CreatedAt = outputModel.CreatedAt
      };
      return ApiResponse<CreatePreparationResponse>.Ok(response, "Preparação criada com sucesso.");
  }
  ```

- **GetPreparationsPresenter**: Similar, mensagem: "Lista de preparações retornada com sucesso."
- **StartPreparationPresenter**: Mensagem: "Preparação iniciada com sucesso."
- **FinishPreparationResponse**: Mensagem: "Preparação finalizada com sucesso."

### 3. Atualizar Use Cases para retornar ApiResponse<T>
- **CreatePreparationUseCase**: 
  ```csharp
  // ANTES: retornava CreatePreparationResponse
  public async Task<CreatePreparationResponse> ExecuteAsync(...)
  
  // DEPOIS: retorna ApiResponse<CreatePreparationResponse>
  public async Task<ApiResponse<CreatePreparationResponse>> ExecuteAsync(...)
  {
      // ... lógica ...
      return CreatePreparationPresenter.Present(outputModel);
  }
  ```

- **GetPreparationsUseCase**: Alterar retorno para `ApiResponse<GetPreparationsResponse>`
- **StartPreparationUseCase**: Alterar retorno para `ApiResponse<StartPreparationResponse>`
- **FinishPreparationUseCase**: Alterar retorno para `ApiResponse<FinishPreparationResponse>`
- **IMPORTANTE**: Use Cases devem retornar `ApiResponse<T>`, não Response models simples

### 4. Adicionar using
- Adicionar `using FastFood.KitchenFlow.Application.Models.Common;` em todos os Presenters
- Adicionar `using FastFood.KitchenFlow.Application.Models.Common;` em todos os Use Cases

## Arquivos a modificar
- `Responses/PreparationManagement/CreatePreparationResponse.cs`
- `Responses/PreparationManagement/GetPreparationsResponse.cs`
- `Responses/PreparationManagement/StartPreparationResponse.cs`
- `Responses/PreparationManagement/FinishPreparationResponse.cs`
- `Presenters/PreparationManagement/CreatePreparationPresenter.cs`
- `Presenters/PreparationManagement/GetPreparationsPresenter.cs`
- `Presenters/PreparationManagement/StartPreparationPresenter.cs`
- `Presenters/PreparationManagement/FinishPreparationPresenter.cs`
- `UseCases/PreparationManagement/*.cs` (verificar tipos de retorno)

## Mensagens de negócio
- **CreatePreparation**: "Preparação criada com sucesso."
- **GetPreparations**: "Lista de preparações retornada com sucesso."
- **StartPreparation**: "Preparação iniciada com sucesso."
- **FinishPreparation**: "Preparação finalizada com sucesso."

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
