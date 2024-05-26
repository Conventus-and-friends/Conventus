using Conventus.Server.Models;
using Conventus.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server.Extensions;

public static class ApplicationDbContextExtensions
{
    private static readonly Func<ApplicationDbContext, long, Task<int>> _getPostsCountByCategoryId =
        EF.CompileAsyncQuery((ApplicationDbContext context, long id) =>
            context.Posts.Where(x => x.CategoryId == id).Count());

    private static readonly Func<ApplicationDbContext, long, int, int, IAsyncEnumerable<Post>> _getPostsByCategoryIdWithPaging =
        EF.CompileAsyncQuery((ApplicationDbContext context, long id, int skip, int take) =>
            context.Posts.Where(x => x.CategoryId == id).Skip(skip).Take(take));

    public static Task<int> GetPostsCountAsync(this ApplicationDbContext dbContext, long categoryId)
        => _getPostsCountByCategoryId(dbContext, categoryId);

    public static Task<int> GetPostsCountAsync(this ApplicationDbContext dbContext)
        => dbContext.Posts.CountAsync();

    public static IAsyncEnumerable<Post> GetPostsAsync(this ApplicationDbContext dbContext, long categoryId, Pager pager)
        => _getPostsByCategoryIdWithPaging(dbContext, categoryId, (pager.Page - 1) * pager.Length, pager.Length);

    private static readonly Func<ApplicationDbContext, Guid, Task<int>> _getCommentsCountByPostId =
        EF.CompileAsyncQuery((ApplicationDbContext context, Guid id) =>
            context.Comments.Where(x => x.PostId == id).Count());

    private static readonly Func<ApplicationDbContext, Guid, int, int, IAsyncEnumerable<Comment>> _getCommentsByPostIdWithPaging =
        EF.CompileAsyncQuery((ApplicationDbContext context, Guid id, int skip, int take) =>
            context.Comments.Where(x => x.PostId == id).OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.TimeCreated).Skip(skip).Take(take));

    public static Task<int> GetCommentsCountAsync(this ApplicationDbContext dbContext, Guid postId)
        => _getCommentsCountByPostId(dbContext, postId);

    public static Task<int> GetCommentsCountAsync(this ApplicationDbContext dbContext)
        => dbContext.Comments.CountAsync();

    public static IAsyncEnumerable<Comment> GetCommentsAsync(this ApplicationDbContext dbContext, Guid postId, Pager pager)
        => _getCommentsByPostIdWithPaging(dbContext, postId, (pager.Page - 1) * pager.Length, pager.Length);
}
