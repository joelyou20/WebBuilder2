using Microsoft.EntityFrameworkCore;
using Octokit;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Server.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddScoped<DbContextOptions<AppDbContext>>();
builder.Services.AddScoped<AppDbContextFactory>();
builder.Services.AddScoped(dbContext => dbContext.GetRequiredService<AppDbContextFactory>().CreateDbContext(Array.Empty<string>()));
builder.Services.AddScoped<IAwsS3Service, AwsS3Service>();
builder.Services.AddScoped<IAwsRoute53Service, AwsRoute53Service>();
builder.Services.AddScoped<IAwsCostExplorerService, AwsCostExplorerService>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<IGithubService, GithubService>();

builder.Services.AddGitHubClient(configuration);
builder.Services.AddAwsS3Client();
builder.Services.AddAwsRoute53Client();
builder.Services.AddAwsCostExplorerClient();

var app = builder.Build();

IEnumerable<string> allowedOrigins = configuration.GetSection("AllowedOrigins").Get<IEnumerable<string>>()!;

app.UseCors(options => options
    .WithOrigins(allowedOrigins.ToArray())
    .AllowAnyHeader()
    .AllowAnyMethod()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
