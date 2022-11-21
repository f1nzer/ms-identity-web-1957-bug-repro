using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;
using Xunit.Abstractions;

namespace IntegrationTests;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _outputHelper;

    public UnitTest1(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Get_EndpointsReturnUnauthorizedEveryTime()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var requestTasks = Enumerable.Range(1, 10).Select(_ => client.GetAsync("/"));
        var responses = await Task.WhenAll(requestTasks);

        // Assert
        Assert.All(responses, x =>
        {
            try
            {
                Assert.Equal(HttpStatusCode.Unauthorized, x.StatusCode);
            }
            catch (Exception)
            {
                var str = x.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                _outputHelper.WriteLine(str);
                throw;
            }
        });
    }
}