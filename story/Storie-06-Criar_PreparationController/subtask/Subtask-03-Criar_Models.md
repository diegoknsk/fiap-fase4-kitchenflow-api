# Subtask 03: Criar InputModel, OutputModel e Response

## Descrição
Criar os modelos de dados na camada Application: InputModel (entrada do UseCase), OutputModel (saída do UseCase) e Response (retorno da API).

## Passos de implementação
- Criar estrutura de pastas no projeto `FastFood.KitchenFlow.Application`:
  - `InputModels/PreparationManagement/`
  - `OutputModels/PreparationManagement/`
  - `Responses/PreparationManagement/`
- Criar `CreatePreparationInputModel` em `InputModels/PreparationManagement/CreatePreparationInputModel.cs`:
  - `OrderId` (Guid) - ID do pedido
  - `OrderSnapshot` (string) - Snapshot JSON do pedido
- Criar `CreatePreparationOutputModel` em `OutputModels/PreparationManagement/CreatePreparationOutputModel.cs`:
  - `Id` (Guid) - ID da preparação criada
  - `OrderId` (Guid) - ID do pedido
  - `Status` (int) - Status da preparação (EnumPreparationStatus)
  - `CreatedAt` (DateTime) - Data/hora de criação
- Criar `CreatePreparationResponse` em `Responses/PreparationManagement/CreatePreparationResponse.cs`:
  - Propriedades do OutputModel
  - `Message` (string) - Mensagem de sucesso
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
- `CreatePreparationInputModel` criado:
  - `OrderId` (Guid)
  - `OrderSnapshot` (string)
- `CreatePreparationOutputModel` criado:
  - `Id` (Guid)
  - `OrderId` (Guid)
  - `Status` (int)
  - `CreatedAt` (DateTime)
- `CreatePreparationResponse` criado:
  - Propriedades do OutputModel
  - `Message` (string)
- Estrutura de pastas criada corretamente
- Projeto Application compila sem erros
- Namespaces corretos
- Estrutura segue padrão do OrderHub/Auth
