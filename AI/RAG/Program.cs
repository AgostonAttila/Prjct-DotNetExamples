using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RAGApplication.Services;
using RAGApplication.Models;

namespace RAGApplication;

class Program
{
    static async Task Main(string[] args)
    {
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        var ragService = serviceProvider.GetService<IRagService>();

        while (true)
        {
            Console.WriteLine("\nRAG Application Menu:");
            Console.WriteLine("1. Process PDF");
            Console.WriteLine("2. Process Website");
            Console.WriteLine("3. Ask Question");
            Console.WriteLine("4. Exit");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter PDF path: ");
                    var pdfPath = Console.ReadLine();
                    if (!string.IsNullOrEmpty(pdfPath))
                    {
                        await ragService.ProcessPdfDocument(pdfPath);
                    }
                    break;

                case "2":
                    Console.Write("Enter website URL: ");
                    var url = Console.ReadLine();
                    if (!string.IsNullOrEmpty(url))
                    {
                        await ragService.ProcessWebsite(url);
                    }
                    break;

                case "3":
                    Console.Write("Enter your question: ");
                    var question = Console.ReadLine();
                    if (!string.IsNullOrEmpty(question))
                    {
                        var answer = await ragService.AskQuestion(question);
                        Console.WriteLine($"\nAnswer: {answer}");
                    }
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // Add configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        
        // Register services
        services.AddSingleton<IDocumentProcessor, DocumentProcessor>();
        services.AddSingleton<IVectorStore, PgVectorStore>();
        services.AddSingleton<IOllamaService, OllamaService>();
        services.AddSingleton<IRagService, RagService>();

        return services;
    }
}