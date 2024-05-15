
using Conventus.Server.Models;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CommentsController(
    ApplicationDbContext context,
    HtmlSanitizer sanitizer,
    ILogger<CommentsController> logger)
    : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = context;
    private readonly HtmlSanitizer _htmlSanitizer = sanitizer;
    private readonly ILogger<CommentsController> _logger = logger;

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> Get([FromQuery] Pager pager)
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
    public async Task<ActionResult<CommentDto>> Get(Guid id)
    {
        var comment = await _dbContext.Comments.FindAsync(id);
        if (comment is null)
        {
            return NotFound();
        }
        return Ok(comment.ToDto());
    }

    [HttpGet("by-post/{postId}")]
    public ActionResult<IEnumerable<CommentDto>> Get(Guid postId, [FromQuery] Pager pager)
    {
        if (!pager.IsValid())
        {
            return BadRequest();
        }

        var comments = _dbContext.Comments
            .Where(x => x.PostId == postId)
            .Skip((pager.Page - 1) * pager.Length).Take(pager.Length);

        return Ok(comments.Select(x => x.ToDto()));
    }

    [HttpGet("count")]
    public Task<int> GetCount()
    {
        return _dbContext.Comments.CountAsync();
    }

    [HttpGet("by-post/{postId}/count")]
    public Task<int> GetCount(Guid postId)
    {
        var comments = _dbContext.Comments
            .Where(x => x.PostId == postId);

        return comments.CountAsync();
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
