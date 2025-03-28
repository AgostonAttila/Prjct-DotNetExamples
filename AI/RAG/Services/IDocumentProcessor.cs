namespace RAGApplication.Services;

public interface IDocumentProcessor
{
    Task<string> ExtractTextFromPdf(string pdfPath);
    Task<string> ExtractTextFromWebsite(string url);
    List<string> ChunkText(string text, int maxChunkSize = 1000);
}