# Subtask 04: Criar DbContext (KitchenFlowDbContext)

## Descrição
Criar o `DbContext` principal do KitchenFlow (`KitchenFlowDbContext`) que gerencia as entidades de persistência e aplica as configurações de mapeamento.

## Passos de implementação
- Criar classe `KitchenFlowDbContext` no projeto `FastFood.KitchenFlow.Infra.Persistence`:
  - Herdar de `DbContext`
  - Construtor recebendo `DbContextOptions<KitchenFlowDbContext>`
  - DbSet para `PreparationEntity`: `public DbSet<PreparationEntity> Preparations { get; set; }`
  - DbSet para `DeliveryEntity`: `public DbSet<DeliveryEntity> Deliveries { get; set; }`
  - Sobrescrever `OnModelCreating`:
    - Aplicar configurações usando `modelBuilder.ApplyConfiguration(new PreparationConfiguration())` e `modelBuilder.ApplyConfiguration(new DeliveryConfiguration())`
    - Ou usar `modelBuilder.ApplyConfigurationsFromAssembly(typeof(KitchenFlowDbContext).Assembly)` para aplicar todas as configurações automaticamente
- Adicionar documentação XML na classe
- Namespace: `FastFood.KitchenFlow.Infra.Persistence`

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\Core\FastFood.Auth.Infra.Persistence\AuthDbContext.cs`
- Seguir padrão simples e limpo do Auth

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que DbContext pode ser instanciado
- Validar que configurações são aplicadas corretamente

## Critérios de aceite
- Classe `KitchenFlowDbContext` criada:
  - Herda de `DbContext`
  - Construtor recebe `DbContextOptions<KitchenFlowDbContext>`
  - DbSet `Preparations` configurado
  - DbSet `Deliveries` configurado
  - `OnModelCreating` aplica configurações corretamente
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence`
- Estrutura segue padrão do Auth (DbContext simples e limpo)
