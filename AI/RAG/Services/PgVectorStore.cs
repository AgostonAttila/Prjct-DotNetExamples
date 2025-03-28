using Npgsql;
using Pgvector;
using Pgvector.Npgsql;

namespace RAGApplication.Services;

public class PgVectorStore : IVectorStore
{
    private readonly string _connectionString;

    public PgVectorStore(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("VectorDB");
        InitializeDatabase().Wait();
    }

    private async Task InitializeDatabase()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        // Enable vector extension
        await using (var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS vector", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        // Create documents table
        await using (var cmd = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS documents (
                id TEXT PRIMARY KEY,
                content TEXT,
                source TEXT,
                processed_at TIMESTAMP,
                embedding vector(384)
            )", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task StoreDocument(Document document)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(@"
            INSERT INTO documents (id, content, source, processed_at, embedding)
            VALUES (@id, @content, @source, @processed_at, @embedding)
            ON CONFLICT (id) DO UPDATE 
            SET content = @content, 
                source = @source, 
                processed_at = @processed_at, 
                embedding = @embedding", conn);

        cmd.Parameters.AddWithValue("id", document.Id);
        cmd.Parameters.AddWithValue("content", document.Content);
        cmd.Parameters.AddWithValue("source", document.Source);
        cmd.Parameters.AddWithValue("processed_at", document.ProcessedAt);
        cmd.Parameters.AddWithValue("embedding", new Vector(document.Embedding));

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Document>> SearchSimilarDocuments(float[] queryEmbedding, int limit = 5)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(@"
            SELECT id, content, source, processed_at, embedding
            FROM documents
            ORDER BY embedding <-> @query_embedding
            LIMIT @limit", conn);

        cmd.Parameters.AddWithValue("query_embedding", new Vector(queryEmbedding));
        cmd.Parameters.AddWithValue("limit", limit);

        var documents = new List<Document>();
        await using var reader = await cmd.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            documents.Add(new Document
            {
                Id = reader.GetString(0),
                Content = reader.GetString(1),
                Source = reader.GetString(2),
                ProcessedAt = reader.GetDateTime(3),
                Embedding = ((Vector)reader.GetValue(4)).ToArray()
            });
        }

        return documents;
    }
}