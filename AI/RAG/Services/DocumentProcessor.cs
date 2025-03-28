using UglyToad.PdfPig;
using HtmlAgilityPack;

namespace RAGApplication.Services;

public class DocumentProcessor : IDocumentProcessor
{
    public async Task<string> ExtractTextFromPdf(string pdfPath)
    {
        var text = new StringBuilder();
        
        using (var document = PdfDocument.Open(pdfPath))
        {
            foreach (var page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
        }

        return text.ToString();
    }

    public async Task<string> ExtractTextFromWebsite(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        
        // Remove script and style elements
        var nodes = doc.DocumentNode.SelectNodes("//script|//style");
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                node.Remove();
            }
        }

        return doc.DocumentNode.InnerText.Trim();
    }

    public List<string> ChunkText(string text, int maxChunkSize = 1000)
    {
        var chunks = new List<string>();
        var sentences = text.Split(new[] { ". ", ".\n", "! ", "? " }, 
            StringSplitOptions.RemoveEmptyEntries);

        var currentChunk = new StringBuilder();
        
        foreach (var sentence in sentences)
        {
            if (currentChunk.Length + sentence.Length > maxChunkSize)
            {
                chunks.Add(currentChunk.ToString().Trim());
                currentChunk.Clear();
            }
            
            currentChunk.AppendLine(sentence + ".");
        }

        if (currentChunk.Length > 0)
        {
            chunks.Add(currentChunk.ToString().Trim());
        }

        return chunks;
    }
}