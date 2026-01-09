using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FastFood.KitchenFlow.Infra.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.KitchenFlow.Infra.Persistence.Repositories;

/// <summary>
/// Implementação concreta do IDeliveryRepository que usa KitchenFlowDbContext
/// para persistir dados de Delivery.
/// </summary>
public class DeliveryRepository : IDeliveryRepository
{
    private readonly KitchenFlowDbContext _context;

    /// <summary>
    /// Construtor que recebe o DbContext via Dependency Injection.
    /// </summary>
    /// <param name="context">DbContext do KitchenFlow.</param>
    public DeliveryRepository(KitchenFlowDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Delivery delivery)
    {
        var entity = ToEntity(delivery);
        _context.Deliveries.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task<Delivery?> GetByPreparationIdAsync(Guid preparationId)
    {
        var entity = await _context.Deliveries
            .FirstOrDefaultAsync(d => d.PreparationId == preparationId);

        if (entity == null)
        {
            return null;
        }

        return ToDomain(entity);
    }

    /// <inheritdoc />
    public async Task<(List<Delivery> deliveries, int totalCount)> GetReadyDeliveriesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Deliveries
            .Where(d => d.Status == (int)EnumDeliveryStatus.ReadyForPickup)
            .OrderBy(d => d.CreatedAt);

        var totalCount = await query.CountAsync();

        var entities = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var deliveries = entities.Select(ToDomain).ToList();

        return (deliveries, totalCount);
    }

    /// <inheritdoc />
    public async Task<Delivery?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Deliveries
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null)
        {
            return null;
        }

        return ToDomain(entity);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Delivery delivery)
    {
        // Verificar se a entidade já está sendo rastreada pelo contexto
        var trackedEntity = await _context.Deliveries.FindAsync(delivery.Id);
        
        if (trackedEntity != null)
        {
            // Se já está rastreada, atualizar as propriedades diretamente
            trackedEntity.PreparationId = delivery.PreparationId;
            trackedEntity.OrderId = delivery.OrderId;
            trackedEntity.Status = (int)delivery.Status;
            trackedEntity.CreatedAt = delivery.CreatedAt;
            trackedEntity.FinalizedAt = delivery.FinalizedAt;
        }
        else
        {
            // Se não está rastreada, criar nova entidade e usar Update
            var entity = ToEntity(delivery);
            _context.Deliveries.Update(entity);
        }
        
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Mapeia entidade de domínio para entidade de persistência.
    /// </summary>
    /// <param name="domain">Entidade de domínio Delivery.</param>
    /// <returns>Entidade de persistência DeliveryEntity.</returns>
    private static DeliveryEntity ToEntity(Delivery domain)
    {
        return new DeliveryEntity
        {
            Id = domain.Id,
            PreparationId = domain.PreparationId,
            OrderId = domain.OrderId,
            Status = (int)domain.Status,
            CreatedAt = domain.CreatedAt,
            FinalizedAt = domain.FinalizedAt
        };
    }

    /// <summary>
    /// Mapeia entidade de persistência para entidade de domínio.
    /// </summary>
    /// <param name="entity">Entidade de persistência DeliveryEntity.</param>
    /// <returns>Entidade de domínio Delivery.</returns>
    private static Delivery ToDomain(DeliveryEntity entity)
    {
        // Como a entidade de domínio tem construtor privado e setters privados,
        // usamos reflection para criar a instância e definir as propriedades.
        var delivery = (Delivery)Activator.CreateInstance(typeof(Delivery), nonPublic: true)!;
        
        // Definir propriedades privadas via reflection
        typeof(Delivery).GetProperty(nameof(Delivery.Id))!
            .SetValue(delivery, entity.Id);
        typeof(Delivery).GetProperty(nameof(Delivery.PreparationId))!
            .SetValue(delivery, entity.PreparationId);
        typeof(Delivery).GetProperty(nameof(Delivery.OrderId))!
            .SetValue(delivery, entity.OrderId);
        typeof(Delivery).GetProperty(nameof(Delivery.Status))!
            .SetValue(delivery, (EnumDeliveryStatus)entity.Status);
        typeof(Delivery).GetProperty(nameof(Delivery.CreatedAt))!
            .SetValue(delivery, entity.CreatedAt);
        typeof(Delivery).GetProperty(nameof(Delivery.FinalizedAt))!
            .SetValue(delivery, entity.FinalizedAt);

        return delivery;
    }
}
