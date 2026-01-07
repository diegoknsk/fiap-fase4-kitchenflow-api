# Subtask 03: Criar InputModels, OutputModels e Responses

## Descrição
Criar os modelos de dados na camada Application: InputModels (entrada dos UseCases), OutputModels (saída dos UseCases) e Responses (retorno da API) para os três novos endpoints de Preparation.

## Passos de implementação
- Criar InputModels em `InputModels/PreparationManagement/`:
  - `GetPreparationsInputModel.cs`:
    - `PageNumber` (int) - default: 1
    - `PageSize` (int) - default: 10
    - `Status` (int?) - filtro opcional por status
  - `StartPreparationInputModel.cs`:
    - `Id` (Guid) - ID da preparação
  - `FinishPreparationInputModel.cs`:
    - `Id` (Guid) - ID da preparação
- Criar OutputModels em `OutputModels/PreparationManagement/`:
  - `GetPreparationsOutputModel.cs`:
    - `Items` (IEnumerable<PreparationItemOutputModel>)
    - `TotalCount` (int)
    - `PageNumber` (int)
    - `PageSize` (int)
    - `TotalPages` (int)
  - `PreparationItemOutputModel.cs`:
    - `Id` (Guid)
    - `OrderId` (Guid)
    - `Status` (int)
    - `CreatedAt` (DateTime)
    - `OrderSnapshot` (string) - incluído para exibição
  - `StartPreparationOutputModel.cs`:
    - `Id` (Guid)
    - `OrderId` (Guid)
    - `Status` (int)
    - `CreatedAt` (DateTime)
  - `FinishPreparationOutputModel.cs`:
    - `Id` (Guid)
    - `OrderId` (Guid)
    - `Status` (int)
    - `CreatedAt` (DateTime)
- Criar Responses em `Responses/PreparationManagement/`:
  - `GetPreparationsResponse.cs`:
    - Propriedades do OutputModel (mesma estrutura)
  - `StartPreparationResponse.cs`:
    - Propriedades do OutputModel
    - `Message` (string)
  - `FinishPreparationResponse.cs`:
    - Propriedades do OutputModel
    - `Message` (string)
- Adicionar documentação XML nas classes
- Namespaces:
  - `FastFood.KitchenFlow.Application.InputModels.PreparationManagement`
  - `FastFood.KitchenFlow.Application.OutputModels.PreparationManagement`
  - `FastFood.KitchenFlow.Application.Responses.PreparationManagement`

## Referências
- **OrderHub**: Verificar estrutura de InputModels, OutputModels e Responses
- **Auth**: Verificar padrão de modelos
- Seguir organização horizontal por contexto (PreparationManagement)

## Como testar
- Executar `dotnet build` no projeto Application para verificar compilação
- Verificar que modelos podem ser instanciados

## Critérios de aceite
- InputModels criados:
  - `GetPreparationsInputModel` - PageNumber, PageSize, Status (opcional)
  - `StartPreparationInputModel` - Id
  - `FinishPreparationInputModel` - Id
- OutputModels criados:
  - `GetPreparationsOutputModel` - com paginação
  - `PreparationItemOutputModel` - com OrderSnapshot
  - `StartPreparationOutputModel` - todas as propriedades
  - `FinishPreparationOutputModel` - todas as propriedades
- Responses criados:
  - `GetPreparationsResponse` - estrutura de paginação
  - `StartPreparationResponse` - com Message
  - `FinishPreparationResponse` - com Message
- Estrutura de pastas criada corretamente
- Projeto Application compila sem erros
- Namespaces corretos
- Estrutura segue padrão do OrderHub/Auth
