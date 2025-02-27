using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.AI.OpenAI;
using Azure.Core;


var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();

IChatClient innerChatClient = new OllamaChatClient(
    new Uri("https://127.0.0.1:11434"),
    "llama3.1");

//IChatClient innerChatClient = new AzureOpenAIClient(new(hostBuilder.Configuration["AzureOpenAIKey"],))
//    .AsChatClient("gpt-4o-mini");




hostBuilder.Services.AddChatClient(builder => builder     
    .UseFunctionInvocation()
    .Use(innerChatClient));


hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

var app = hostBuilder.Build();  
var chatClient = app.Services.GetRequiredService<IChatClient>();

//whole
var response = await chatClient.GetResponseAsync("What is AI?Reply in max 10 words");
Console.WriteLine(response.Message.Text);

//Stream
//var responseStream = await chatClient.CompleteStreamingAsync("What is AI?Reply at least in  1000 words");
//await foreach (var chunk in responseStream)
//{
//    Console.Write(chunk.Text);
//}

//OPENAI version
//var response = await chatClient.GetResponseAsync("What is AI? Reply in max 10 words");

//if (response.RawRepresentation is OpenAI.Chat.ChatCompletion openAiCompletion)
//{
//    Console.WriteLine($"OpenAI finderprint: { openAiCompletion.SystemFingerprint}");
//}


