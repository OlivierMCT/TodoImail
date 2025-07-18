using Imail.Core.Persistance;

namespace TodoImail.Services.Entities; 
public record TodoEntity : BaseEntity {
    public required string Label { get; set; }
    public required bool IsDone { get; set; }
    public required DateOnly DueDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public int? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
}
