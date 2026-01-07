# Subtask 03: Criar InputModels, OutputModels e Responses

## Descrição
Criar os modelos de dados na camada Application: InputModels (entrada dos UseCases), OutputModels (saída dos UseCases) e Responses (retorno da API) para os três endpoints de Delivery.

## Passos de implementação
- Criar estrutura de pastas no projeto `FastFood.KitchenFlow.Application`:
  - `InputModels/DeliveryManagement/`
  - `OutputModels/DeliveryManagement/`
  - `Responses/DeliveryManagement/`
- Criar InputModels:
  - `CreateDeliveryInputModel` em `InputModels/DeliveryManagement/CreateDeliveryInputModel.cs`:
    - `PreparationId` (Guid) - obrigatório
    - `OrderId` (Guid?) - opcional
  - `GetReadyDeliveriesInputModel` em `InputModels/DeliveryManagement/GetReadyDeliveriesInputModel.cs`:
    - `PageNumber` (int) - default: 1
    - `PageSize` (int) - default: 10
  - `FinalizeDeliveryInputModel` em `InputModels/DeliveryManagement/FinalizeDeliveryInputModel.cs`:
    - `Id` (Guid) - ID da delivery
- Criar OutputModels:
  - `CreateDeliveryOutputModel` em `OutputModels/DeliveryManagement/CreateDeliveryOutputModel.cs`:
    - `Id` (Guid)
    - `PreparationId` (Guid)
    - `OrderId` (Guid?)
    - `Status` (int)
    - `CreatedAt` (DateTime)
  - `GetReadyDeliveriesOutputModel` em `OutputModels/DeliveryManagement/GetReadyDeliveriesOutputModel.cs`:
    - `Items` (IEnumerable<DeliveryItemOutputModel>)
    - `TotalCount` (int)
    - `PageNumber` (int)
    - `PageSize` (int)
    - `TotalPages` (int)
  - `DeliveryItemOutputModel` em `OutputModels/DeliveryManagement/DeliveryItemOutputModel.cs`:
    - `Id` (Guid)
    - `PreparationId` (Guid)
    - `OrderId` (Guid?)
    - `Status` (int)
    - `CreatedAt` (DateTime)
  - `FinalizeDeliveryOutputModel` em `OutputModels/DeliveryManagement/FinalizeDeliveryOutputModel.cs`:
    - `Id` (Guid)
    - `PreparationId` (Guid)
    - `OrderId` (Guid?)
    - `Status` (int)
    - `CreatedAt` (DateTime)
    - `FinalizedAt` (DateTime)
- Criar Responses:
  - `CreateDeliveryResponse` em `Responses/DeliveryManagement/CreateDeliveryResponse.cs`:
    - Propriedades do OutputModel
    - `Message` (string)
  - `GetReadyDeliveriesResponse` em `Responses/DeliveryManagement/GetReadyDeliveriesResponse.cs`:
    - Propriedades do OutputModel (mesma estrutura)
  - `FinalizeDeliveryResponse` em `Responses/DeliveryManagement/FinalizeDeliveryResponse.cs`:
    - Propriedades do OutputModel
    - `Message` (string)
- Adicionar documentação XML nas classes
- Namespaces:
  - `FastFood.KitchenFlow.Application.InputModels.DeliveryManagement`
  - `FastFood.KitchenFlow.Application.OutputModels.DeliveryManagement`
  - `FastFood.KitchenFlow.Application.Responses.DeliveryManagement`

## Referências
- **OrderHub**: Verificar estrutura de InputModels, OutputModels e Responses
- **Auth**: Verificar padrão de modelos
- Seguir organização horizontal por contexto (DeliveryManagement)

## Como testar
- Executar `dotnet build` no projeto Application para verificar compilação
- Verificar que modelos podem ser instanciados

## Critérios de aceite
- InputModels criados:
  - `CreateDeliveryInputModel` - PreparationId (Guid), OrderId (Guid?)
  - `GetReadyDeliveriesInputModel` - PageNumber (int), PageSize (int)
  - `FinalizeDeliveryInputModel` - Id (Guid)
- OutputModels criados:
  - `CreateDeliveryOutputModel` - todas as propriedades
  - `GetReadyDeliveriesOutputModel` - com paginação
  - `DeliveryItemOutputModel` - item da lista
  - `FinalizeDeliveryOutputModel` - com FinalizedAt
- Responses criados:
  - `CreateDeliveryResponse` - com Message
  - `GetReadyDeliveriesResponse` - estrutura de paginação
  - `FinalizeDeliveryResponse` - com Message
- Estrutura de pastas criada corretamente
- Projeto Application compila sem erros
- Namespaces corretos
- Estrutura segue padrão do OrderHub/Auth
