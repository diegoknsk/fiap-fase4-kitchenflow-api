using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FastFood.KitchenFlow.Infra.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.KitchenFlow.Infra.Persistence.Repositories;

/// <summary>
/// Implementação concreta do IPreparationRepository que usa KitchenFlowDbContext
/// para persistir dados de Preparation.
/// </summary>
public class PreparationRepository : IPreparationRepository
{
    private readonly KitchenFlowDbContext _context;

    /// <summary>
    /// Construtor que recebe o DbContext via Dependency Injection.
    /// </summary>
    /// <param name="context">DbContext do KitchenFlow.</param>
    public PreparationRepository(KitchenFlowDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Preparation preparation)
    {
        var entity = ToEntity(preparation);
        _context.Preparations.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task<Preparation?> GetByOrderIdAsync(Guid orderId)
    {
        var entity = await _context.Preparations
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (entity == null)
        {
            return null;
        }

        return ToDomain(entity);
    }

    /// <inheritdoc />
    public async Task<Preparation?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Preparations
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            return null;
        }

        return ToDomain(entity);
    }

    /// <summary>
    /// Mapeia entidade de domínio para entidade de persistência.
    /// </summary>
    /// <param name="domain">Entidade de domínio Preparation.</param>
    /// <returns>Entidade de persistência PreparationEntity.</returns>
    private static PreparationEntity ToEntity(Preparation domain)
    {
        return new PreparationEntity
        {
            Id = domain.Id,
            OrderId = domain.OrderId,
            Status = (int)domain.Status,
            CreatedAt = domain.CreatedAt,
            OrderSnapshot = domain.OrderSnapshot
        };
    }

    /// <summary>
    /// Mapeia entidade de persistência para entidade de domínio.
    /// </summary>
    /// <param name="entity">Entidade de persistência PreparationEntity.</param>
    /// <returns>Entidade de domínio Preparation.</returns>
    private static Preparation ToDomain(PreparationEntity entity)
    {
        // Como a entidade de domínio tem construtor privado e setters privados,
        // usamos reflection para criar a instância e definir as propriedades.
        var preparation = (Preparation)Activator.CreateInstance(typeof(Preparation), nonPublic: true)!;
        
        // Definir propriedades privadas via reflection
        typeof(Preparation).GetProperty(nameof(Preparation.Id))!
            .SetValue(preparation, entity.Id);
        typeof(Preparation).GetProperty(nameof(Preparation.OrderId))!
            .SetValue(preparation, entity.OrderId);
        typeof(Preparation).GetProperty(nameof(Preparation.Status))!
            .SetValue(preparation, (EnumPreparationStatus)entity.Status);
        typeof(Preparation).GetProperty(nameof(Preparation.CreatedAt))!
            .SetValue(preparation, entity.CreatedAt);
        typeof(Preparation).GetProperty(nameof(Preparation.OrderSnapshot))!
            .SetValue(preparation, entity.OrderSnapshot);

        return preparation;
    }
}
