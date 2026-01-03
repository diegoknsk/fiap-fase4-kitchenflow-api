# Storie-03: Criar Entidades de Domínio (Preparation e Delivery)

## Descrição
Como desenvolvedor, quero criar as entidades de domínio `Preparation` e `Delivery` com seus enums de status, validações de negócio e regras de transição de estado, para estabelecer a base do modelo de domínio do microserviço KitchenFlow seguindo os princípios da Clean Architecture.

## Objetivo
Criar as entidades de domínio `Preparation` e `Delivery` no projeto `FastFood.KitchenFlow.Domain`, incluindo:
- Entidade `Preparation` com propriedades, métodos de negócio e validações
- Entidade `Delivery` com propriedades, métodos de negócio e validações
- Enums `EnumPreparationStatus` e `EnumDeliveryStatus`
- Validações de domínio e regras de transição de estado
- Exceções de domínio quando necessário
- Garantir que as entidades não tenham dependências externas (EF Core, ASP.NET, etc.)

## Escopo Técnico
- **Tecnologias**: .NET 8, C# 12
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Domain` (entidades, enums, validações)
  - `FastFood.KitchenFlow.Tests.Unit` (testes unitários)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Domain\Entities\PreparationManagement\Preparation.cs`
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Domain\Entities\DeliveryManagement\Delivery.cs`
  - Adaptar para o novo modelo com `OrderSnapshot` e `PreparationId`
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Core\FastFood.OrderHub.Domain\Entities\OrderManagement\Order.cs` (padrão de entidade)
  - Seguir padrão de validações e métodos de negócio do OrderHub

## Modelo de Dados do Domínio

### Entidade Preparation
- `Id` (Guid) - Chave primária
- `OrderId` (Guid ou string) - ID do pedido (manter compatível com padrão do OrderHub)
- `Status` (EnumPreparationStatus) - Status atual da preparação
- `CreatedAt` (DateTime) - Data/hora de criação
- `OrderSnapshot` (string) - Snapshot JSON imutável do pedido no momento do pagamento
  - Armazenado como string no domínio (será mapeado para jsonb no banco)
  - Usado para exibição no display da cozinha, referência para entrega e auditoria

**Métodos de negócio:**
- `Create(Guid orderId, string orderSnapshot)` - Factory method para criar nova preparação
- `StartPreparation()` - Inicia a preparação (Received → InProgress)
- `FinishPreparation()` - Finaliza a preparação (InProgress → Finished)

**Validações:**
- OrderId não pode ser vazio
- OrderSnapshot não pode ser nulo ou vazio
- Validar transições de status permitidas

### Enum EnumPreparationStatus
- `Received = 0` - Pedido recebido após pagamento confirmado
- `InProgress = 1` - Pedido em preparação na cozinha
- `Finished = 2` - Preparação finalizada, pronto para entrega

### Entidade Delivery
- `Id` (Guid) - Chave primária
- `PreparationId` (Guid) - FK obrigatória para Preparation.Id
- `OrderId` (Guid ou string, opcional) - ID do pedido (apenas para facilitar consulta)
- `Status` (EnumDeliveryStatus) - Status atual da entrega
- `CreatedAt` (DateTime) - Data/hora de criação
- `FinalizedAt` (DateTime?) - Data/hora de finalização (nullable)

**Métodos de negócio:**
- `Create(Guid preparationId, Guid? orderId)` - Factory method para criar nova entrega
- `FinalizeDelivery()` - Finaliza a entrega (ReadyForPickup → Finalized)

**Validações:**
- PreparationId não pode ser vazio (obrigatório)
- Validar transições de status permitidas
- Delivery não pode existir sem Preparation

### Enum EnumDeliveryStatus
- `ReadyForPickup = 1` - Pronto para retirada
- `Finalized = 2` - Entrega finalizada

### Relacionamento
- Uma `Preparation` pode ter zero ou uma `Delivery`
- `Delivery` NÃO existe sem `Preparation`
- O vínculo principal é `Delivery.PreparationId`

## Regras de Negócio

### Preparation
1. **Criação**: Ao criar uma Preparation, o status inicial deve ser `Received`
2. **Transições de Status**:
   - `Received` → `InProgress` (via `StartPreparation()`)
   - `InProgress` → `Finished` (via `FinishPreparation()`)
   - Não permitir transições inválidas (ex: `Received` → `Finished`)
3. **OrderSnapshot**: Deve ser fornecido na criação e não pode ser alterado (imutável)

### Delivery
1. **Criação**: Ao criar uma Delivery, o status inicial deve ser `ReadyForPickup`
2. **Transições de Status**:
   - `ReadyForPickup` → `Finalized` (via `FinalizeDelivery()`)
   - Não permitir transições inválidas
3. **Dependência**: Delivery sempre depende de uma Preparation existente

## Subtasks

