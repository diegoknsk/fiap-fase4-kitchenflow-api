using FastFood.KitchenFlow.Infra.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.KitchenFlow.Infra.Persistence.Configurations;

/// <summary>
/// Configuração de mapeamento para a entidade DeliveryEntity.
/// </summary>
public class DeliveryConfiguration : IEntityTypeConfiguration<DeliveryEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryEntity> builder)
    {
        // Nome da tabela
        builder.ToTable("Deliveries");

        // Chave primária
        builder.HasKey(d => d.Id);

        // Propriedades
        builder.Property(d => d.Id)
            .IsRequired();

        builder.Property(d => d.PreparationId)
            .IsRequired();

        builder.Property(d => d.OrderId)
            .IsRequired(false);

        builder.Property(d => d.Status)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(d => d.FinalizedAt)
            .IsRequired(false)
            .HasColumnType("timestamp with time zone");

        // Relacionamento FK: Delivery.PreparationId -> Preparation.Id
        builder.HasOne<PreparationEntity>()
            .WithMany()
            .HasForeignKey(d => d.PreparationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(d => d.PreparationId)
            .HasDatabaseName("IX_Deliveries_PreparationId");

        builder.HasIndex(d => d.OrderId)
            .HasDatabaseName("IX_Deliveries_OrderId");
    }
}
