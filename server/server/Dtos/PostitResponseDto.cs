using System.Text.Json.Serialization;
using server.Dtos;

namespace server.Dtos;

public record PostitResponseDto(
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("data")] List<PostitResponseDto.ResposneData> Data)
{
    public record ResposneData(
        [property: JsonPropertyName("post_code")] string PostCode);

}