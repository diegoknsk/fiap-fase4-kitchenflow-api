using FastFood.KitchenFlow.Infra.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.KitchenFlow.Infra.Persistence.Configurations;

/// <summary>
/// Configuração de mapeamento para a entidade PreparationEntity.
/// </summary>
public class PreparationConfiguration : IEntityTypeConfiguration<PreparationEntity>
{
    public void Configure(EntityTypeBuilder<PreparationEntity> builder)
    {
        // Nome da tabela
        builder.ToTable("Preparations");

        // Chave primária
        builder.HasKey(p => p.Id);

        // Propriedades
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.OrderSnapshot)
            .IsRequired()
            .HasColumnType("jsonb");

        // Índices
        builder.HasIndex(p => p.OrderId)
            .HasDatabaseName("IX_Preparations_OrderId");
    }
}
