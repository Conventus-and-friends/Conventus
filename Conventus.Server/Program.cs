using Conventus.Server;
using Conventus.Server.Consumers;
using Conventus.Server.Extensions;
using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.Entities;
using Conventus.Server.Workers;
using Ganss.Xss;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// set up database
builder.Services.AddDbContext<ApplicationDbContext>();

// set up identity
builder.Services
    .AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// add caching
// TODO: add actual distrubuted caching
builder.Services.AddDistributedMemoryCache();

// set up Quartz
builder.Services.AddQuartz(q =>
{
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
});
builder.Services.AddQuartzServer(q => q.WaitForJobsToComplete = true);

// set up authentication/authorization
builder.Services.AddConventusOpenIddict();
builder.Services.AddHostedService<ClientRegistrationWorker>();

// add html sanitizer
builder.Services.AddScoped<HtmlSanitizer>();

// set up MassTransit
builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<CreatePostConsumer>();
    c.AddConsumer<CreateCommentConsumer>();
    c.AddConsumer<CreateCategoryConsumer>();

    c.AddRequestClient<CreatePost>();
    c.AddRequestClient<CreateComment>();
    c.AddRequestClient<CreateCategory>();

    c.UsingInMemory((context, cfg) =>
        cfg.ConfigureEndpoints(context));
});

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
app.UseCors();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.MapFallbackToFile("/index.html");

await app.RunAsync();
