using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using server.Dtos;
using server.Infrastructure;

namespace server.Tests.Infrastructure;

public class PostitServiceTests
{
    private readonly MockHttpMessageHandler mockHttp = new();
    private readonly IConfiguration configuration;
    
    private readonly PostitService service;
    
    public PostitServiceTests()
    {
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "PostitApiKey", "test-api-key" },
                { "PostitApi:BaseUrl", "https://test-url.com/" }
            })
            .Build();

        var httpClient = mockHttp.ToHttpClient();
        service = new PostitService(httpClient, configuration);
    }
    
    [Fact]
    public async Task GetPostCodeAsync_ValidAddress_ReturnsPostCodeWithLtPrefix()
    {
        var response = JsonSerializer.Serialize(new PostitResponseDto(
            Success: true,
            Total: 1,
            Data: [new PostitResponseDto.ResposneData(PostCode: "12345")]
        ));

        mockHttp
            .When("https://test-url.com/*")
            .Respond("application/json", response);

        var result = await service.GetPostCodeAsync("test-address");

        result.Should().Be("LT-12345");
    }
    
    [Fact]
    public async Task GetPostCodeAsync_ApiReturnsNoResults_ReturnsNull()
    {
        var responseJson = JsonSerializer.Serialize(new PostitResponseDto(
            Success: true,
            Total: 0,
            Data: []
        ));

        mockHttp
            .When("https://test-url.com/*")
            .Respond("application/json", responseJson);

        var result = await service.GetPostCodeAsync("test-address");

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetPostCodeAsync_ApiReturnsUnsuccessfulStatus_ReturnsNull()
    {
        var responseJson = JsonSerializer.Serialize(new PostitResponseDto(
            Success: false,
            Total: 0,
            Data: []
        ));

        mockHttp
            .When("https://test-url.com/*")
            .Respond("application/json", responseJson);

        var result = await service.GetPostCodeAsync("test-address");

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetPostCodeAsync_HttpRequestFails_ReturnsNull()
    {
        mockHttp
            .When("https://test-url.com/*")
            .Respond(HttpStatusCode.InternalServerError);

        var result = await service.GetPostCodeAsync("test-address");

        result.Should().BeNull();
    }
}