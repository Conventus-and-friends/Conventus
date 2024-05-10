using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models;

public sealed class Category
{
    public long Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? Description { get; set; }
}
