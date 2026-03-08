using System.Text.Json.Serialization;

namespace server.Dtos;

public record CustomerDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("postCode")] string? PostCode);