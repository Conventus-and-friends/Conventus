using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models.Entities;

public sealed class Category
{
    internal const int NAME_MAX_LENGTH = 30;
    internal const int DESCRIPTION_MAX_LENGTH = 200;

    public long Id { get; set; }

    [Required]
    [MaxLength(NAME_MAX_LENGTH)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(DESCRIPTION_MAX_LENGTH)]
    public string? Description { get; set; }

    public ICollection<Post> Posts { get; set; } = [];
}
