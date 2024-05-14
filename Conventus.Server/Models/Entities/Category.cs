using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models.Entities;

public class Category
{
    internal const int NAME_MAX_LENGTH = 30;
    internal const int DESCRIPTION_MAX_LENGTH = 200;

    public long Id { get; set; }

    [Required]
    [MaxLength(NAME_MAX_LENGTH)]
    [Comment("The name of the category")]
    public string Name { get; set; } = string.Empty;
    [MaxLength(DESCRIPTION_MAX_LENGTH)]
    [Comment("The description of the category")]
    public string? Description { get; set; }

    public virtual ICollection<Post> Posts { get; } = [];
}
