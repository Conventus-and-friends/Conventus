using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.Entities;
using Ganss.Xss;
using MassTransit;

namespace Conventus.Server.Consumers;

public class CreateCommentConsumer(ApplicationDbContext dbContext, HtmlSanitizer sanitizer, ILogger<CreateCommentConsumer> logger)
    : IConsumer<CreateComment>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly HtmlSanitizer _sanitizer = sanitizer;
    private readonly ILogger<CreateCommentConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CreateComment> context)
    {
        var message = context.Message;

        var now = DateTime.UtcNow;

        var post = new Comment
        {
            Content = _sanitizer.Sanitize(message.Content),
            PostId = message.PostId,
            TimeCreated = TimeOnly.FromDateTime(now),
            DateCreated = DateOnly.FromDateTime(now),
        };

        var result = await _dbContext.Comments.AddAsync(post);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await context.RespondAsync<CommentCreationFailed>(new(ex));
            return;
        }

        await context.RespondAsync<CommentCreated>(new(
            result.Entity.Id,
            now
        ));

        _logger.LogDebug("Added comment {Id} to post with id: {PostId}", result.Entity.Id, result.Entity.PostId);
    }
}
