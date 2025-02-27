using Microsoft.Extensions.AI;
using Qdrant.Client;

namespace RAGScenario;

class SemanticSearch(
    IEmbeddingGenerator<string,Embedding<float>> embeddingGenerator, 
    QdrantClient qdrantClient)
{
    public async Task<string> SearchForIssues(string searchPhrase)
    {

        var queryEmbedding = await embeddingGenerator.GenerateEmbeddingVectorAsync(searchPhrase);

        var searchResults = await qdrantClient.SearchAsync("issues", queryEmbedding, limit: 5);

        return string.Join(Environment.NewLine, searchResults.Select(r => $"<issue id='{r.Id} , {r.Payload["title"].StringValue}/> "));
}
