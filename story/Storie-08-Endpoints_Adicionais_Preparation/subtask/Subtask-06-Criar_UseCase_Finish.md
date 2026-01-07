# Subtask 06: Criar UseCase FinishPreparationUseCase

## Descrição
Criar o UseCase `FinishPreparationUseCase` na camada Application que finaliza uma preparação (InProgress → Finished), incluindo validações de status.

## Passos de implementação
- Criar classe `FinishPreparationUseCase` em `UseCases/PreparationManagement/FinishPreparationUseCase.cs`:
  - Construtor recebe `IPreparationRepository` (via DI)
  - Método `ExecuteAsync(FinishPreparationInputModel inputModel)`:
    - Validar `inputModel`:
      - `Id` não pode ser vazio
    - Buscar Preparation:
      - Chamar `preparationRepository.GetByIdAsync(inputModel.Id)`
      - Se não existe, retornar erro 404
    - Validar status:
      - Se Preparation.Status != EnumPreparationStatus.InProgress, retornar erro 400
    - Finalizar Preparation:
      - Chamar método de domínio `preparation.FinishPreparation()`
      - Isso altera status para `Finished`
    - Atualizar no banco:
      - Chamar `preparationRepository.UpdateAsync(preparation)`
    - Criar OutputModel:
      - Mapear entidade de domínio para `FinishPreparationOutputModel`
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `FinishPreparationResponse`
- Adicionar tratamento de erros (exceções de domínio, etc.)
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases
- **Auth**: Verificar padrão de UseCases
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Validação de status**: Só pode finalizar se status é `InProgress`
- **Método de domínio**: Usar `preparation.FinishPreparation()` que já valida e altera status
- **Transição**: `InProgress` → `Finished`
- **Delivery**: Após finalizar, pode-se criar Delivery (não automático nesta story)

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `FinishPreparationUseCase` criada
- Construtor recebe `IPreparationRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel
  - Busca Preparation
  - Valida que Preparation existe (404 se não existe)
  - Valida status (400 se status inválido)
  - Chama método de domínio `FinishPreparation()`
  - Atualiza via repository
  - Cria OutputModel
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
