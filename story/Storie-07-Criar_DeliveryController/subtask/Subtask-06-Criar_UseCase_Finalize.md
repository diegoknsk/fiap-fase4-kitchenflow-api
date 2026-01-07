# Subtask 06: Criar UseCase FinalizeDeliveryUseCase

## Descrição
Criar o UseCase `FinalizeDeliveryUseCase` na camada Application que finaliza uma Delivery (ReadyForPickup → Finalized), incluindo validações de status.

## Passos de implementação
- Criar classe `FinalizeDeliveryUseCase` em `UseCases/DeliveryManagement/FinalizeDeliveryUseCase.cs`:
  - Construtor recebe `IDeliveryRepository` (via DI)
  - Método `ExecuteAsync(FinalizeDeliveryInputModel inputModel)`:
    - Validar `inputModel`:
      - `Id` não pode ser vazio
    - Buscar Delivery:
      - Chamar `deliveryRepository.GetByIdAsync(inputModel.Id)`
      - Se não existe, retornar erro 404
    - Validar status:
      - Se Delivery.Status != EnumDeliveryStatus.ReadyForPickup, retornar erro 400
    - Finalizar Delivery:
      - Chamar método de domínio `delivery.FinalizeDelivery()`
      - Isso altera status para `Finalized` e define `FinalizedAt`
    - Atualizar no banco:
      - Chamar `deliveryRepository.UpdateAsync(delivery)`
    - Criar OutputModel:
      - Mapear entidade de domínio para `FinalizeDeliveryOutputModel`
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `FinalizeDeliveryResponse`
- Adicionar tratamento de erros (exceções de domínio, etc.)
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.DeliveryManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases
- **Auth**: Verificar padrão de UseCases
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Validação de status**: Só pode finalizar se status é `ReadyForPickup`
- **Método de domínio**: Usar `delivery.FinalizeDelivery()` que já valida e altera status
- **FinalizedAt**: Será definido automaticamente pelo método de domínio

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repository

## Critérios de aceite
- Classe `FinalizeDeliveryUseCase` criada
- Construtor recebe `IDeliveryRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel
  - Busca Delivery
  - Valida que Delivery existe (404 se não existe)
  - Valida status (400 se status inválido)
  - Chama método de domínio `FinalizeDelivery()`
  - Atualiza via repository
  - Cria OutputModel
  - Chama Presenter
  - Retorna Response
- Tratamento de erros implementado
- Projeto Application compila sem erros
- Namespace correto
- UseCase segue padrão do OrderHub/Auth
