using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController(
    ApplicationDbContext context,
    ILogger<CategoriesController> logger)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly ILogger<CategoriesController> _logger = logger;

    [HttpGet]
    public IEnumerable<CategoryDto> Get()
    {
        foreach (var category in _dbContext.Categories)
        {
            yield return category.ToDto();
        }
    }

    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(long id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound();
        }
        return Ok(category.ToDto());
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
