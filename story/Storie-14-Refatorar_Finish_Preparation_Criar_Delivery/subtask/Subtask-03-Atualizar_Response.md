# Subtask 03: Atualizar FinishPreparationResponse

## Descrição
Adicionar a propriedade `DeliveryId` ao `FinishPreparationResponse` para que a API retorne o ID do delivery criado.

## Passos de implementação
- Abrir classe `FinishPreparationResponse` em `Application/Responses/PreparationManagement/FinishPreparationResponse.cs`
- Adicionar propriedade `DeliveryId`:
  - Tipo: `Guid?` (nullable, para manter consistência com OutputModel)
  - Documentação XML: "Identificador do delivery criado automaticamente quando a preparação foi finalizada."
  - Exemplo:
    ```csharp
    /// <summary>
    /// Identificador do delivery criado automaticamente quando a preparação foi finalizada.
    /// </summary>
    public Guid? DeliveryId { get; set; }
    ```
- Verificar que a propriedade está após as propriedades existentes
- Verificar que não há quebra de compatibilidade (propriedade é opcional/nullable)

## Referências
- **Response atual**: Verificar estrutura existente
- **Padrão**: Seguir padrão dos outros Responses do projeto
- **Swagger**: A propriedade será automaticamente documentada no Swagger

## Observações importantes
- **Nullable**: Usar `Guid?` para manter compatibilidade com OutputModel
- **Documentação**: Adicionar documentação XML clara sobre o propósito da propriedade
- **Ordem**: Manter ordem lógica das propriedades (Id, OrderId, Status, CreatedAt, DeliveryId)
- **Swagger**: A propriedade aparecerá automaticamente na documentação Swagger
