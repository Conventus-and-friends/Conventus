using Conventus.Server.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController(ApplicationDbContext context)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;

    [HttpGet]
    public IEnumerable<Category> Get()
    {
        return _dbContext.Categories;
    }

    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<Category>> Get(long id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post([FromBody] Category category)
    {
        if (!category.IsValid())
        {
            return BadRequest();
        }

        var result = await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return Ok(result.Entity);
    }
}
