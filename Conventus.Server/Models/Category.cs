using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models;

public sealed class Category : IModelValidating
{
    public long Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? Description { get; set; }

    public bool IsValid()
    {
        return Name.Length <= 30 && Description?.Length <= 200;
    }
}
