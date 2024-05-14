using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conventus.Server.Models.Entities;

public class Post
{
    internal const int TITLE_MAX_LENGTH = 50;
    internal const int CONTENT_MAX_LENGTH = int.MaxValue;

    public Guid Id { get; set; }

    [Required]
    [MaxLength(TITLE_MAX_LENGTH)]
    [Comment("The title of the post")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(CONTENT_MAX_LENGTH)]
    [Comment("The content of the post")]
    public string? Content { get; set; }

    [Required]
    [Comment("The id of the category the post belongs to")]
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; } = [];

    [Required]
    [Comment("The day the post was created on")]
    public DateOnly DateCreated { get; set; }
    [Required]
    [Comment("The time the post was created on")]
    public TimeOnly TimeCreated { get; set; }

    [NotMapped]
    public DateTime Created => DateCreated.ToDateTime(TimeCreated);
}
