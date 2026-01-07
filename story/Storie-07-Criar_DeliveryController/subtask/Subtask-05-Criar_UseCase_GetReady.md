# Subtask 05: Criar UseCase GetReadyDeliveriesUseCase

## Descrição
Criar o UseCase `GetReadyDeliveriesUseCase` na camada Application que busca deliveries prontas para retirada (status ReadyForPickup) com paginação.

## Passos de implementação
- Criar classe `GetReadyDeliveriesUseCase` em `UseCases/DeliveryManagement/GetReadyDeliveriesUseCase.cs`:
  - Construtor recebe `IDeliveryRepository` (via DI)
  - Método `ExecuteAsync(GetReadyDeliveriesInputModel inputModel)`:
    - Validar `inputModel`:
      - `PageNumber` deve ser >= 1
      - `PageSize` deve ser >= 1 e <= 100 (limite máximo)
    - Buscar deliveries:
      - Chamar `deliveryRepository.GetReadyDeliveriesAsync(inputModel.PageNumber, inputModel.PageSize)`
      - Receber tupla `(IEnumerable<Delivery>, int totalCount)`
    - Mapear para OutputModel:
      - Converter cada `Delivery` para `DeliveryItemOutputModel`
      - Calcular `TotalPages = (int)Math.Ceiling(totalCount / (double)inputModel.PageSize)`
      - Criar `GetReadyDeliveriesOutputModel` com:
        - `Items` (lista de DeliveryItemOutputModel)
        - `TotalCount` (totalCount)
        - `PageNumber` (inputModel.PageNumber)
        - `PageSize` (inputModel.PageSize)
        - `TotalPages` (calculado)
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `GetReadyDeliveriesResponse`
- Adicionar tratamento de erros
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.DeliveryManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases com paginação
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Paginação**: Validar parâmetros de paginação (PageNumber >= 1, PageSize entre 1 e 100)
- **TotalPages**: Calcular corretamente usando Math.Ceiling
- **Filtro**: Repository já filtra por status ReadyForPickup

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `GetReadyDeliveriesUseCase` criada
- Construtor recebe `IDeliveryRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel (PageNumber, PageSize)
  - Busca deliveries via repository
  - Mapeia para OutputModel
  - Calcula TotalPages
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
