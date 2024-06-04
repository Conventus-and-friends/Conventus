namespace Conventus.Server.Models.Contracts;

public record CreatePost
{
    public required string Title { get; init; }
    public required string? Content { get; init; }
    public required long CategoryId { get; init; }
}

public record PostCreated
(
    Guid Id,
    DateTime Created
);

public record PostCreationFailed(Exception Exception);
