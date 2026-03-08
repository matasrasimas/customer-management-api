using System.Text.Json;
using server.Dtos;

namespace server.Infrastructure;

public class PostitService(HttpClient httpClient, IConfiguration configuration) : IPostitService
{
    private readonly string apiKey = configuration["PostitApiKey"]!;
    private readonly string baseUrl = configuration["PostitApi:BaseUrl"]!;
    
    public async Task<string?> GetPostCodeAsync(string address)
    {
        var url = $"{baseUrl}?address={Uri.EscapeDataString(address)}&key={apiKey}";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;
        
        PostitResponseDto? deserializedContent = await DeserializeResponseAsync(response);

        if (deserializedContent is null || !deserializedContent.Success || deserializedContent.Total == 0)
            return null;

        return $"LT-{deserializedContent.Data[0].PostCode}";
    }

    private async Task<PostitResponseDto?> DeserializeResponseAsync(HttpResponseMessage response)
    {
        var serializedContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PostitResponseDto>(serializedContent);
    }
}