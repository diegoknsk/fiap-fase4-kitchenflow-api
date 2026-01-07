using FastFood.KitchenFlow.Domain.Common.Enums;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FastFood.KitchenFlow.Infra.Persistence;
using FastFood.KitchenFlow.Infra.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Persistence.Repositories;

public class PreparationRepositoryTests : IDisposable
{
    private readonly KitchenFlowDbContext _dbContext;
    private readonly PreparationRepository _repository;

    public PreparationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<KitchenFlowDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new KitchenFlowDbContext(options);
        _repository = new PreparationRepository(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_WhenValidPreparation_ShouldPersistAndReturnId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);

        // Act
        var result = await _repository.CreateAsync(preparation);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(preparation.Id);
        
        var persisted = await _dbContext.Preparations
            .FirstOrDefaultAsync(p => p.Id == preparation.Id);
        persisted.Should().NotBeNull();
        persisted!.OrderId.Should().Be(orderId);
        persisted.Status.Should().Be((int)EnumPreparationStatus.Received);
        persisted.OrderSnapshot.Should().Be(orderSnapshot);
    }

    [Fact]
    public async Task GetByOrderIdAsync_WhenExists_ShouldReturnPreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        await _repository.CreateAsync(preparation);

        // Act
        var result = await _repository.GetByOrderIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result!.OrderId.Should().Be(orderId);
        result.Id.Should().Be(preparation.Id);
        result.Status.Should().Be(EnumPreparationStatus.Received);
    }

    [Fact]
    public async Task GetByOrderIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByOrderIdAsync(orderId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnPreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        var preparationId = await _repository.CreateAsync(preparation);

        // Act
        var result = await _repository.GetByIdAsync(preparationId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(preparationId);
        result.OrderId.Should().Be(orderId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPagedAsync_WhenNoFilter_ShouldReturnAllPreparations()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        
        var prep1 = Preparation.Create(orderId1, orderSnapshot);
        var prep2 = Preparation.Create(orderId2, orderSnapshot);
        
        await _repository.CreateAsync(prep1);
        await Task.Delay(10); // Garantir ordem diferente de criação
        await _repository.CreateAsync(prep2);

        // Act
        var (preparations, totalCount) = await _repository.GetPagedAsync(1, 10, null);

        // Assert
        totalCount.Should().Be(2);
        preparations.Should().HaveCount(2);
        preparations.Should().Contain(p => p.Id == prep1.Id);
        preparations.Should().Contain(p => p.Id == prep2.Id);
    }

    [Fact]
    public async Task GetPagedAsync_WhenFilteredByStatus_ShouldReturnFilteredPreparations()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        
        var prep1 = Preparation.Create(orderId1, orderSnapshot);
        prep1.StartPreparation();
        var prep2 = Preparation.Create(orderId2, orderSnapshot);
        
        await _repository.CreateAsync(prep1);
        await _repository.CreateAsync(prep2);
        await _repository.UpdateAsync(prep1);

        // Act
        var (preparations, totalCount) = await _repository.GetPagedAsync(1, 10, (int)EnumPreparationStatus.InProgress);

        // Assert
        totalCount.Should().Be(1);
        preparations.Should().HaveCount(1);
        preparations.First().Status.Should().Be(EnumPreparationStatus.InProgress);
    }

    [Fact]
    public async Task GetPagedAsync_WhenPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        
        for (int i = 0; i < 5; i++)
        {
            var prep = Preparation.Create(Guid.NewGuid(), orderSnapshot);
            await _repository.CreateAsync(prep);
        }

        // Act
        var (preparations, totalCount) = await _repository.GetPagedAsync(2, 2, null);

        // Assert
        totalCount.Should().Be(5);
        preparations.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetOldestReceivedAsync_WhenExists_ShouldReturnOldestPreparation()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        
        var prep1 = Preparation.Create(orderId1, orderSnapshot);
        await Task.Delay(10);
        var prep2 = Preparation.Create(orderId2, orderSnapshot);
        
        await _repository.CreateAsync(prep1);
        await _repository.CreateAsync(prep2);

        // Act
        var result = await _repository.GetOldestReceivedAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(prep1.Id);
        result.Status.Should().Be(EnumPreparationStatus.Received);
    }

    [Fact]
    public async Task GetOldestReceivedAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var prep = Preparation.Create(orderId, orderSnapshot);
        prep.StartPreparation();
        await _repository.CreateAsync(prep);
        await _repository.UpdateAsync(prep);

        // Act
        var result = await _repository.GetOldestReceivedAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdatePreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);
        await _repository.CreateAsync(preparation);
        
        preparation.StartPreparation();

        // Act
        await _repository.UpdateAsync(preparation);

        // Assert
        var updated = await _repository.GetByIdAsync(preparation.Id);
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(EnumPreparationStatus.InProgress);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        var preparation = Preparation.Create(orderId, orderSnapshot);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(preparation));
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
