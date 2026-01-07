# Subtask 01: Refatorar FinishPreparationUseCase

## Descrição
Refatorar o `FinishPreparationUseCase` para criar automaticamente o delivery quando a preparação é finalizada, seguindo o padrão do projeto antigo.

## Passos de implementação
- Abrir classe `FinishPreparationUseCase` em `Application/UseCases/PreparationManagement/FinishPreparationUseCase.cs`
- Adicionar `IDeliveryRepository` como dependência no construtor:
  - Parâmetro: `IDeliveryRepository deliveryRepository`
  - Armazenar em campo privado: `_deliveryRepository`
- Após finalizar a preparação (linha 64: `preparation.FinishPreparation()`):
  - Verificar se já existe delivery para esta preparation (idempotência):
    - Chamar `_deliveryRepository.GetByPreparationIdAsync(preparation.Id)`
    - Se já existir, usar o delivery existente
    - Se não existir, criar novo delivery:
      - Usar `Delivery.Create(preparation.Id, preparation.OrderId)`
      - Chamar `_deliveryRepository.CreateAsync(delivery)`
  - Armazenar o `DeliveryId` (do delivery existente ou recém-criado)
- Atualizar `FinishPreparationOutputModel` (linha 70-76):
  - Adicionar propriedade `DeliveryId` ao outputModel
  - Usar o `DeliveryId` obtido acima
- Verificar que validações existentes não foram alteradas
- Verificar que tratamento de exceções está correto

## Referências
- **Projeto antigo**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\UseCases\PreparationManagement\FinishPreparationUseCase.cs` (linhas 55-56)
- **CreateDeliveryUseCase**: Verificar lógica de criação de delivery e idempotência
- **Delivery.Create**: Método factory da entidade Delivery

## Observações importantes
- **Idempotência**: Sempre verificar se delivery já existe antes de criar
- **OrderId**: Usar `preparation.OrderId` ao criar o delivery (pode ser null)
- **Tratamento de exceções**: Se ocorrer erro ao criar delivery, a preparação já foi finalizada - considerar rollback ou tratamento adequado
- **Mantém compatibilidade**: Não alterar assinatura do método `ExecuteAsync`, apenas adicionar lógica interna
