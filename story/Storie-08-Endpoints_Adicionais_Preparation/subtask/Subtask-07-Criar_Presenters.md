# Subtask 07: Criar Presenters

## Descrição
Criar os Presenters na camada Application que transformam os OutputModels em Responses para os três novos UseCases de Preparation.

## Passos de implementação
- Criar classe `GetPreparationsPresenter` em `Presenters/PreparationManagement/GetPreparationsPresenter.cs`:
  - Método estático `Present(GetPreparationsOutputModel outputModel)`:
    - Recebe `GetPreparationsOutputModel`
    - Cria `GetPreparationsResponse`:
      - Mapear propriedades do OutputModel (mesma estrutura)
    - Retornar `GetPreparationsResponse`
- Criar classe `StartPreparationPresenter` em `Presenters/PreparationManagement/StartPreparationPresenter.cs`:
  - Método estático `Present(StartPreparationOutputModel outputModel)`:
    - Recebe `StartPreparationOutputModel`
    - Cria `StartPreparationResponse`:
      - Mapear propriedades do OutputModel
      - Definir `Message = "Preparação iniciada com sucesso"`
    - Retornar `StartPreparationResponse`
- Criar classe `FinishPreparationPresenter` em `Presenters/PreparationManagement/FinishPreparationPresenter.cs`:
  - Método estático `Present(FinishPreparationOutputModel outputModel)`:
    - Recebe `FinishPreparationOutputModel`
    - Cria `FinishPreparationResponse`:
      - Mapear propriedades do OutputModel
      - Definir `Message = "Preparação finalizada com sucesso"`
    - Retornar `FinishPreparationResponse`
- Adicionar documentação XML nas classes
- Namespace: `FastFood.KitchenFlow.Application.Presenters.PreparationManagement`

## Referências
- **OrderHub**: Verificar padrão de Presenters
- **Auth**: Verificar padrão de Presenters
- Por padrão, Presenters fazem mapeamento direto, mas podem fazer transformações quando necessário

## Como testar
- Executar `dotnet build` no projeto Application
- Verificar que Presenters podem ser chamados

## Critérios de aceite
- Classe `GetPreparationsPresenter` criada:
  - Método `Present` recebe `GetPreparationsOutputModel`
  - Retorna `GetPreparationsResponse`
- Classe `StartPreparationPresenter` criada:
  - Método `Present` recebe `StartPreparationOutputModel`
  - Retorna `StartPreparationResponse` com Message
- Classe `FinishPreparationPresenter` criada:
  - Método `Present` recebe `FinishPreparationOutputModel`
  - Retorna `FinishPreparationResponse` com Message
- Projeto Application compila sem erros
- Namespace correto
- Presenters seguem padrão do OrderHub/Auth
