using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.Entities;
using Ganss.Xss;
using MassTransit;

namespace Conventus.Server.Consumers;

public class CreatePostConsumer(ApplicationDbContext dbContext, HtmlSanitizer sanitizer, ILogger<CreatePostConsumer> logger)
    : IConsumer<CreatePost>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly HtmlSanitizer _sanitizer = sanitizer;
    private readonly ILogger<CreatePostConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CreatePost> context)
    {
        var message = context.Message;

        var now = DateTime.UtcNow;

        var post = new Post
        {
            Content = message.Content is not null ? _sanitizer.Sanitize(message.Content) : message.Content,
            Title = _sanitizer.Sanitize(message.Title),
            CategoryId = message.CategoryId,
            TimeCreated = TimeOnly.FromDateTime(now),
            DateCreated = DateOnly.FromDateTime(now),
        };

        var result = await _dbContext.Posts.AddAsync(post);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await context.RespondAsync<PostCreationFailed>(new(ex));
            return;
        }

        await context.RespondAsync<PostCreated>(new(
            result.Entity.Id,
            now
        ));

        _logger.LogDebug("New post created with id: {Id}", result.Entity.Id);
    }
}
