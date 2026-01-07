using FastFood.KitchenFlow.Application.Models.Common;
using FastFood.KitchenFlow.Application.OutputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.Presenters.PreparationManagement;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FluentAssertions;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Application.Presenters.PreparationManagement;

public class GetPreparationsPresenterTests
{
    [Fact]
    public void Present_WhenValidOutputModel_ShouldReturnApiResponseSuccess()
    {
        // Arrange
        var outputModel = new GetPreparationsOutputModel
        {
            Items = new List<PreparationItemOutputModel>
            {
                new PreparationItemOutputModel
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.NewGuid(),
                    Status = 0,
                    CreatedAt = DateTime.UtcNow,
                    OrderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}"""
                },
                new PreparationItemOutputModel
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.NewGuid(),
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    OrderSnapshot = """{"orderId":"test2","orderCode":"ORD-002"}"""
                }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10,
            TotalPages = 1
        };

        // Act
        var result = GetPreparationsPresenter.Present(outputModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApiResponse<GetPreparationsResponse>>();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Lista de preparações retornada com sucesso.");
        result.Content.Should().NotBeNull();
        
        var contentDict = result.Content as Dictionary<string, object>;
        contentDict.Should().NotBeNull();
        contentDict.Should().ContainKey("getPreparations");
        
        var response = contentDict!["getPreparations"] as GetPreparationsResponse;
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(2);
        response.TotalCount.Should().Be(2);
        response.PageNumber.Should().Be(1);
        response.PageSize.Should().Be(10);
        response.TotalPages.Should().Be(1);
        
        // Verificar itens
        var items = response.Items.ToList();
        items[0].Id.Should().Be(outputModel.Items.First().Id);
        items[1].Id.Should().Be(outputModel.Items.Last().Id);
    }

    [Fact]
    public void Present_WhenEmptyItems_ShouldReturnApiResponseWithEmptyList()
    {
        // Arrange
        var outputModel = new GetPreparationsOutputModel
        {
            Items = new List<PreparationItemOutputModel>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10,
            TotalPages = 0
        };

        // Act
        var result = GetPreparationsPresenter.Present(outputModel);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Lista de preparações retornada com sucesso.");
        
        var contentDict = result.Content as Dictionary<string, object>;
        contentDict.Should().NotBeNull();
        contentDict.Should().ContainKey("getPreparations");
        
        var response = contentDict!["getPreparations"] as GetPreparationsResponse;
        response.Should().NotBeNull();
        response!.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
    }

    [Fact]
    public void Present_WhenMultiplePages_ShouldReturnCorrectPagination()
    {
        // Arrange
        var outputModel = new GetPreparationsOutputModel
        {
            Items = new List<PreparationItemOutputModel>
            {
                new PreparationItemOutputModel
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.NewGuid(),
                    Status = 0,
                    CreatedAt = DateTime.UtcNow,
                    OrderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}"""
                }
            },
            TotalCount = 25,
            PageNumber = 2,
            PageSize = 10,
            TotalPages = 3
        };

        // Act
        var result = GetPreparationsPresenter.Present(outputModel);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        
        var contentDict = result.Content as Dictionary<string, object>;
        var response = contentDict!["getPreparations"] as GetPreparationsResponse;
        response.Should().NotBeNull();
        response!.TotalCount.Should().Be(25);
        response.PageNumber.Should().Be(2);
        response.PageSize.Should().Be(10);
        response.TotalPages.Should().Be(3);
    }

    [Fact]
    public void Present_ShouldMapAllItemPropertiesCorrectly()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var orderSnapshot = """{"orderId":"test","orderCode":"ORD-001"}""";
        
        var outputModel = new GetPreparationsOutputModel
        {
            Items = new List<PreparationItemOutputModel>
            {
                new PreparationItemOutputModel
                {
                    Id = itemId,
                    OrderId = orderId,
                    Status = 2,
                    CreatedAt = createdAt,
                    OrderSnapshot = orderSnapshot
                }
            },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10,
            TotalPages = 1
        };

        // Act
        var result = GetPreparationsPresenter.Present(outputModel);

        // Assert
        var contentDict = result.Content as Dictionary<string, object>;
        var response = contentDict!["getPreparations"] as GetPreparationsResponse;
        var item = response!.Items.First();
        
        item.Id.Should().Be(itemId);
        item.OrderId.Should().Be(orderId);
        item.Status.Should().Be(2);
        item.CreatedAt.Should().Be(createdAt);
        item.OrderSnapshot.Should().Be(orderSnapshot);
    }
}
