# Subtask 03: Criar entidade Delivery com validações

## Descrição
Criar a entidade de domínio `Delivery` no projeto `FastFood.KitchenFlow.Domain` com todas as propriedades, métodos de negócio e validações necessárias. A entidade deve seguir o padrão de entidades ricas do OrderHub e adaptar a lógica do monolito para o novo modelo com `PreparationId` como FK obrigatória.

## Passos de implementação
- Criar classe `Delivery` em `Entities/DeliveryManagement/Delivery.cs`
- Implementar propriedades:
  - `Id` (Guid) - chave primária, privado setter
  - `PreparationId` (Guid) - FK obrigatória para Preparation.Id, privado setter
  - `OrderId` (Guid?) - ID do pedido (opcional, apenas para facilitar consulta), privado setter
  - `Status` (EnumDeliveryStatus) - status atual, privado setter
  - `CreatedAt` (DateTime) - data/hora de criação, privado setter
  - `FinalizedAt` (DateTime?) - data/hora de finalização (nullable), privado setter
- Implementar construtor privado sem parâmetros (para EF Core, se necessário)
- Implementar factory method estático `Create(Guid preparationId, Guid? orderId = null)`:
  - Validar que `preparationId` não é vazio (obrigatório)
  - Criar nova instância com `Id = Guid.NewGuid()`
  - Definir `Status = EnumDeliveryStatus.ReadyForPickup`
  - Definir `CreatedAt = DateTime.UtcNow`
  - Definir `FinalizedAt = null`
  - Retornar instância de Delivery
- Implementar método `FinalizeDelivery()`:
  - Validar que status atual é `ReadyForPickup`
  - Lançar exceção se status não permitir transição
  - Alterar status para `Finalized`
  - Definir `FinalizedAt = DateTime.UtcNow`
- Adicionar validações usando padrão similar ao monolito (DomainValidation ou exceções diretas)
- Referenciar enum `EnumDeliveryStatus` criado na Subtask 01
- Adicionar documentação XML na classe e métodos (opcional, mas recomendado)

## Referências
- **Monolito**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Domain\Entities\DeliveryManagement\Delivery.cs`
- **OrderHub**: `c:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Core\FastFood.OrderHub.Domain\Entities\OrderManagement\Order.cs`
- Adaptar padrão de validações e métodos de negócio

## Observações importantes
- **PreparationId é obrigatório**: Delivery sempre depende de uma Preparation existente
- **OrderId é opcional**: Apenas para facilitar consultas, não é o vínculo principal
- **Relacionamento**: O vínculo principal é `Delivery.PreparationId`, não `OrderId`
- **Sem navegação**: Delivery não tem propriedade de navegação para Preparation (seguindo padrão de desacoplamento)

## Como testar
- **Testes unitários** (serão criados na Subtask 05):
  - Teste de criação via `Create()` com `preparationId` válido
  - Teste de criação com `preparationId` vazio (deve lançar exceção)
  - Teste de criação com `orderId` opcional (deve funcionar com null)
  - Teste de transição `ReadyForPickup` → `Finalized` (válida)
  - Teste de transição inválida `Finalized` → `ReadyForPickup` (deve lançar exceção)
  - Teste de que `FinalizedAt` é definido ao finalizar
- **Compilação**: Executar `dotnet build` no projeto Domain
- **Validação manual**: Verificar que a entidade pode ser instanciada e métodos funcionam

## Critérios de aceite
- Classe `Delivery` criada em `Entities/DeliveryManagement/Delivery.cs`
- Propriedades implementadas com setters privados:
  - `Id` (Guid)
  - `PreparationId` (Guid) - obrigatório
  - `OrderId` (Guid?) - opcional
  - `Status` (EnumDeliveryStatus)
  - `CreatedAt` (DateTime)
  - `FinalizedAt` (DateTime?) - nullable
- Factory method `Create()` implementado com validações:
  - Valida que `preparationId` não é vazio
  - Aceita `orderId` opcional
- Método `FinalizeDelivery()` implementado:
  - Valida transição `ReadyForPickup` → `Finalized`
  - Define `FinalizedAt = DateTime.UtcNow`
  - Lança exceção para transições inválidas
- Validações de negócio implementadas (preparationId não vazio)
- Projeto Domain compila sem erros
- Nenhuma dependência externa (EF Core, ASP.NET, etc.)
- Código segue padrão do OrderHub (entidades ricas com métodos de negócio)
- Namespace: `FastFood.KitchenFlow.Domain.Entities.DeliveryManagement`
