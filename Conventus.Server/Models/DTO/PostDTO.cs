using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using static Conventus.Server.Models.Entities.Post;

namespace Conventus.Server.Models.DTO;

public sealed class PostDto : IModelValidating
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(TITLE_MAX_LENGTH)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(CONTENT_MAX_LENGTH)]
    public string? Content { get; set; }

    [Required]
    public long CategoryId { get; set; }

    [Pure]
    public bool IsValid()
    {
        return Title.Length <= TITLE_MAX_LENGTH && Content?.Length <= CONTENT_MAX_LENGTH && CategoryId != 0;
    }
}
