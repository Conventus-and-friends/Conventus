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

        return Ok(_dbContext.Posts
            .Skip((pager.Page - 1) * pager.PageLength).Take(pager.PageLength)
            .Select(x => x.ToDto()));
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
    public async Task<ActionResult<IEnumerable<PostDto>>> Get(long categoryId, [FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        var posts = _dbContext.Posts
            .Where(x => x.CategoryId == categoryId)
            .Skip((pager.Page - 1) * pager.PageLength).Take(pager.PageLength);

        return Ok(posts.Select(x => x.ToDto()));
    }

    [HttpGet("page-count")]
    public ActionResult<int> GetPageCount([FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }
        return Ok((int)Math.Ceiling(_dbContext.Posts.Count() / (double)pager.PageLength));
    }

    [HttpGet("by-category/{categoryId}/page-count")]
    public async Task<ActionResult<int>> GetPageCount(long categoryId, [FromQuery] Pager pager)
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
        return Ok((int)Math.Ceiling(category.Posts.Count / (double)pager.PageLength));
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
