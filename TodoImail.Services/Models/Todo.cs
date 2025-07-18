using Imail.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace TodoImail.Services.Models; 

public enum TodoStatus { Archived, Closed, Pending, Urgent, Late }

public record Todo {
    public required int Id { get; init; }
    public required string Label { get; init; }
    public required bool IsDone { get; init; }
    public required bool IsDeletable { get; init; }
    public required DateOnly DueDate { get; init; }
    public required TodoStatus Status { get; init; }
    public TodoCoordinate? Coords { get; init; }
    public Category? Category { get; init; }
}

public record TodoCoordinate {
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}

public abstract record TodoUpdate {
    [Required(AllowEmptyStrings = false), MaxLength(180)]
    public virtual string? Label { get; set; }

    [Required]
    public virtual DateOnly? DueDate { get; set; }
    
    [RequiredWith(nameof(Longitude), ErrorMessage = "required with longitude"), Range(34.5, 71.0)]
    public virtual double? Latitude { get; set; }
    
    [RequiredWith(nameof(Latitude), ErrorMessage = "required with latitude"), Range(-25.0, 40.0)]
    public virtual double? Longitude { get; set; }
    
    [Range(1, int.MaxValue)]
    public virtual int? CategoryId { get; set; }
}

public record TodoCreate : TodoUpdate {
    [Required, MinDate(MinDateAttribute.Today, ErrorMessage = "must be in future")]
    public override DateOnly? DueDate { get; set; }    
}

public record TodoModify : TodoUpdate {
    [Required, Range(1, int.MaxValue)]
    public int? Id { get; init; }

    [Required]
    public virtual bool? IsDone { get; set; }
}
