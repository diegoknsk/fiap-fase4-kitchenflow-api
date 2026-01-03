# Subtask 05: Criar UseCase StartPreparationUseCase

## Descrição
Criar o UseCase `StartPreparationUseCase` na camada Application que inicia uma preparação (Received → InProgress), incluindo validações de status.

## Passos de implementação
- Criar classe `StartPreparationUseCase` em `UseCases/PreparationManagement/StartPreparationUseCase.cs`:
  - Construtor recebe `IPreparationRepository` (via DI)
  - Método `ExecuteAsync(StartPreparationInputModel inputModel)`:
    - Validar `inputModel`:
      - `Id` não pode ser vazio
    - Buscar Preparation:
      - Chamar `preparationRepository.GetByIdAsync(inputModel.Id)`
      - Se não existe, retornar erro 404
    - Validar status:
      - Se Preparation.Status != EnumPreparationStatus.Received, retornar erro 400
    - Iniciar Preparation:
      - Chamar método de domínio `preparation.StartPreparation()`
      - Isso altera status para `InProgress`
    - Atualizar no banco:
      - Chamar `preparationRepository.UpdateAsync(preparation)`
    - Criar OutputModel:
      - Mapear entidade de domínio para `StartPreparationOutputModel`
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `StartPreparationResponse`
- Adicionar tratamento de erros (exceções de domínio, etc.)
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases
- **Auth**: Verificar padrão de UseCases
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Validação de status**: Só pode iniciar se status é `Received`
- **Método de domínio**: Usar `preparation.StartPreparation()` que já valida e altera status
- **Transição**: `Received` → `InProgress`

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `StartPreparationUseCase` criada
- Construtor recebe `IPreparationRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel
  - Busca Preparation
  - Valida que Preparation existe (404 se não existe)
  - Valida status (400 se status inválido)
  - Chama método de domínio `StartPreparation()`
  - Atualiza via repository
  - Cria OutputModel
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
