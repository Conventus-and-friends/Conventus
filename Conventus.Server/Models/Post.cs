using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Conventus.Server.Models;

public sealed class Post : IModelValidating
{
    private const int TITLE_MAX_LENGTH = 50;
    private const int CONTENT_MAX_LENGTH = int.MaxValue;

    public Guid Id { get; set; }

    [Required]
    [MaxLength(TITLE_MAX_LENGTH)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(CONTENT_MAX_LENGTH)]
    public string? Content { get; set; }

    [Required]
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Pure]
    public bool IsValid()
    {
        return Title.Length <= TITLE_MAX_LENGTH && Content?.Length <= CONTENT_MAX_LENGTH && CategoryId != 0;
    }
}
