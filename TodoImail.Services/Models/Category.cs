using System.Drawing;

namespace TodoImail.Services.Models; 
public record Category {
    public required int Id { get; init; }
    public required string Label { get; init; }
    public required Color Color { get; init; }
    public int? TodosCount { get; init; }
}
