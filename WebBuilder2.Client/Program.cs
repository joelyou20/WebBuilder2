using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Net;
using System.Net.Http.Headers;
using WebBuilder2.Client;
using WebBuilder2.Client.Clients;
using WebBuilder2.Client.Clients.Contracts;
using WebBuilder2.Client.Managers;
using WebBuilder2.Client.Managers.Contracts;
using WebBuilder2.Client.Services;
using WebBuilder2.Client.Services.Contracts;
using WebBuilder2.Client.Utils.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

WebAssemblyHostConfiguration configuration = builder.Configuration;

Settings settings = configuration.GetSection("Settings").Get<Settings>()!;

builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Services.AddLogging();

// CLIENTS ==========================>

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// HACK: Currently using JSON file as a stand in for a DB until I set one up
//builder.Services.AddHttpClient<IDatabaseClient, JsonClient>(client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//    client.DefaultRequestHeaders.Accept.Add(
//     new MediaTypeWithQualityHeaderValue("application/json"));
//});
// <--- END OF HACK --->
builder.Services.AddHttpClient<ISiteClient, SiteClient>(client => { client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!); });
builder.Services.AddHttpClient<IGithubClient, GithubClient>(client => { client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!); });
builder.Services.AddHttpClient<IAwsClient, AwsClient>(client => { client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!); });
builder.Services.AddHttpClient<IRepositoryClient, RepositoryClient>(client => { client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!); });
builder.Services.AddHttpClient<IScriptClient, ScriptClient>(client => { client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!); });

// <================== END OF CLIENTS

// MANAGERS ==========================>

builder.Services.AddScoped<IConnectionManager, ConnectionManager>();
builder.Services.AddScoped<ISiteManager, SiteManager>();
builder.Services.AddScoped<IGithubTemplateManager, GithubTemplateManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

// <================== END OF MANAGERS

// SERVICES ==========================>

builder.Services.AddScoped<IGithubService, GithubService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IAwsService, AwsService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IScriptService, ScriptService>();

// <================== END OF SERVICES

builder.Services.AddMudServices();
var app = builder.Build();
await app.RunAsync();
