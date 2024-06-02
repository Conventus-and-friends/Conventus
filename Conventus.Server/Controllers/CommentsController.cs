
using Conventus.Server.Extensions;
using Conventus.Server.Models;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CommentsController(
    ApplicationDbContext context,
    HtmlSanitizer sanitizer,
    IDistributedCache cache,
    ILogger<CommentsController> logger)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly HtmlSanitizer _htmlSanitizer = sanitizer;
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<CommentsController> _logger = logger;

    private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany([FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        return Ok(_dbContext.Comments
                .Skip((pager.Page - 1) * pager.Length).Take(pager.Length)
                .Select(x => x.ToDto()));
    }

    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<CommentDto>> GetById(Guid id)
    {
        var comment = await _cache.GetOrCreateAsync($"comments.comment_{id}", async () =>
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment is null)
            {
                return null;
            }
            return comment.ToDto();
        }, _cacheOptions);

        if (comment is null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    [HttpGet("by-post/{postId}")]
    public ActionResult<IAsyncEnumerable<CommentDto>> GetByPost(Guid postId, [FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        var comments = _dbContext.GetCommentsAsync(postId, pager);

        return Ok(comments.Select(x => x.ToDto()));
    }

    [HttpGet("count")]
    public Task<int> GetCount()
    {
        return _dbContext.GetCommentsCountAsync();
    }

    [HttpGet("by-post/{postId}/count")]
    public Task<int> GetCountByPost(Guid postId)
    {
        return _dbContext.GetCommentsCountAsync(postId);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Post([FromBody] CommentDto comment)
    {
        if (!comment.IsValid())
        {
            return BadRequest();
        }

        comment.Content = _htmlSanitizer.Sanitize(comment.Content);

        comment.Created = DateTime.UtcNow;

        var result = await _dbContext.Comments.AddAsync(comment.ToEntity());

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        comment.Id = result.Entity.Id;

        _logger.LogDebug("Added comment {Id} to post with id: {PostId}", comment.Id, comment.PostId);

        return Ok(comment);
    }
}
