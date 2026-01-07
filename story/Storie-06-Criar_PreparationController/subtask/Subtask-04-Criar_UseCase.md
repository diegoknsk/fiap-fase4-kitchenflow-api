# Subtask 04: Criar UseCase CreatePreparationUseCase

## Descrição
Criar o UseCase `CreatePreparationUseCase` na camada Application que orquestra a criação de uma Preparation, incluindo validações, verificação de idempotência e chamada ao repository.

## Passos de implementação
- Criar pasta `UseCases/PreparationManagement/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar classe `CreatePreparationUseCase` em `UseCases/PreparationManagement/CreatePreparationUseCase.cs`:
  - Construtor recebe `IPreparationRepository` (via DI)
  - Método `ExecuteAsync(CreatePreparationInputModel inputModel)`:
    - Validar `inputModel`:
      - `OrderId` não pode ser vazio
      - `OrderSnapshot` não pode ser nulo ou vazio
    - Verificar idempotência:
      - Chamar `repository.GetByOrderIdAsync(inputModel.OrderId)`
      - Se já existe, retornar dados existentes (ou lançar exceção)
    - Criar entidade de domínio:
      - Usar factory method `Preparation.Create(inputModel.OrderId, inputModel.OrderSnapshot)`
    - Persistir:
      - Chamar `repository.CreateAsync(preparation)`
    - Criar OutputModel:
      - Mapear entidade de domínio para `CreatePreparationOutputModel`
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `CreatePreparationResponse`
- Adicionar tratamento de erros (exceções de domínio, etc.)
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases (ex: `StartOrderUseCase`)
- **Auth**: Verificar padrão de UseCases
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Idempotência**: Se Preparation já existe para o OrderId, decidir:
  - Opção 1: Retornar dados existentes (200 OK)
  - Opção 2: Retornar erro 409 Conflict
  - Recomendação: Retornar dados existentes (mais resiliente)
- **Validações**: Validações de negócio ficam na entidade de domínio, validações básicas no UseCase

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `CreatePreparationUseCase` criada
- Construtor recebe `IPreparationRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel
  - Verifica idempotência
  - Cria entidade de domínio
  - Persiste via repository
  - Cria OutputModel
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
