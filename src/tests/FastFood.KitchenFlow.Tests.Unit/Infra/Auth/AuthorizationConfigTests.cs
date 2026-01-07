using FastFood.KitchenFlow.Infra.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
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

    [Fact]
    public async Task CustomerWithScopePolicy_WhenUserHasRoleButNotScope_ShouldFail()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication();
        services.AddAuthorizationPolicies();

        var serviceProvider = services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var claims = new List<Claim>
        {
            new Claim("role", "customer")
            // Missing scope claim
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(
            new[] { new CustomerWithScopePolicyRequirement() },
            principal,
            null!);

        // Act
        var result = await authorizationService.AuthorizeAsync(
            principal,
            null,
            AuthorizationConfig.CustomerWithScopePolicy);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task CustomerWithScopePolicy_WhenUserHasScopeButNotRole_ShouldFail()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication();
        services.AddAuthorizationPolicies();

        var serviceProvider = services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var claims = new List<Claim>
        {
            new Claim("scope", "customer")
            // Missing role claim
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(
            new[] { new CustomerWithScopePolicyRequirement() },
            principal,
            null!);

        // Act
        var result = await authorizationService.AuthorizeAsync(
            principal,
            null,
            AuthorizationConfig.CustomerWithScopePolicy);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task CustomerWithScopePolicy_WhenUserHasBothRoleAndScope_ShouldSucceed()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication();
        services.AddAuthorizationPolicies();

        var serviceProvider = services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var claims = new List<Claim>
        {
            new Claim("role", "customer"),
            new Claim("scope", "customer")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var result = await authorizationService.AuthorizeAsync(
            principal,
            null,
            AuthorizationConfig.CustomerWithScopePolicy);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task CustomerWithScopePolicy_WhenUserHasNeitherRoleNorScope_ShouldFail()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication();
        services.AddAuthorizationPolicies();

        var serviceProvider = services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var claims = new List<Claim>
        {
            new Claim("other", "value")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var result = await authorizationService.AuthorizeAsync(
            principal,
            null,
            AuthorizationConfig.CustomerWithScopePolicy);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    private class CustomerWithScopePolicyRequirement : IAuthorizationRequirement
    {
    }
}
