using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models.Entities;

public sealed class Post
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
    public Category Category { get; set; } = null!;
}