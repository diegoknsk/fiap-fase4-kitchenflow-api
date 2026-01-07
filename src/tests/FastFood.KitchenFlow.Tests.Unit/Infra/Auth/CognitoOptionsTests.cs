using FastFood.KitchenFlow.Infra.Auth;
using FluentAssertions;
using Xunit;

namespace FastFood.KitchenFlow.Tests.Unit.Infra.Auth;

public class CognitoOptionsTests
{
    [Fact]
    public void SectionName_ShouldBeCorrect()
    {
        // Assert
        CognitoOptions.SectionName.Should().Be("Authentication:Cognito");
    }

    [Fact]
    public void Properties_ShouldHaveDefaultValues()
    {
        // Arrange
        var options = new CognitoOptions();

        // Assert
        options.UserPoolId.Should().BeEmpty();
        options.ClientId.Should().BeEmpty();
        options.Region.Should().Be("us-east-1");
        options.ClockSkewMinutes.Should().Be(5);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var options = new CognitoOptions
        {
            UserPoolId = "us-east-1_TestPool",
            ClientId = "test-client-id",
            Region = "us-west-2",
            ClockSkewMinutes = 10
        };

        // Assert
        options.UserPoolId.Should().Be("us-east-1_TestPool");
        options.ClientId.Should().Be("test-client-id");
        options.Region.Should().Be("us-west-2");
        options.ClockSkewMinutes.Should().Be(10);
    }

    [Fact]
    public void Authority_ShouldBeGeneratedFromRegionAndUserPoolId()
    {
        // Arrange
        var options = new CognitoOptions
        {
            UserPoolId = "us-east-1_TestPool",
            Region = "us-west-2"
        };

        // Assert
        options.Authority.Should().Be("https://cognito-idp.us-west-2.amazonaws.com/us-east-1_TestPool");
    }
}
