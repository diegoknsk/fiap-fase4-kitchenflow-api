using FastFood.KitchenFlow.Infra.Persistence.Configurations;
using FastFood.KitchenFlow.Infra.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.KitchenFlow.Infra.Persistence;

/// <summary>
/// DbContext principal do KitchenFlow que gerencia as entidades de persistência.
/// </summary>
public class KitchenFlowDbContext : DbContext
{
    /// <summary>
    /// Conjunto de entidades Preparations.
    /// </summary>
    public DbSet<PreparationEntity> Preparations { get; set; }

    /// <summary>
    /// Conjunto de entidades Deliveries.
    /// </summary>
    public DbSet<DeliveryEntity> Deliveries { get; set; }

    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public KitchenFlowDbContext(DbContextOptions<KitchenFlowDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configura o modelo de dados aplicando as configurações de mapeamento.
    /// </summary>
    /// <param name="modelBuilder">Builder usado para configurar o modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly automaticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KitchenFlowDbContext).Assembly);
    }
}
