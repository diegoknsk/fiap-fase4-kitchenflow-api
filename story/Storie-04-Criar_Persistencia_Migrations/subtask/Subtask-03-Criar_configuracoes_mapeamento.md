# Subtask 03: Criar configurações de mapeamento (EntityTypeConfiguration)

## Descrição
Criar as configurações de mapeamento usando `IEntityTypeConfiguration<T>` para mapear as entidades de persistência para as tabelas do PostgreSQL, incluindo tipos de dados, constraints, índices e relacionamentos.

## Passos de implementação
- Criar pasta `Configurations/` no projeto `FastFood.KitchenFlow.Infra.Persistence`
- Criar classe `PreparationConfiguration` em `Configurations/PreparationConfiguration.cs`:
  - Implementar `IEntityTypeConfiguration<PreparationEntity>`
  - Configurar nome da tabela: `Preparations`
  - Configurar chave primária: `Id`
  - Configurar propriedades:
    - `OrderId`: Guid, required
    - `Status`: int, required
    - `CreatedAt`: DateTime, required, tipo `timestamp with time zone`
    - `OrderSnapshot`: string, mapeado para `jsonb` no PostgreSQL
      - Usar `.HasColumnType("jsonb")` para configurar tipo jsonb
  - Criar índice em `OrderId` para otimizar consultas
- Criar classe `DeliveryConfiguration` em `Configurations/DeliveryConfiguration.cs`:
  - Implementar `IEntityTypeConfiguration<DeliveryEntity>`
  - Configurar nome da tabela: `Deliveries`
  - Configurar chave primária: `Id`
  - Configurar propriedades:
    - `PreparationId`: Guid, required
    - `OrderId`: Guid?, nullable (opcional)
    - `Status`: int, required
    - `CreatedAt`: DateTime, required, tipo `timestamp with time zone`
    - `FinalizedAt`: DateTime?, nullable, tipo `timestamp with time zone`
  - Configurar FK: `PreparationId` → `Preparations.Id`
    - Usar `.HasOne<>().WithMany()` ou `.HasForeignKey()`
    - Definir comportamento de delete (RESTRICT ou CASCADE conforme regra)
  - Criar índices:
    - Índice em `PreparationId` (FK)
    - Índice em `OrderId` (opcional, para consultas)
- Adicionar documentação XML nas classes
- Namespace: `FastFood.KitchenFlow.Infra.Persistence.Configurations`

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\Core\FastFood.Auth.Infra.Persistence\Configurations\CustomerConfiguration.cs`
- Usar Fluent API do EF Core para configurações

## Observações importantes
- **OrderSnapshot como jsonb**: Usar `.HasColumnType("jsonb")` para mapear string para tipo jsonb do PostgreSQL
- **FK obrigatória**: `Delivery.PreparationId` deve ser obrigatória e referenciar `Preparation.Id`
- **Comportamento de delete**: Decidir se deve ser RESTRICT (não permite deletar Preparation com Delivery) ou CASCADE (deleta Delivery quando Preparation é deletada)
- **Índices**: Criar índices em campos usados frequentemente em consultas (OrderId, PreparationId)

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que configurações estão corretas
- Validar sintaxe do Fluent API

## Critérios de aceite
- Classe `PreparationConfiguration` criada:
  - Implementa `IEntityTypeConfiguration<PreparationEntity>`
  - Tabela `Preparations` configurada
  - Chave primária `Id` configurada
  - Propriedades mapeadas corretamente
  - `OrderSnapshot` configurado como `jsonb`
  - Índice em `OrderId` criado
- Classe `DeliveryConfiguration` criada:
  - Implementa `IEntityTypeConfiguration<DeliveryEntity>`
  - Tabela `Deliveries` configurada
  - Chave primária `Id` configurada
  - Propriedades mapeadas corretamente
  - FK `PreparationId` → `Preparations.Id` configurada
  - Índices em `PreparationId` e `OrderId` criados
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence.Configurations`
- Configurações seguem padrão do Auth (Fluent API)
