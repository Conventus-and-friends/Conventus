using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using static Conventus.Server.Models.Entities.Category;


namespace Conventus.Server.Models.DTO;

public sealed class CategoryDTO : IModelValidating
{
    public long Id { get; set; }

    [Required]
    [MaxLength(NAME_MAX_LENGTH)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(DESCRIPTION_MAX_LENGTH)]
    public string? Description { get; set; }

    [Pure]
    public bool IsValid()
    {
        return Name.Length <= NAME_MAX_LENGTH && Description?.Length <= DESCRIPTION_MAX_LENGTH;
    }
}
