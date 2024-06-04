using Conventus.Server.Extensions;
using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController(
    ApplicationDbContext context,
    IRequestClient<CreateCategory> createCategoryClient,
    IDistributedCache cache)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly IRequestClient<CreateCategory> _createCategoryClient = createCategoryClient;
    private readonly IDistributedCache _cache = cache;

    private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

    [HttpGet]
    public async IAsyncEnumerable<CategoryDto> Get()
    {
        await foreach (var category in _dbContext.Categories.AsAsyncEnumerable())
        {
            yield return category.ToDto();
        }
    }

    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(long id)
    {
        var category = await _cache.GetOrCreateAsync($"categories.category_{id}", async () =>
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
            {
                return null;
            }
            return category.ToDto();
        }, _cacheOptions);

        if (category is null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Post([FromBody] CategoryDto category)
    {
        if (!category.IsValid())
        {
            return BadRequest();
        }

        var response = await _createCategoryClient.GetResponse<CategoryCreated, CategoryCreationFailed>(category.ToCreateContract());

        if (response.Is(out Response<CategoryCreationFailed>? failure))
        {
            if (failure.Message.Exception is DbUpdateException)
            {
                return BadRequest();
            }
            throw failure.Message.Exception;
        }

        category.Id = ((CategoryCreated)response.Message).Id;

        return Ok(category);
    }
}
