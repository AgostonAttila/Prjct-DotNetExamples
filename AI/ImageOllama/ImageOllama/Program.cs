using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddChatClient(new Microsoft.Extensions.AI.OllamaChatClient(new Uri("http://localhost:11434"), "llava:7b"));

var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

//var message = new ChatMessage(ChatRole.User, "What is in this image?");
//message.Contents.Add(new DataContent(File.ReadAllBytes("images/traffic1.png"), "image/png"));

//var response = await chatClient.GetResponseAsync(message);
//Console.WriteLine(response.Messages[0].Text);

foreach (var filePath in Directory.GetFiles("images", "*.png"))
{

    var name = Path.GetFileNameWithoutExtension(filePath);
    var message2 = new ChatMessage(ChatRole.User,
        $"""
        Extract information from this image from camera {name}.
        The image from the camera may be blurry, so take that into account when extracting information.
        Pay extra attention to columns of cars and try to accurately count the number of cars.
        And pay extra - special attention to pedestrians, as we want to make sure that we spot them.
        """);

    message2.Contents.Add(new DataContent(File.ReadAllBytes(filePath), "image/png"));

    var trafficCamResult = await chatClient.GetResponseAsync<TrafficCamResult>(message2);

    Console.WriteLine(trafficCamResult.Result with { CameraName = name });
}


public record TrafficCamResult
{
    public string CameraName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrafficStatus Status { get; set; }
    public int NumberOfCars { get; set; }
    public int NumberOfTrucks { get; set; }
    public int NumberOfPedestrians { get; set; }

    public enum TrafficStatus
    {
        Empty,
        Normal,
        Heavy,
        Blocked
    }

}