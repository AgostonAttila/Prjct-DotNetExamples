using Duende.Bff.Blazor.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddBffBlazorClient()
    .AddLocalApiHttpClient<WeatherHttpClient>();

builder.Services.AddSingleton<IWeatherClient, WeatherHttpClient>();

builder.Services
    .AddCascadingAuthenticationState();

await builder.Build().RunAsync();
