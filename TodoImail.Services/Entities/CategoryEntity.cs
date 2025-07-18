using Imail.Core.Persistance;

namespace TodoImail.Services.Entities; 
public record CategoryEntity : BaseEntity {
    public required string Label { get; set; }
    public required string Color { get; set; }

    public ICollection<TodoEntity> Todos { get; set; } = [];
}
