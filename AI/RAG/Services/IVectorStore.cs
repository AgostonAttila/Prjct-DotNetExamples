namespace RAGApplication.Services;

public interface IVectorStore
{
    Task StoreDocument(Document document);
    Task<List<Document>> SearchSimilarDocuments(float[] queryEmbedding, int limit = 5);
}