using Conventus.Server.Extensions;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController(
    ApplicationDbContext context,
    IDistributedCache cache,
    ILogger<CategoriesController> logger)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<CategoriesController> _logger = logger;

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

        var result = await _dbContext.Categories.AddAsync(category.ToEntity());
        await _dbContext.SaveChangesAsync();

        category.Id = result.Entity.Id;

        _logger.LogInformation("New category created with name: {Name}", category.Name);

        return Ok(category);
    }
}
