using Conventus.Server.Models;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController(
    ApplicationDbContext context,
    HtmlSanitizer sanitizer,
    ILogger<PostsController> logger)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly HtmlSanitizer _htmlSanitizer = sanitizer;
    private readonly ILogger<PostsController> _logger = logger;

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> Get([FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        return Ok(_dbContext.Posts
            .Skip((pager.Page - 1) * pager.Length).Take(pager.Length)
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
    public ActionResult<IEnumerable<PostDto>> Get(long categoryId, [FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        var posts = _dbContext.Posts
            .Where(x => x.CategoryId == categoryId)
            .Skip((pager.Page - 1) * pager.Length).Take(pager.Length);

        return Ok(posts.Select(x => x.ToDto()));
    }

    [HttpGet("count")]
    public Task<int> GetCount()
    {
        return _dbContext.Posts.CountAsync();
    }

    [HttpGet("by-category/{categoryId}/count")]
    public Task<int> GetCount(long categoryId)
    {
        var posts = _dbContext.Posts
            .Where(x => x.CategoryId == categoryId);

        return posts.CountAsync();
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Post([FromBody] PostDto post)
    {
        if (!post.IsValid())
        {
            return BadRequest();
        }

        post.Title = _htmlSanitizer.Sanitize(post.Title);
        if (post.Content is not null)
        {
            post.Content = _htmlSanitizer.Sanitize(post.Content);
        }

        post.Created = DateTime.UtcNow;

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

        _logger.LogDebug("New post created with id: {Id}", post.Id);

        return Ok(post);
    }
}
