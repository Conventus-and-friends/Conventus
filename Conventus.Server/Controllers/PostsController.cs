using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController(ApplicationDbContext context)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;

    [HttpGet]
    public IEnumerable<PostDto> Get()
    {
        foreach (var post in _dbContext.Posts)
        {
            yield return post.ToDto();
        }
    }

    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<PostDto>> Get(Guid id)
    {
        var post = await _dbContext.Posts.FindAsync(id);
        if (post is null)
        {
            return NotFound();
        }
        return Ok(post.ToDto());
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetByCategory(long categoryId)
    {
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category is null)
        {
            return NotFound();
        }
        return Ok(category.Posts.Select(x => x.ToDto()));
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Post([FromBody] PostDto post)
    {
        if (!post.IsValid())
        {
            return BadRequest();
        }

        var result = await _dbContext.Posts.AddAsync(post.ToEntity());
        await _dbContext.SaveChangesAsync();

        post.Id = result.Entity.Id;

        return Ok(post);
    }
}
