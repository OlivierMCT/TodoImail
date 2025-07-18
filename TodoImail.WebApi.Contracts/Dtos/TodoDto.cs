using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoImail.WebApi.Contracts.Dtos; 
public record TodoDto {
    public required int Id { get; init; }
    
    public required string Label { get; init; }

    [JsonPropertyName("done")]
    public required bool IsDone { get; init; }
    
    public required string Status { get; init; }
    
    [JsonPropertyName("category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual int? CategoryId { get; init; }
}

public record TodoDetailDto : TodoDto {
    [JsonPropertyName("deletable")] 
    public required bool IsDeletable { get; init; }

    [JsonPropertyName("due")]
    public required DateOnly DueDate { get; init; }

    [JsonPropertyName("lat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Latitude { get; init; }

    [JsonPropertyName("lng")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Longitude { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CategoryDto? Category { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public override int? CategoryId { get; init; }
}

public record TodoPostDto {
    [Required(AllowEmptyStrings = false)]
    public virtual string? Label { get; set; }

    [Required]
    [JsonPropertyName("due")]
    public virtual DateOnly? DueDate { get; set; }

    [Range(-90.0, 90.0)]
    [JsonPropertyName("lat")]
    public virtual double? Latitude { get; set; }

    [Range(-180.0, 180.0)]
    [JsonPropertyName("lng")]
    public virtual double? Longitude { get; set; }

    [JsonPropertyName("category")]
    public virtual int? CategoryId { get; set; }
}

public record TodoPutDto {
    [Required(AllowEmptyStrings = false)]
    public virtual string? Label { get; set; }

    [Required]
    [JsonPropertyName("due")]
    public virtual DateOnly? DueDate { get; set; }

    [Required]
    [JsonPropertyName("done")]
    public bool? IsDone { get; set; }

    [Range(-90.0, 90.0)]
    [JsonPropertyName("lat")]
    public virtual double? Latitude { get; set; }

    [Range(-180.0, 180.0)]
    [JsonPropertyName("lng")]
    public virtual double? Longitude { get; set; }

    [JsonPropertyName("category")]
    public virtual int? CategoryId { get; set; }
}

public record TodoPatchDto {
    [Required]
    [JsonPropertyName("done")]
    public bool? IsDone { get; set; }
}