- [ ] [Subtask 01: Criar estrutura de pastas e enums no Domain](./subtask/Subtask-01-Criar_estrutura_pastas_enums.md)
- [ ] [Subtask 02: Criar entidade Preparation com validações](./subtask/Subtask-02-Criar_entidade_Preparation.md)
- [ ] [Subtask 03: Criar entidade Delivery com validações](./subtask/Subtask-03-Criar_entidade_Delivery.md)
- [ ] [Subtask 04: Criar exceções de domínio (se necessário)](./subtask/Subtask-04-Criar_excecoes_dominio.md)
- [ ] [Subtask 05: Criar testes unitários para entidades](./subtask/Subtask-05-Criar_testes_entidades.md)
- [ ] [Subtask 06: Validar compilação e estrutura de domínio](./subtask/Subtask-06-Validar_dominio.md)

## Critérios de Aceite da História

- [ ] Estrutura de pastas criada em `FastFood.KitchenFlow.Domain`:
  - `Entities/PreparationManagement/` com `Preparation.cs`
  - `Entities/DeliveryManagement/` com `Delivery.cs`
  - `Common/Enums/` com `EnumPreparationStatus.cs` e `EnumDeliveryStatus.cs`
  - `Common/Exceptions/` (se necessário para exceções de domínio)
- [ ] Entidade `Preparation` criada com:
  - Propriedades: `Id`, `OrderId`, `Status`, `CreatedAt`, `OrderSnapshot`
  - Métodos: `Create()`, `StartPreparation()`, `FinishPreparation()`
  - Validações de negócio implementadas
- [ ] Entidade `Delivery` criada com:
  - Propriedades: `Id`, `PreparationId`, `OrderId` (opcional), `Status`, `CreatedAt`, `FinalizedAt`
  - Métodos: `Create()`, `FinalizeDelivery()`
  - Validações de negócio implementadas
- [ ] Enums criados com valores corretos:
  - `EnumPreparationStatus`: `Received = 0`, `InProgress = 1`, `Finished = 2`
  - `EnumDeliveryStatus`: `ReadyForPickup = 1`, `Finalized = 2`
- [ ] Validações de transição de status implementadas:
  - Preparation: `Received` → `InProgress` → `Finished`
  - Delivery: `ReadyForPickup` → `Finalized`
  - Exceções lançadas para transições inválidas
- [ ] Testes unitários criados cobrindo:
  - Criação de entidades
  - Transições de status válidas
  - Transições de status inválidas (deve lançar exceção)
  - Validações de propriedades obrigatórias
- [ ] Projeto Domain compila sem erros
- [ ] Nenhuma dependência externa no projeto Domain (EF Core, ASP.NET, etc.)
- [ ] Código segue padrão do projeto de referência (OrderHub)
- [ ] Testes unitários passando (`dotnet test`)

## Observações Arquiteturais

### Clean Architecture
- **Domain Layer**: Contém apenas lógica de negócio pura
- **Zero dependências externas**: Domain não referencia EF Core, ASP.NET, ou qualquer framework
- **Entidades ricas**: Entidades contêm métodos de negócio, não apenas propriedades
- **Validações no domínio**: Regras de negócio e validações ficam nas entidades

### Padrões Aplicados
- **Factory Methods**: Métodos estáticos `Create()` para criação de entidades
- **Encapsulamento**: Propriedades privadas com setters privados, métodos públicos para alterações
- **Domain-Driven Design**: Entidades representam conceitos do domínio (Preparation, Delivery)
- **Invariantes**: Validações garantem que entidades sempre estão em estado válido

### Diferenças do Monolito
- **OrderSnapshot**: Novo campo em Preparation para armazenar snapshot imutável do pedido
- **PreparationId em Delivery**: FK obrigatória, relacionamento explícito
- **OrderId opcional em Delivery**: Apenas para facilitar consultas, não é o vínculo principal
- **Sem navegação para Order**: KitchenFlow não conhece a entidade Order do OrderHub

### Análise dos Projetos de Referência

**OrderHub (Order.cs):**
- Entidade rica com métodos de negócio (`AddProduct`, `RemoveProduct`, `CalculateTotalPrice`)
- Propriedades com setters privados
- Métodos que alteram estado interno
- Validações dentro dos métodos

**Monolito (Preparation.cs e Delivery.cs):**
- Padrão similar com métodos `StartPreparation()`, `FinishPreparation()`, `FinalizeDelivery()`
- Uso de `DomainValidation.ThrowIf()` para validações
- Status como enum com transições controladas

**Aplicação no KitchenFlow:**
- Seguir padrão de entidades ricas do OrderHub
- Adaptar validações do monolito para o novo modelo
- Adicionar suporte a `OrderSnapshot` (string no domínio, jsonb no banco)
- Manter relacionamento `Delivery.PreparationId` como FK obrigatória
