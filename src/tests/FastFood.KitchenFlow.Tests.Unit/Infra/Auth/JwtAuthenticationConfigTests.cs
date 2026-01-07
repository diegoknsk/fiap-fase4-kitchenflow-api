using FastFood.KitchenFlow.Infra.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Auth;

public class JwtAuthenticationConfigTests
{
    [Fact]
    public void AddCustomerJwtBearer_ShouldConfigureJwtBearerScheme()
    {
        // Arrange
        var services = new ServiceCollection();
        var authBuilder = services.AddAuthentication();
        var configuration = CreateValidConfiguration();

        // Act
        authBuilder.AddCustomerJwtBearer(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var authOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>();
        authOptions.Value.Schemes.Should().Contain(s => s.Name == "CustomerBearer");
    }

    [Fact]
    public void AddCustomerJwtBearer_ShouldConfigureTokenValidationParameters()
    {
        // Arrange
        var services = new ServiceCollection();
        var authBuilder = services.AddAuthentication();
        var configuration = CreateValidConfiguration();

        // Act
        authBuilder.AddCustomerJwtBearer(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var options = jwtBearerOptions.Get("CustomerBearer");
        
        options.Should().NotBeNull();
        options.TokenValidationParameters.Should().NotBeNull();
        options.TokenValidationParameters.ValidateIssuer.Should().BeTrue();
        options.TokenValidationParameters.ValidateAudience.Should().BeTrue();
        options.TokenValidationParameters.ValidateLifetime.Should().BeTrue();
        options.TokenValidationParameters.ValidateIssuerSigningKey.Should().BeTrue();
        options.TokenValidationParameters.ValidIssuer.Should().Be("https://test-issuer.com");
        options.TokenValidationParameters.ValidAudience.Should().Be("test-audience");
        options.TokenValidationParameters.ClockSkew.Should().Be(TimeSpan.FromSeconds(30));
        options.TokenValidationParameters.RoleClaimType.Should().Be("role");
        options.TokenValidationParameters.NameClaimType.Should().Be(JwtRegisteredClaimNames.Sub);
    }

    [Fact]
    public void AddCustomerJwtBearer_ShouldConfigureIssuerSigningKey()
    {
        // Arrange
        var services = new ServiceCollection();
        var authBuilder = services.AddAuthentication();
        var secretKey = "test-secret-key-that-is-at-least-32-characters-long";
        var configuration = CreateConfiguration(
            issuer: "https://test-issuer.com",
            audience: "test-audience",
            secretKey: secretKey);

        // Act
        authBuilder.AddCustomerJwtBearer(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var options = jwtBearerOptions.Get("CustomerBearer");
        
        options.TokenValidationParameters.IssuerSigningKey.Should().NotBeNull();
        var signingKey = options.TokenValidationParameters.IssuerSigningKey as SymmetricSecurityKey;
        signingKey.Should().NotBeNull();
    }

    [Fact]
    public void AddCustomerJwtBearer_WhenConfigurationIsInvalid_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();
        var authBuilder = services.AddAuthentication();
        var configuration = CreateConfiguration(
            issuer: null,
            audience: "test-audience",
            secretKey: "test-secret-key");

        // Act & Assert - A exceção é lançada quando o lambda é executado
        // O AddJwtBearer executa o lambda imediatamente, então a exceção deve ser lançada
        var exception = Assert.Throws<InvalidOperationException>(() => 
        {
            authBuilder.AddCustomerJwtBearer(configuration);
            // Força a execução do lambda acessando as opções
            var serviceProvider = services.BuildServiceProvider();
            var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
            _ = jwtBearerOptions.Get("CustomerBearer");
        });
        
        exception.Message.Should().Contain("JWT Issuer não configurado");
    }

    [Fact]
    public void ConfigureJwtSecurityTokenHandler_ShouldDisableMapInboundClaims()
    {
        // Arrange
        var originalValue = JwtSecurityTokenHandler.DefaultMapInboundClaims;

        try
        {
            // Act
            JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler();

            // Assert
            JwtSecurityTokenHandler.DefaultMapInboundClaims.Should().BeFalse();
        }
        finally
        {
            // Restore original value
            JwtSecurityTokenHandler.DefaultMapInboundClaims = originalValue;
        }
    }

    [Fact]
    public void AddCustomerJwtBearer_ShouldReturnAuthenticationBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        var authBuilder = services.AddAuthentication();
        var configuration = CreateValidConfiguration();

        // Act
        var result = authBuilder.AddCustomerJwtBearer(configuration);

        // Assert
        result.Should().BeSameAs(authBuilder);
    }

    private static IConfiguration CreateValidConfiguration()
    {
        return CreateConfiguration(
            issuer: "https://test-issuer.com",
            audience: "test-audience",
            secretKey: "test-secret-key-that-is-at-least-32-characters-long");
    }

    private static IConfiguration CreateConfiguration(string? issuer, string? audience, string? secretKey)
    {
        var configurationData = new Dictionary<string, string?>();

        if (issuer != null)
            configurationData["JwtCustomer:Issuer"] = issuer;

        if (audience != null)
            configurationData["JwtCustomer:Audience"] = audience;

        if (secretKey != null)
            configurationData["JwtCustomer:SecretKey"] = secretKey;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();
    }
}
