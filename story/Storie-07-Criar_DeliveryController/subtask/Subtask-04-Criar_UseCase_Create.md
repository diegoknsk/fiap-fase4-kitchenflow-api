# Subtask 04: Criar UseCase CreateDeliveryUseCase

## Descrição
Criar o UseCase `CreateDeliveryUseCase` na camada Application que orquestra a criação de uma Delivery, incluindo validações, verificação de que Preparation existe e está Finished, e verificação de idempotência.

## Passos de implementação
- Criar pasta `UseCases/DeliveryManagement/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar classe `CreateDeliveryUseCase` em `UseCases/DeliveryManagement/CreateDeliveryUseCase.cs`:
  - Construtor recebe `IDeliveryRepository` e `IPreparationRepository` (via DI)
  - Método `ExecuteAsync(CreateDeliveryInputModel inputModel)`:
    - Validar `inputModel`:
      - `PreparationId` não pode ser vazio
    - Verificar que Preparation existe:
      - Nota: Adicionar método `GetByIdAsync(Guid id)` ao `IPreparationRepository` se ainda não existir (pode ser necessário expandir a Story 06)
      - Chamar `preparationRepository.GetByIdAsync(inputModel.PreparationId)`
      - Se não existe, lançar exceção ou retornar erro
    - Verificar que Preparation está Finished:
      - Se Preparation.Status != EnumPreparationStatus.Finished, retornar erro
    - Verificar idempotência:
      - Chamar `deliveryRepository.GetByPreparationIdAsync(inputModel.PreparationId)`
      - Se já existe, retornar dados existentes (ou lançar exceção 409)
    - Criar entidade de domínio:
      - Usar factory method `Delivery.Create(inputModel.PreparationId, inputModel.OrderId)`
    - Persistir:
      - Chamar `deliveryRepository.CreateAsync(delivery)`
    - Criar OutputModel:
      - Mapear entidade de domínio para `CreateDeliveryOutputModel`
    - Chamar Presenter:
      - `presenter.Present(outputModel)` para obter Response
    - Retornar `CreateDeliveryResponse`
- Adicionar tratamento de erros (exceções de domínio, etc.)
- Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.UseCases.DeliveryManagement`

## Referências
- **OrderHub**: Verificar padrão de UseCases
- **Auth**: Verificar padrão de UseCases
- Seguir padrão: UseCase pequeno e focado, chama Ports, chama Presenter

## Observações importantes
- **Preparation deve existir**: Validar que Preparation existe antes de criar Delivery
- **Preparation deve estar Finished**: Só criar Delivery se Preparation está com status Finished
- **Idempotência**: Se Delivery já existe para a Preparation, decidir:
  - Opção 1: Retornar dados existentes (200 OK)
  - Opção 2: Retornar erro 409 Conflict
  - Recomendação: Retornar dados existentes (mais resiliente)
- **OrderId opcional**: Pode ser null, será obtido da Preparation se necessário

## Como testar
- Executar `dotnet build` no projeto Application
- (Opcional) Criar teste unitário básico mockando repositories

## Critérios de aceite
- Classe `CreateDeliveryUseCase` criada
- Construtor recebe `IDeliveryRepository` e `IPreparationRepository`
- Método `ExecuteAsync` implementado:
  - Valida InputModel
  - Verifica que Preparation existe
  - Verifica que Preparation está Finished
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
