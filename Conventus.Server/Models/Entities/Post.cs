using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models.Entities;

public class Post
{
    internal const int TITLE_MAX_LENGTH = 50;
    internal const int CONTENT_MAX_LENGTH = int.MaxValue;

    public Guid Id { get; set; }

    [Required]
    [MaxLength(TITLE_MAX_LENGTH)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(CONTENT_MAX_LENGTH)]
    public string? Content { get; set; }

    [Required]
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    [Required]
    public DateOnly DateCreated { get; set; }
    [Required]
    public TimeOnly TimeCreated { get; set; }

    public DateTime Created => DateCreated.ToDateTime(TimeCreated);
}
