using System.Text.Json.Serialization;

namespace TodoImail.BlazorApp.Client.Models {
    public record Category {
        public required int Id { get; init; }
        public required string Label { get; init; }
        public required string ShortLabel { get; init; }
        public required string Color { get; init; }
        public int? TodosCount { get; init; }
    }
}
