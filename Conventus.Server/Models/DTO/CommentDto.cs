using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Conventus.Server.Models.Entities.Comment;

namespace Conventus.Server.Models.DTO;

public class CommentDto : IModelValidating
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [Required]
    [MaxLength(COMMENT_MAX_LENGTH)]
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("post")] public Guid PostId { get; set; }

    [JsonPropertyName("created")] public DateTime Created { get; set; }

    public bool IsValid()
    {
        return Content.Length <= COMMENT_MAX_LENGTH;
    }
}
