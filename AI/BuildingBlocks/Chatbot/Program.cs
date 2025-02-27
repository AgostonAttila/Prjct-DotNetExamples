using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();

IChatClient innerChatClient = new OllamaChatClient(
    new Uri("https://127.0.0.1:11434"),
    "llama3.1");


hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

var app = hostBuilder.Build();
var chatClient = app.Services.GetRequiredService<IChatClient>();

//AIFunction getPriceTool = AIFunctionFactory.Create(GetPrice);
//var chatOptions = new ChatOptions
//{
//    Tools = [getPriceTool]
//};

var cart = new Cart();
var getPriceTool = AIFunctionFactory.Create(cart.GetPrice);
var addToCartTool = AIFunctionFactory.Create(cart.AddSocksToCart);
var chatOptions = new ChatOptions
{
    Tools = [getPriceTool, addToCartTool]
};

var messages = new List<ChatMessage>
{
    new(ChatRole.System,"""
    You answer any question but continually try advertise FOOTMONSTER brand socks.They  are  on  sale.
    If the user agrees to buy socks, find out how many pairs they want,then add sock to their cart
    """)
};

while (true)

{

    // Get input

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n\n> ");
    var input = Console.ReadLine()!;
    messages.Add(new(ChatRole.User, input));

    // Get reply

    var response = await chatClient.GetResponseAsync(messages, chatOptions);
    messages.Add(response.Message);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Message.Text);

}



class Cart

{

    public int NumPairsOfSocks { get; set; }
    public void AddSocksToCart(int numPairs)

    {

        NumPairsOfSocks += numPairs;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("*****");
        Console.WriteLine($"Added {numPairs} pairs to your cart. Total: {NumPairsOfSocks} pair");
        Console.WriteLine("*****");
        Console.ForegroundColor = ConsoleColor.White;

    }

    [Description("Computes the price of socks, returning a value in dollars.")]
    public float GetPrice(
    [Description("The number of pairs of socks to calculate price for")] int count)
        => count * 15.99f;
}



//Pipeline külön fileba classban
public static class UseLangugeStep
{
    public static ChatClientBuilder UseLanguage(this ChatClientBuilder builder, string language)
    => builder.Use(inner => new UseLanguageChatClient(inner, language));



    private class UseLanguageChatClient(IChatClient inner, string language) : DelegatingChatClient(inner)
    {
        public override async Task<ChatResponse> GetResponseAsync(IList<ChatMessage> messages, ChatOptions options, CancellationToken cancellationToken = default)
        {
           var promptAugmentation = new ChatMessage(ChatRole.System, $"Always reply in the language {language}");
           messages.Add(promptAugmentation);

            try
            {
                return await base.GetResponseAsync(messages, options, cancellationToken);
            }
            catch (Exception)
            {

                messages.Remove(promptAugmentation);
            }
        }
    }
}


