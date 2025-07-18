
using System.Text.Json.Serialization;

namespace TodoImail.WebApi.Contracts.Dtos; 
public record CategoryDto {
    public required int Id { get; init; }
    public required string Label { get; init; }
    public required string Color { get; init; }
    [JsonPropertyName("todos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TodosCount { get; init; }
}
