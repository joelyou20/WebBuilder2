using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http.Headers;
using WebBuilder2;
using WebBuilder2.Clients;
using WebBuilder2.Clients.Contracts;
using WebBuilder2.Services;
using WebBuilder2.Services.Contracts;
using WebBuilder2.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = builder.Configuration.GetSection("Settings").Get<Settings>()!;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<IDatabaseClient, JsonClient>(client => {
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(
     new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddScoped<ISiteService, SiteService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
