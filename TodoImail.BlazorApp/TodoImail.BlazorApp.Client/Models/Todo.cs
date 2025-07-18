using Imail.Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoImail.BlazorApp.Client.Models {
    public record Todo {
        public required int Id { get; init; }
        public required string Label { get; init; }
        public required bool IsDone { get; init; }
        public required string Status { get; init; }
        public Category? Category { get; init; }
    }

    public record TodoDetail : Todo {
        public required bool IsDeletable { get; init; }
        public required DateOnly DueDate { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public required bool WithCoords { get; init; }
    }

    public record TodoAdd {
        [Required(AllowEmptyStrings = false, ErrorMessage = "le libellé est obligatoire")]
        public string? Label { get; set; }

        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "la date d'échéance est obligatoire")]
        public DateOnly? DueDate { get; set; }

        [RequiredWith(nameof(Longitude), ErrorMessage = "la latitude est obligatoire avec la longitude")]
        [Range(-90, 90, ErrorMessage = "la latitude doit être comprise entre {1} et {2}")]
        public double? Latitude { get; set; }

        [RequiredWith(nameof(Latitude), ErrorMessage = "la longitude est obligatoire avec la longitude")]
        [Range(-180, 180, ErrorMessage = "la longitude doit être comprise entre {1} et {2}")]
        public double? Longitude { get; set; }
    }
}
