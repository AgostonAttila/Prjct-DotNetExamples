namespace RAGApplication.Models;

public class Document
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; }
    public string Source { get; set; }
    public DateTime ProcessedAt { get; set; }
    public float[] Embedding { get; set; }
}