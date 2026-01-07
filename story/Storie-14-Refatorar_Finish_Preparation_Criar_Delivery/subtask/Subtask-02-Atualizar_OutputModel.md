# Subtask 02: Atualizar FinishPreparationOutputModel

## Descrição
Adicionar a propriedade `DeliveryId` ao `FinishPreparationOutputModel` para que o UseCase possa retornar o ID do delivery criado.

## Passos de implementação
- Abrir classe `FinishPreparationOutputModel` em `Application/OutputModels/PreparationManagement/FinishPreparationOutputModel.cs`
- Adicionar propriedade `DeliveryId`:
  - Tipo: `Guid?` (nullable, caso não seja necessário em algum cenário futuro)
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
- **OutputModel atual**: Verificar estrutura existente
- **Padrão**: Seguir padrão dos outros OutputModels do projeto

## Observações importantes
- **Nullable**: Usar `Guid?` para manter compatibilidade, embora na prática sempre terá valor
- **Documentação**: Adicionar documentação XML clara sobre o propósito da propriedade
- **Ordem**: Manter ordem lógica das propriedades (Id, OrderId, Status, CreatedAt, DeliveryId)
