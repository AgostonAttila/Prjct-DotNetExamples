using OllamaSharp;

var uri = new Uri("http://localhost:11434"); 
var ollama = new OllamaApiClient(uri);

ollama.SelectedModel = "phi3.5:latest";

var chat = new Chat(ollama);
while (true)
{
    Console.Write("User>");
    var message = Console.ReadLine();
    Console.Write("Assistant>");
    await foreach (var answerToken in chat.SendAsync(message))
        Console.Write(answerToken);
    Console.WriteLine();
}