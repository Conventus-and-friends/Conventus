using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.Entities;
using MassTransit;

namespace Conventus.Server.Consumers;

public class CreateCategoryConsumer(ApplicationDbContext dbContext, ILogger<CreateCategoryConsumer> logger)
    : IConsumer<CreateCategory>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<CreateCategoryConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CreateCategory> context)
    {
        var message = context.Message;

        var post = new Category
        {
            Name = message.Name,
            Description = message.Description,
        };

        var result = await _dbContext.Categories.AddAsync(post);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await context.RespondAsync<CategoryCreationFailed>(new(ex));
            return;
        }

        await context.RespondAsync<CategoryCreated>(new(
            result.Entity.Id
        ));

        _logger.LogInformation("New category created with name: {Name}", result.Entity.Name);
    }
}
