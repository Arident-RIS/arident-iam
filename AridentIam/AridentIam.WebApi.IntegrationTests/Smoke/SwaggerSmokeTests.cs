using AridentIam.WebApi.IntegrationTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace AridentIam.WebApi.IntegrationTests.Smoke;

public sealed class SwaggerSmokeTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task Swagger_Json_Should_Be_Available()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");

        response.IsSuccessStatusCode.Should().BeTrue();
    }
}