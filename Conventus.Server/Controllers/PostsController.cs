using Conventus.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController(ApplicationDbContext context)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;

    [HttpGet]
    public IEnumerable<Post> Get()
    {
        return _dbContext.Posts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> Get(Guid id)
    {
        var post = await _dbContext.Posts.FindAsync(id);
        if (post is null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetByCategory(long categoryId)
    {
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category is null)
        {
            return NotFound();
        }
        return Ok(category.Posts);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> Post([FromBody] Post post)
    {
        if (!post.IsValid())
        {
            return BadRequest();
        }

        var result = await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        return Ok(result.Entity);
    }
}
