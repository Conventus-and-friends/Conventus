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
            context.Posts.Where(x => x.CategoryId == id).OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.TimeCreated).Skip(skip).Take(take));

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

    // TODO: actual relevance
    private static readonly Func<ApplicationDbContext, int, IAsyncEnumerable<Post>> _getRelevantPosts =
        EF.CompileAsyncQuery((ApplicationDbContext context, int take) =>
            context.Posts.Include(x => x.Comments).Where(x => x.Comments.Count > 0)
            .OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.Comments.Count).Take(take));

    public static IAsyncEnumerable<Post> GetRelevantPostsAsync(this ApplicationDbContext dbContext, int take)
        => _getRelevantPosts(dbContext, take);

    // TODO: actual relevance
    private static readonly Func<ApplicationDbContext, long, Guid, int, IAsyncEnumerable<Post>> _getSimilarPosts =
        EF.CompileAsyncQuery((ApplicationDbContext context, long categoryId, Guid postId, int take) =>
            context.Posts.Where(x => x.CategoryId == categoryId && x.Id != postId)
            .OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.TimeCreated).Take(take));

    public static IAsyncEnumerable<Post> GetSimilarPostsAsync(this ApplicationDbContext dbContext, long categoryId, Guid postId, int take)
        => _getSimilarPosts(dbContext, categoryId, postId, take);

    // TODO: this has nothing to do with actual search
    private static readonly Func<ApplicationDbContext, string, int, IAsyncEnumerable<Post>> _getSearchResults =
        EF.CompileAsyncQuery((ApplicationDbContext context, string queryPattern, int take) =>
            context.Posts.Where(x => EF.Functions.Like(x.Title, queryPattern) || EF.Functions.Like(x.Content, queryPattern))
            .OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.TimeCreated).Take(take));

    public static IAsyncEnumerable<Post> GetSearchResultsAsync(this ApplicationDbContext dbContext, string query, int take)
        => _getSearchResults(dbContext, $"%{query}%", take);
}
