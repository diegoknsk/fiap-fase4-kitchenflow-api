# Subtask 04: Criar UseCase GetPreparationsUseCase

## Descrição
Criar o UseCase `GetPreparationsUseCase` na camada Application que busca preparações com paginação e filtro opcional por status.

## Passos de implementação
- Criar classe `GetPreparationsUseCase` em `UseCases/PreparationManagement/GetPreparationsUseCase.cs`:
  - Construtor recebe `IPreparationRepository` (via DI)
  - Método `ExecuteAsync(GetPreparationsInputModel inputModel)`:
    - Validar `inputModel`:
      - `PageNumber` deve ser >= 1
      - `PageSize` deve ser >= 1 e <= 100 (limite máximo)
      - `Status` pode ser null (opcional)
    - Buscar preparações:
      - Chamar `preparationRepository.GetPagedAsync(inputModel.PageNumber, inputModel.PageSize, inputModel.Status)`
      - Receber tupla `(IEnumerable<Preparation>, int totalCount)`
    - Mapear para OutputModel:
      - Converter cada `Preparation` para `PreparationItemOutputModel`
      - Incluir `OrderSnapshot` no item (para exibição no display)
      - Calcular `TotalPages = (int)Math.Ceiling(totalCount / (double)inputModel.PageSize)`
      - Criar `GetPreparationsOutputModel` com:
        - `Items` (lista de PreparationItemOutputModel)
        - `TotalCount` (totalCount)
        - `PageNumber` (inputModel.PageNumber)
        - `PageSize` (inputModel.PageSize)
        - `TotalPages` (calculado)
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `GetPreparationsResponse`
- Adicionar tratamento de erros
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases com paginação
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Paginação**: Validar parâmetros de paginação (PageNumber >= 1, PageSize entre 1 e 100)
- **Filtro**: Status é opcional (null = sem filtro)
- **TotalPages**: Calcular corretamente usando Math.Ceiling
- **OrderSnapshot**: Incluir na resposta para exibição no display da cozinha

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `GetPreparationsUseCase` criada
- Construtor recebe `IPreparationRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel (PageNumber, PageSize, Status opcional)
  - Busca preparações via repository
  - Mapeia para OutputModel
  - Calcula TotalPages
  - Inclui OrderSnapshot nos items
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
