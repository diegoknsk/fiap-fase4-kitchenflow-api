# Storie-04: Criar Camada de Persistência e Migrations (Preparations + Deliveries)

## Descrição
Como desenvolvedor, quero criar a camada de persistência com Entity Framework Core para PostgreSQL, incluindo entidades de persistência, configurações de mapeamento, DbContext e migrations iniciais para as tabelas `Preparations` e `Deliveries`, para que possamos persistir os dados do microserviço KitchenFlow no banco de dados.

## Objetivo
Criar a camada de persistência completa no projeto `FastFood.KitchenFlow.Infra.Persistence`, incluindo:
- Entidades de persistência (`PreparationEntity`, `DeliveryEntity`) separadas das entidades de domínio
- Configurações de mapeamento (EntityTypeConfiguration) para cada entidade
- DbContext configurado para PostgreSQL
- Migration inicial criando as tabelas `Preparations` e `Deliveries` com relacionamento FK
- Configuração de `OrderSnapshot` como tipo `jsonb` no PostgreSQL
- Índices apropriados para otimização de consultas

## Escopo Técnico
- **Tecnologias**: .NET 8, Entity Framework Core, Npgsql.EntityFrameworkCore.PostgreSQL
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Infra.Persistence` (entidades, configurações, DbContext, migrations)
  - `FastFood.KitchenFlow.Api` (configuração de DbContext no Program.cs)
  - `FastFood.KitchenFlow.Migrator` (será ajustado na próxima story)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\Core\FastFood.Auth.Infra.Persistence\` (padrão completo)
  - Seguir estrutura: Entities, Configurations, DbContext, Migrations
- **Banco de dados**: PostgreSQL local
  - Host: localhost
  - Porta: 4433
  - Database: dbKitchenLocal
  - User: postgres
  - Password: postgres

## Modelo de Dados de Persistência

### Tabela Preparations
- `Id` (Guid, PK)
- `OrderId` (Guid ou string) - ID do pedido (compatível com OrderHub)
- `Status` (int) - EnumPreparationStatus (0=Received, 1=InProgress, 2=Finished)
- `CreatedAt` (timestamp with time zone)
- `OrderSnapshot` (jsonb) - Snapshot imutável do pedido no momento do pagamento
- **Índices**: OrderId (para consultas rápidas)

### Tabela Deliveries
- `Id` (Guid, PK)
- `PreparationId` (Guid, FK obrigatória → Preparations.Id)
- `OrderId` (Guid ou string, nullable) - ID do pedido (opcional, apenas para facilitar consulta)
- `Status` (int) - EnumDeliveryStatus (1=ReadyForPickup, 2=Finalized)
- `CreatedAt` (timestamp with time zone)
- `FinalizedAt` (timestamp with time zone, nullable)
- **Índices**: PreparationId (FK), OrderId (opcional, para consultas)
- **Relacionamento**: FK `PreparationId` → `Preparations.Id` (ON DELETE RESTRICT ou CASCADE conforme regra de negócio)

### Relacionamento
- Uma `Preparation` pode ter zero ou uma `Delivery`
- `Delivery` NÃO existe sem `Preparation`
- FK obrigatória: `Delivery.PreparationId` → `Preparation.Id`

## Subtasks

- [x] [Subtask 01: Configurar dependências EF Core e Npgsql](./subtask/Subtask-01-Configurar_dependencias_EF.md)
- [x] [Subtask 02: Criar entidades de persistência (PreparationEntity, DeliveryEntity)](./subtask/Subtask-02-Criar_entidades_persistencia.md)
- [x] [Subtask 03: Criar configurações de mapeamento (EntityTypeConfiguration)](./subtask/Subtask-03-Criar_configuracoes_mapeamento.md)
- [x] [Subtask 04: Criar DbContext (KitchenFlowDbContext)](./subtask/Subtask-04-Criar_DbContext.md)
- [x] [Subtask 05: Configurar DbContext no Program.cs da API](./subtask/Subtask-05-Configurar_DbContext_API.md)
- [x] [Subtask 06: Criar migration inicial (Preparations + Deliveries + FK)](./subtask/Subtask-06-Criar_migration_inicial.md)
- [x] [Subtask 07: Validar estrutura de persistência](./subtask/Subtask-07-Validar_persistencia.md)

## Critérios de Aceite da História

- [x] Projeto `FastFood.KitchenFlow.Infra.Persistence` possui dependências:
  - `Microsoft.EntityFrameworkCore`
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
  - Referência a `FastFood.KitchenFlow.Domain`
- [x] Entidade `PreparationEntity` criada em `Entities/PreparationEntity.cs`:
  - Propriedades: `Id`, `OrderId`, `Status`, `CreatedAt`, `OrderSnapshot`
  - `OrderSnapshot` como `string` (será mapeado para jsonb)
- [x] Entidade `DeliveryEntity` criada em `Entities/DeliveryEntity.cs`:
  - Propriedades: `Id`, `PreparationId`, `OrderId` (nullable), `Status`, `CreatedAt`, `FinalizedAt`
- [x] Configuração `PreparationConfiguration` criada:
  - Mapeia para tabela `Preparations`
  - `OrderSnapshot` configurado como `jsonb` no PostgreSQL
  - Índice em `OrderId`
- [x] Configuração `DeliveryConfiguration` criada:
  - Mapeia para tabela `Deliveries`
  - FK `PreparationId` → `Preparations.Id` configurada
  - Índices em `PreparationId` e `OrderId` (se necessário)
- [x] `KitchenFlowDbContext` criado:
  - DbSet para `PreparationEntity` e `DeliveryEntity`
  - Configurações aplicadas via `ApplyConfigurationsFromAssembly` ou manualmente
- [x] DbContext configurado no `Program.cs` da API:
  - Connection string lida de `appsettings.json`
  - `AddDbContext<KitchenFlowDbContext>` configurado
  - Connection string: `Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres`
- [x] Migration inicial criada:
  - Tabela `Preparations` com todas as colunas
  - Tabela `Deliveries` com todas as colunas
  - FK `Delivery.PreparationId` → `Preparation.Id`
  - Tipo `jsonb` para `OrderSnapshot`
  - Índices criados
- [x] Projeto compila sem erros
- [x] Estrutura segue padrão do projeto Auth (Entities, Configurations, DbContext, Migrations)

## Observações Arquiteturais

### Clean Architecture
- **Separação de entidades**: Entidades de persistência (`PreparationEntity`, `DeliveryEntity`) são **separadas** das entidades de domínio (`Preparation`, `Delivery`)
- **DbContext isolado**: DbContext nunca é acessado fora da camada `Infra.Persistence`
- **Mapeamento**: O repositório (a ser criado) será responsável por mapear entre entidades de domínio e entidades de persistência

### Padrões Aplicados
- **Entity Framework Core**: Usado para ORM e migrations
- **Fluent API**: Configurações de mapeamento usando `IEntityTypeConfiguration<T>`
- **PostgreSQL**: Tipo `jsonb` para `OrderSnapshot` (suporta consultas JSON)
- **Índices**: Criados para otimizar consultas frequentes (OrderId, PreparationId)

### Diferenças do Monolito
- **OrderSnapshot**: Novo campo em `Preparations` como `jsonb` (não existia no monolito)
- **PreparationId em Delivery**: FK obrigatória, relacionamento explícito
- **OrderId opcional em Delivery**: Apenas para facilitar consultas
- **Sem navegação para Order**: KitchenFlow não conhece a entidade Order do OrderHub

### Análise dos Projetos de Referência

**Auth (AuthDbContext.cs, CustomerConfiguration.cs):**
- DbContext simples com DbSet
- Configurações usando `IEntityTypeConfiguration<T>`
- Aplicação de configurações via `ApplyConfiguration` ou `ApplyConfigurationsFromAssembly`
- Entidades de persistência separadas das entidades de domínio

**Monolito (FastFoodDbContext.cs, PreparationEntity.cs, DeliveryEntity.cs):**
- Estrutura similar, mas sem `OrderSnapshot`
- Preparations e Deliveries já existiam, mas sem relacionamento explícito

**Aplicação no KitchenFlow:**
- Seguir padrão do Auth (estrutura limpa e organizada)
- Adicionar suporte a `OrderSnapshot` como `jsonb`
- Criar relacionamento FK `Delivery.PreparationId` → `Preparation.Id`
- Manter separação entre entidades de domínio e persistência

### Configuração de Connection String

A connection string deve ser configurada em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
  }
}
```

E também em `appsettings.Development.json` (não commitado):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
  }
}
```
