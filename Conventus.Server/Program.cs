using Conventus.Server;
using Conventus.Server.Extensions;
using Ganss.Xss;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// set up database
builder.Services.AddDbContext<ApplicationDbContext>();

// add html sanitizer
builder.Services.AddScoped<HtmlSanitizer>();

var app = builder.Build();

// ensure database is created and running
await app.MigrateDatabaseAsync();

app.UseDefaultFiles();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

await app.RunAsync();
