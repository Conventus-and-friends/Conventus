using Conventus.Server.Extensions;
using Conventus.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public string DbPath { get; }

    public ApplicationDbContext(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        // set up default values to system folder
        // TODO: read from config
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "conventus.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        // TODO: more db options
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapPostCategoryRelations();
        modelBuilder.MapCommentPostRelations();
        base.OnModelCreating(modelBuilder);
    }
}
