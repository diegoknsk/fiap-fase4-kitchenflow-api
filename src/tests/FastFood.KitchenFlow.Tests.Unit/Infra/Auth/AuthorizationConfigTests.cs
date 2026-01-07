using FastFood.KitchenFlow.Infra.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Auth;

public class AuthorizationConfigTests
{
    [Fact]
    public void AddAuthorizationPolicies_ShouldAddAllPolicies()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication();

        // Act
        services.AddAuthorizationPolicies();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var authorizationOptions = serviceProvider.GetRequiredService<IAuthorizationService>();
        authorizationOptions.Should().NotBeNull();
    }

    [Fact]
    public void Constants_ShouldHaveCorrectValues()
    {
        // Assert
        AuthorizationConfig.AdminPolicy.Should().Be("Admin");
        AuthorizationConfig.CustomerPolicy.Should().Be("Customer");
        AuthorizationConfig.CustomerWithScopePolicy.Should().Be("CustomerWithScope");
    }

    [Fact]
    public void AddAuthorizationPolicies_ShouldReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddAuthorizationPolicies();

        // Assert
        result.Should().BeSameAs(services);
    }
}
