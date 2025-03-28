namespace RAGApplication.Services;

public class RagService : IRagService
{
    private readonly IDocumentProcessor _documentProcessor;
    private readonly IVectorStore _vectorStore;
    private readonly IOllamaService _ollamaService;

    public RagService(
        IDocumentProcessor documentProcessor,
        IVectorStore vectorStore,
        IOllamaService ollamaService)
    {
        _documentProcessor = documentProcessor;
        _vectorStore = vectorStore;
        _ollamaService = ollamaService;
    }

    public async Task ProcessPdfDocument(string pdfPath)
    {
        Console.WriteLine($"Processing PDF: {pdfPath}");
        
        var text = await _documentProcessor.ExtractTextFromPdf(pdfPath);
        var chunks = _documentProcessor.ChunkText(text);
        
        foreach (var chunk in chunks)
        {
            var embedding = await _ollamaService.GetEmbeddings(chunk);
            
            var document = new Document
            {
                Content = chunk,
                Source = pdfPath,
                ProcessedAt = DateTime.UtcNow,
                Embedding = embedding
            };

            await _vectorStore.StoreDocument(document);
        }

        Console.WriteLine("PDF processing complete!");
    }

    public async Task ProcessWebsite(string url)
    {
        Console.WriteLine($"Processing website: {url}");
        
        var text = await _documentProcessor.ExtractTextFromWebsite(url);
        var chunks = _documentProcessor.ChunkText(text);
        
        foreach (var chunk in chunks)
        {
            var embedding = await _ollamaService.GetEmbeddings(chunk);
            
            var document = new Document
            {
                Content = chunk,
                Source = url,
                ProcessedAt = DateTime.UtcNow,
                Embedding = embedding
            };

            await _vectorStore.StoreDocument(document);
        }

        Console.WriteLine("Website processing complete!");
    }

    public async Task<string> AskQuestion(string question)
    {
        Console.WriteLine("Processing question...");
        
        var questionEmbedding = await _ollamaService.GetEmbeddings(question);
        var relevantDocs = await _vectorStore.SearchSimilarDocuments(questionEmbedding);
        
        var context = relevantDocs.Select(d => d.Content).ToList();
        var answer = await _ollamaService.GenerateResponse(question, context);
        
        return answer;
    }
}