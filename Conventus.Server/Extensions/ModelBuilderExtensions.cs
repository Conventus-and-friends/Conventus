using Conventus.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Add parent relation between <see cref="Post"/> and <see cref="Category"/>
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void MapPostCategoryRelations(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(x => x.Posts)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();
    }

    /// <summary>
    /// Add parent relation between <see cref="Comment"/> and <see cref="Post"/>
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void MapCommentPostRelations(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .IsRequired();
    }
}
