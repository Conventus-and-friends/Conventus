
using Conventus.Server;
using Conventus.Server.Models;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Mappers;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;

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
}