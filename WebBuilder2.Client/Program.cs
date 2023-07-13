using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
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
using WebBuilder2.Client.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

WebAssemblyHostConfiguration configuration = builder.Configuration;

Settings settings = configuration.GetSection("Settings").Get<Settings>()!;

// CLIENTS ==========================>

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// HACK: Currently using JSON file as a stand in for a DB until I set one up
builder.Services.AddHttpClient<IDatabaseClient, JsonClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(
     new MediaTypeWithQualityHeaderValue("application/json"));
});
// <--- END OF HACK --->
builder.Services.AddHttpClient<ISiteClient, SiteClient>(client =>
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!);
});

builder.Services.AddHttpClient<IGithubClient, GithubClient>(client =>
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("ServerUrl")!);
});

// <================== END OF CLIENTS

// MANAGERS ==========================>

builder.Services.AddScoped<IConnectionManager, ConnectionManager>();

// <================== END OF MANAGERS

// SERVICES ==========================>

builder.Services.AddScoped<IGithubConnectionService, GithubConnectionService>();
builder.Services.AddScoped<ISiteService, SiteService>();

// <================== END OF SERVICES

builder.Services.AddMudServices();

var app = builder.Build();

await app.RunAsync();
