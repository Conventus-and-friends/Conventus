using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;
using static Conventus.Server.Models.Entities.Category;

namespace Conventus.Server.Models.DTO;

public sealed class CategoryDto : IModelValidating
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [Required]
    [MaxLength(NAME_MAX_LENGTH)]
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [MaxLength(DESCRIPTION_MAX_LENGTH)]
    [JsonPropertyName("description")] public string? Description { get; set; }

    [Pure]
    public bool IsValid()
    {
        return Name.Length <= NAME_MAX_LENGTH && (Description?.Length ?? 0) <= DESCRIPTION_MAX_LENGTH;
    }
}
