using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qdrant.Client;
using RAGScenario;

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();
IChatClient innerChatClient = new OllamaChatClient(new Uri("http://127.0.0.1:11434/"), modelId: "llama3.1");

IEmbeddingGenerator<String, Embedding<float>> embeddingGenerator = new OllamaEmbeddingGenerator(
    new Uri("http://127.0.0.1:11434/"),
    modelId: "llama3"
    );

hostBuilder.Services.AddSingleton(new QdrantClient(new Uri("http://127.0.0.1:6334")));
// hostBuilder.Services.AddEmbeddingGenerator (embeddingGenerator);
hostBuilder.Services.AddEmbeddingGenerator<string, Embedding<float>>(embeddingGenerator);
hostBuilder.Services.AddChatClient(innerChatClient).UseFunctionInvocation();
hostBuilder.Services.AddSingleton<SemanticSearch>();

var app = hostBuilder.Build();
var semanticSearch = app.Services.GetRequiredService<SemanticSearch>();
var chatClient = app.Services.GetRequiredService<IChatClient>();
var searchTool = AIFunctionFactory.Create(semanticSearch.SearchForIssues);
var chatOptions = new ChatOptions
{
    Tools = new List<AITool> { (AITool)searchTool }
};



//Load data from PDF

var pdfPath = "d:\\UtasOr-UTBF-kulfold.pdf";
await semanticSearch.LoadDataFromPDF(pdfPath);

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Hello! I'll tell you what people are saying on dotnet/runtime");

var messages = new List<ChatMessage>
{
    new (ChatRole.System,"""
        Search for issues related to the user's question. Summarize the general
        vibe of what developers are saying about it. Cite the issues inline,
        e.g., "Developers love the performance (#number)".
        """)
};

while (true)
{

    // READ INPUT
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n> ");
    var input = Console.ReadLine()!;
    messages.Add(new(ChatRole.User, input));

    // PRODUCE RESPONSE
    var response = chatClient.GetStreamingResponseAsync(messages, chatOptions);
    Console.ForegroundColor = ConsoleColor.Yellow;
    await foreach (var chunk in response)
    {
        Console.Write(chunk.Text);
    }
}