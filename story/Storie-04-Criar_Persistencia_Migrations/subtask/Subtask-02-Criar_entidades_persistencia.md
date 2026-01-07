# Subtask 02: Criar entidades de persistência (PreparationEntity, DeliveryEntity)

## Descrição
Criar as entidades de persistência `PreparationEntity` e `DeliveryEntity` no projeto `FastFood.KitchenFlow.Infra.Persistence`. Estas entidades representam a estrutura das tabelas no banco de dados e são **separadas** das entidades de domínio.

## Passos de implementação
- Criar pasta `Entities/` no projeto `FastFood.KitchenFlow.Infra.Persistence`
- Criar classe `PreparationEntity` em `Entities/PreparationEntity.cs`:
  - `Id` (Guid) - chave primária
  - `OrderId` (Guid) - ID do pedido (ou string, conforme padrão do OrderHub)
  - `Status` (int) - status da preparação (mapeia para EnumPreparationStatus)
  - `CreatedAt` (DateTime) - data/hora de criação
  - `OrderSnapshot` (string) - snapshot JSON (será mapeado para jsonb no banco)
  - Propriedades públicas com getters e setters (EF Core requer)
- Criar classe `DeliveryEntity` em `Entities/DeliveryEntity.cs`:
  - `Id` (Guid) - chave primária
  - `PreparationId` (Guid) - FK obrigatória para Preparation.Id
  - `OrderId` (Guid?) - ID do pedido (opcional, nullable)
  - `Status` (int) - status da entrega (mapeia para EnumDeliveryStatus)
  - `CreatedAt` (DateTime) - data/hora de criação
  - `FinalizedAt` (DateTime?) - data/hora de finalização (nullable)
  - Propriedades públicas com getters e setters
- Adicionar documentação XML nas classes (opcional, mas recomendado)
- Namespace: `FastFood.KitchenFlow.Infra.Persistence.Entities`

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\Core\FastFood.Auth.Infra.Persistence\Entities\CustomerEntity.cs`
- **Monolito**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\01-InterfacesExternas\FastFood.Infra.Persistence\Entities\PreparationEntity.cs` e `DeliveryEntity.cs`
- Adaptar para incluir `OrderSnapshot` em PreparationEntity

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que as entidades podem ser instanciadas
- Validar que propriedades estão corretas

## Critérios de aceite
- Classe `PreparationEntity` criada em `Entities/PreparationEntity.cs`:
  - Propriedades: `Id` (Guid), `OrderId` (Guid), `Status` (int), `CreatedAt` (DateTime), `OrderSnapshot` (string)
  - Propriedades públicas com getters e setters
- Classe `DeliveryEntity` criada em `Entities/DeliveryEntity.cs`:
  - Propriedades: `Id` (Guid), `PreparationId` (Guid), `OrderId` (Guid?), `Status` (int), `CreatedAt` (DateTime), `FinalizedAt` (DateTime?)
  - Propriedades públicas com getters e setters
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence.Entities`
- Estrutura segue padrão do Auth (entidades simples, sem lógica de negócio)
