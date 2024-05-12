using Conventus.Server.Models;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController(ApplicationDbContext context)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> Get([FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        return Ok(_dbContext.Posts.Select(x => x.ToDto()).Skip((pager.Page - 1) * pager.PageLength).Take(pager.PageLength));
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
    public async Task<ActionResult<IEnumerable<PostDto>>> GetByCategory(long categoryId, [FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category is null)
        {
            return NotFound();
        }
        return Ok(category.Posts.Select(x => x.ToDto()).Skip((pager.Page - 1) * pager.PageLength).Take(pager.PageLength));
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Post([FromBody] PostDto post)
    {
        if (!post.IsValid())
        {
            return BadRequest();
        }

        var result = await _dbContext.Posts.AddAsync(post.ToEntity());

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        post.Id = result.Entity.Id;

        return Ok(post);
    }
}
