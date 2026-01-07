using FastFood.KitchenFlow.Infra.Auth;
using FluentAssertions;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Auth;

public class JwtOptionsTests
{
    [Fact]
    public void Constructor_ShouldSetAllProperties()
    {
        // Arrange & Act
        var options = new JwtOptions(
            "https://test-issuer.com",
            "test-audience",
            "test-secret-key",
            60
        );

        // Assert
        options.Issuer.Should().Be("https://test-issuer.com");
        options.Audience.Should().Be("test-audience");
        options.SecretKey.Should().Be("test-secret-key");
        options.ExpiresInMinutes.Should().Be(60);
    }

    [Fact]
    public void RecordEquality_ShouldWork()
    {
        // Arrange
        var options1 = new JwtOptions("issuer", "audience", "secret", 60);
        var options2 = new JwtOptions("issuer", "audience", "secret", 60);
        var options3 = new JwtOptions("issuer2", "audience", "secret", 60);

        // Assert
        options1.Should().Be(options2);
        options1.Should().NotBe(options3);
    }
}
