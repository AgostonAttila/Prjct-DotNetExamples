namespace RAGApplication.Services;

public interface IOllamaService
{
    Task<float[]> GetEmbeddings(string text);
    Task<string> GenerateResponse(string prompt, List<string> context);
}