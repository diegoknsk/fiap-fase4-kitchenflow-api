# Subtask 06: Criar migration inicial (Preparations + Deliveries + FK)

## Descrição
Criar a migration inicial do Entity Framework Core que cria as tabelas `Preparations` e `Deliveries` no PostgreSQL, incluindo todas as colunas, tipos de dados, constraints, índices e o relacionamento FK.

## Passos de implementação
- Verificar que o projeto `FastFood.KitchenFlow.Infra.Persistence` está configurado corretamente
- Verificar que o projeto `FastFood.KitchenFlow.Api` referencia `Infra.Persistence` (necessário para design-time)
- Executar comando para criar migration:
  ```bash
  dotnet ef migrations add InitialCreate --project src/Infra/FastFood.KitchenFlow.Infra.Persistence --startup-project src/InterfacesExternas/FastFood.KitchenFlow.Api
  ```
- Verificar que a migration foi criada em `Migrations/`:
  - Arquivo `{Timestamp}_InitialCreate.cs`
  - Arquivo `{Timestamp}_InitialCreate.Designer.cs`
  - Arquivo `KitchenFlowDbContextModelSnapshot.cs` (ou atualizado)
- Revisar a migration gerada:
  - Tabela `Preparations` criada com:
    - `Id` (uuid, PK)
    - `OrderId` (uuid)
    - `Status` (integer)
    - `CreatedAt` (timestamp with time zone)
    - `OrderSnapshot` (jsonb)
    - Índice em `OrderId`
  - Tabela `Deliveries` criada com:
    - `Id` (uuid, PK)
    - `PreparationId` (uuid, FK)
    - `OrderId` (uuid, nullable)
    - `Status` (integer)
    - `CreatedAt` (timestamp with time zone)
    - `FinalizedAt` (timestamp with time zone, nullable)
    - Índices em `PreparationId` e `OrderId`
  - FK `Delivery.PreparationId` → `Preparation.Id` criada
- Se necessário, ajustar a migration manualmente (ex: nome de índices, comportamento de delete)
- Verificar que o método `Down()` reverte a migration corretamente

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\Core\FastFood.Auth.Infra.Persistence\Migrations\20251214222347_InitialCreate.cs`
- Seguir padrão de migrations do EF Core

## Observações importantes
- **Tipo jsonb**: Verificar que `OrderSnapshot` está sendo criado como `jsonb` no PostgreSQL
- **FK obrigatória**: Verificar que `PreparationId` em `Deliveries` é obrigatória (NOT NULL)
- **Índices**: Verificar que índices foram criados corretamente
- **Comportamento de delete**: Verificar comportamento da FK (RESTRICT ou CASCADE)

## Como testar
- Executar `dotnet build` para verificar que migration compila
- (Opcional) Aplicar migration manualmente para testar:
  ```bash
  dotnet ef database update --project src/Infra/FastFood.KitchenFlow.Infra.Persistence --startup-project src/InterfacesExternas/FastFood.KitchenFlow.Api
  ```
- Verificar que tabelas foram criadas no banco (se aplicado)
- Verificar que método `Down()` reverte corretamente

## Critérios de aceite
- Migration `InitialCreate` criada em `Migrations/`
- Tabela `Preparations` criada com todas as colunas:
  - `Id` (uuid, PK)
  - `OrderId` (uuid)
  - `Status` (integer)
  - `CreatedAt` (timestamp with time zone)
  - `OrderSnapshot` (jsonb)
  - Índice em `OrderId`
- Tabela `Deliveries` criada com todas as colunas:
  - `Id` (uuid, PK)
  - `PreparationId` (uuid, FK, NOT NULL)
  - `OrderId` (uuid, nullable)
  - `Status` (integer)
  - `CreatedAt` (timestamp with time zone)
  - `FinalizedAt` (timestamp with time zone, nullable)
  - Índices em `PreparationId` e `OrderId`
- FK `Delivery.PreparationId` → `Preparation.Id` criada
- Método `Down()` reverte a migration corretamente
- Migration compila sem erros
- Estrutura segue padrão do Auth (migrations organizadas)
