namespace Conventus.Server.Models.Contracts;

public record CreateComment
{
    public required string Content { get; init; }
    public required Guid PostId { get; init; }
}

public record CommentCreated
(
    Guid Id,
    DateTime Created
);

public record CommentCreationFailed(Exception Exception);
