using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Octokit;
using WebBuilder2.Server.Services;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Settings;
using WebBuilder2.Server.Utils;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var githubSettings = configuration.GetSection("GithubSettings").Get<GithubSettings>()!;
builder.Services.AddScoped<IGithubService, GithubService>();
builder.Services.AddScoped(sp => new GitHubClient(new ProductHeaderValue(githubSettings.OrganizationName))
    {
        Credentials = new Credentials(githubSettings.Token)
    });

builder.Services.AddScoped<IAwsS3Service, AwsS3Service>();

AwsSettings awsSettings = configuration.GetSection("AwsSettings").Get<AwsSettings>()!;
BasicAWSCredentials awsCredentials = new(awsSettings.AccessKey, awsSettings.SecretKey);
AmazonS3Config awsConfig = new()
{
    UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
    RegionEndpoint = AwsConfig.RegionEndpoint
};

builder.Services.AddScoped(sp => new AmazonS3Client(awsCredentials, awsConfig));

var app = builder.Build();

IEnumerable<string> allowedOrigins = configuration.GetSection("AllowedOrigins").Get<IEnumerable<string>>()!;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseCors(options => options
    .WithOrigins(allowedOrigins.ToArray())
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
