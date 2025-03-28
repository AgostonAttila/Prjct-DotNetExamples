using System.Text.Json;
using System.Text;

namespace RAGApplication.Services;

public class OllamaService : IOllamaService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _modelName;

    public OllamaService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
        _modelName = configuration["Ollama:ModelName"] ?? "llama2";
    }

    public async Task<float[]> GetEmbeddings(string text)
    {
        var request = new
        {
            model = _modelName,
            prompt = text
        };

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/api/embeddings",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content);
        
        return result["embedding"].EnumerateArray()
            .Select(x => x.GetSingle())
            .ToArray();
    }

    public async Task<string> GenerateResponse(string prompt, List<string> context)
    {
        var contextText = string.Join("\n\n", context);
        var fullPrompt = $"Context:\n{contextText}\n\nQuestion: {prompt}\n\nAnswer:";

        var request = new
        {
            model = _modelName,
            prompt = fullPrompt,
            stream = false
        };

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/api/generate",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
        
        return result["response"];
    }
}