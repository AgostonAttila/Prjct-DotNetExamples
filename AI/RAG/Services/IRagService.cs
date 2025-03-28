namespace RAGApplication.Services;

public interface IRagService
{
    Task ProcessPdfDocument(string pdfPath);
    Task ProcessWebsite(string url);
    Task<string> AskQuestion(string question);
}