# Subtask 04: Atualizar FinishPreparationPresenter

## Descrição
Atualizar o `FinishPreparationPresenter` para mapear a propriedade `DeliveryId` do OutputModel para o Response.

## Passos de implementação
- Abrir classe `FinishPreparationPresenter` em `Application/Presenters/PreparationManagement/FinishPreparationPresenter.cs`
- Atualizar método `Present`:
  - No mapeamento do `FinishPreparationResponse` (linha 19-25):
    - Adicionar `DeliveryId = outputModel.DeliveryId`
  - Exemplo completo:
    ```csharp
    var response = new FinishPreparationResponse
    {
        Id = outputModel.Id,
        OrderId = outputModel.OrderId,
        Status = outputModel.Status,
        CreatedAt = outputModel.CreatedAt,
        DeliveryId = outputModel.DeliveryId
    };
    ```
- Verificar que o mapeamento está completo e correto
- Verificar que não há quebra de compatibilidade

## Referências
- **Presenter atual**: Verificar estrutura existente
- **Padrão**: Seguir padrão dos outros Presenters do projeto
- **OutputModel**: Verificar que `DeliveryId` foi adicionado na Subtask 02
- **Response**: Verificar que `DeliveryId` foi adicionado na Subtask 03

## Observações importantes
- **Mapeamento completo**: Garantir que todas as propriedades são mapeadas
- **Ordem**: Manter ordem lógica das propriedades
- **Nullable**: O `DeliveryId` pode ser null no OutputModel, mas será mapeado corretamente
