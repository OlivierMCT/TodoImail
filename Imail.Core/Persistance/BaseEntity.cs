namespace Imail.Core.Persistance; 
public abstract record BaseEntity {
    public int Id { get; set; }

    public Guid RowId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
