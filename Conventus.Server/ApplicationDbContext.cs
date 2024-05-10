using Conventus.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Conventus.Server;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public string DbPath { get; }

    public ApplicationDbContext(string dbPath)
    {
        DbPath = dbPath;
    }

    public ApplicationDbContext()
    {
        // set up default values to system folder
        // TODO: read from config
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "conventus.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: more db options
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }
}
