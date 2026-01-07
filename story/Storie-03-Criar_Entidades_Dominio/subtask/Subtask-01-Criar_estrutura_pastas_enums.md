# Subtask 01: Criar estrutura de pastas e enums no Domain

## Descrição
Criar a estrutura de pastas no projeto `FastFood.KitchenFlow.Domain` e implementar os enums `EnumPreparationStatus` e `EnumDeliveryStatus` que serão usados pelas entidades de domínio.

## Passos de implementação
- Criar estrutura de pastas no projeto `FastFood.KitchenFlow.Domain`:
  - `Entities/PreparationManagement/` (para entidade Preparation)
  - `Entities/DeliveryManagement/` (para entidade Delivery)
  - `Common/Enums/` (para enums compartilhados)
  - `Common/Exceptions/` (para exceções de domínio, se necessário)
- Criar enum `EnumPreparationStatus` em `Common/Enums/EnumPreparationStatus.cs`:
  - `Received = 0` - Pedido recebido após pagamento confirmado
  - `InProgress = 1` - Pedido em preparação na cozinha
  - `Finished = 2` - Preparação finalizada, pronto para entrega
- Criar enum `EnumDeliveryStatus` em `Common/Enums/EnumDeliveryStatus.cs`:
  - `ReadyForPickup = 1` - Pronto para retirada
  - `Finalized = 2` - Entrega finalizada
- Verificar que os enums seguem o padrão do projeto de referência (OrderHub)
- Adicionar documentação XML nos enums (opcional, mas recomendado)

## Como testar
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Domain` para verificar compilação
- Verificar que os enums estão acessíveis e podem ser usados
- Validar que os valores dos enums correspondem aos valores esperados

## Critérios de aceite
- Estrutura de pastas criada:
  - `Entities/PreparationManagement/`
  - `Entities/DeliveryManagement/`
  - `Common/Enums/`
  - `Common/Exceptions/` (pasta criada, mesmo que vazia por enquanto)
- Enum `EnumPreparationStatus` criado com valores:
  - `Received = 0`
  - `InProgress = 1`
  - `Finished = 2`
- Enum `EnumDeliveryStatus` criado com valores:
  - `ReadyForPickup = 1`
  - `Finalized = 2`
- Projeto Domain compila sem erros
- Namespaces seguem padrão: `FastFood.KitchenFlow.Domain.Common.Enums`
- Enums podem ser referenciados em outros projetos (quando necessário)
