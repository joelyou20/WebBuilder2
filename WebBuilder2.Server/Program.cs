using Microsoft.EntityFrameworkCore;
using Octokit;
using Serilog;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Repositories;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;
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
builder.Services.AddScoped<IAwsRoute53DomainsService, AwsRoute53DomainsService>();
builder.Services.AddScoped<IAwsCostExplorerService, AwsCostExplorerService>();
builder.Services.AddScoped<IAwsSecretsManagerService, AwsSecretsManagerService>();
builder.Services.AddScoped<IAwsAmplifyService, AwsAmplifyService>();
builder.Services.AddScoped<IGithubService, GithubService>();
builder.Services.AddScoped<IGoogleAdSenseService, GoogleAdSenseService>();

builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<IScriptRepository, ScriptRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddAwsSecretsManagerClient();
builder.Services.AddAwsS3Client();
builder.Services.AddAwsRoute53Client();
builder.Services.AddAwsRoute53DomainsClient();
builder.Services.AddAwsCostExplorerClient();
builder.Services.AddAwsAmplifyClient();
builder.Services.AddGitHubClient(sp => sp.GetRequiredService<IAwsSecretsManagerService>(), configuration);

builder.Services.Configure<GoogleSettings>(configuration.GetSection(nameof(GoogleSettings)));

builder.Services.AddAdSenseService(sp => sp.GetRequiredService<IAwsSecretsManagerService>(), configuration);

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

app.UseSerilogIngestion();
//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
