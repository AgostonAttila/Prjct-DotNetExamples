using System.ComponentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var ollamaEndpoint = builder.Configuration["AI__Ollama__Endpoint"] ?? "http://localhost:11434";
var chatModel = builder.Configuration["AI__Ollama__ChatModel"] ?? "llama3.2";

IChatClient client = new OllamaChatClient(ollamaEndpoint, modelId: chatModel)
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

[Description("Gets the weather")]
string GetWeather() => Random.Shared.NextDouble() > 0.5 ? "It's sunny" : "It's raining";

[Description("Gets the location")]
string GetLocation() => "Vienna, Austria";

[Description("Gets the weather and location")]
string GetWeatherAndLocation() => $"{GetWeather()} in {GetLocation()}";

var chatOptions = new ChatOptions
{
    Tools =
    [
        AIFunctionFactory.Create(GetWeather),
        AIFunctionFactory.Create(GetLocation),
        AIFunctionFactory.Create(GetWeatherAndLocation)
    ]
};

builder.Services.AddChatClient(client);

builder.Services.AddOpenApi();

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Cache()
               .Expire(TimeSpan.FromMinutes(5)));
});

var app = builder.Build();

app.MapOpenApi().CacheOutput();

app.MapScalarApiReference();

app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

app.MapPost("/chat", async (IChatClient client, string message) =>
{
    try
    {
        var response = await client.CompleteAsync(
            message,
            chatOptions,
            cancellationToken: default);

        return Results.Ok(new { response });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Chat completion failed",
            detail: ex.Message);
    }
});

app.Run();
