using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Conventus.Server.Models;

public sealed class Category : IModelValidating
{
    private const int NAME_MAX_LENGTH = 30;
    private const int DESCRIPTION_MAX_LENGTH = 200;

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
