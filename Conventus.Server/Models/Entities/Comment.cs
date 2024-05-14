using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conventus.Server.Models.Entities;

public class Comment
{
    internal const int COMMENT_MAX_LENGTH = int.MaxValue;

    public Guid Id { get; set; }

    [Required]
    [MaxLength(COMMENT_MAX_LENGTH)]
    [Comment("The content of the comment")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [Comment("The id of the post the comment belongs to")]
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    [Required]
    [Comment("The day the comment was created on")]
    public DateOnly DateCreated { get; set; }
    [Required]
    [Comment("The time the comment was created on")]
    public TimeOnly TimeCreated { get; set; }

    [NotMapped]
    public DateTime Created => DateCreated.ToDateTime(TimeCreated);
}
