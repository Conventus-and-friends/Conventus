namespace Conventus.Server.Models.Contracts;

public record CreateCategory
{
    public required string Name { get; init; }
    public required string? Description { get; init; }
}

public record CategoryCreated(long Id);

public record CategoryCreationFailed(Exception Exception);
