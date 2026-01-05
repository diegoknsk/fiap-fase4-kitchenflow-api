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

    /// <inheritdoc />
    public async Task<(IEnumerable<Preparation> preparations, int totalCount)> GetPagedAsync(int pageNumber, int pageSize, int? status)
    {
        var query = _context.Preparations.AsQueryable();

        // Aplicar filtro por status se fornecido
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        // Contar total de registros (com filtro aplicado)
        var totalCount = await query.CountAsync();

        // Aplicar paginação
        var entities = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Mapear para entidades de domínio
        var preparations = entities.Select(ToDomain).ToList();

        return (preparations, totalCount);
    }

    /// <inheritdoc />
    public async Task<Preparation?> GetOldestReceivedAsync()
    {
        var entity = await _context.Preparations
            .Where(p => p.Status == (int)EnumPreparationStatus.Received)
            .OrderBy(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        return ToDomain(entity);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Preparation preparation)
    {
        var entity = await _context.Preparations
            .FirstOrDefaultAsync(p => p.Id == preparation.Id);

        if (entity == null)
        {
            throw new InvalidOperationException($"Preparação {preparation.Id} não encontrada para atualização.");
        }

        // Atualizar propriedades
        entity.Status = (int)preparation.Status;
        entity.OrderId = preparation.OrderId;
        entity.CreatedAt = preparation.CreatedAt;
        entity.OrderSnapshot = preparation.OrderSnapshot;

        await _context.SaveChangesAsync();
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
