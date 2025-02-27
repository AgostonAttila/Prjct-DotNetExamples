
using Microsoft.Extensions.AI;
using System.Numerics.Tensors;

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OllamaEmbeddingGenerator(new Uri("https://127.0.0.1:11434"), modelId: "all-minilm");


var result = await embeddingGenerator.GenerateEmbeddingAsync("Cats are better than dogs");

//Console.WriteLine($"Vector of lenght {result.Vector.Length}");
//foreach (var value in result.Vector.Span)
//{
//    Console.Write("{0:0.00}", value);
//}

// EMBEDDING

var candidates = new string[] {"Onboarding PRocess for New Employees","UndertandingOur Common Strength" };

Console.Write("Genereating embeddings for candidates...");
var candidateEmbeddings = await embeddingGenerator.GenerateAndZipAsync(candidates);

Console.WriteLine(candidateEmbeddings);

// SEMANTIC SEARCH

while (true)
{
    Console.Write("\nQuery: ");
    var input = Console.ReadLine();
    if (input == "") break;

    var inputEmbedding = await embeddingGenerator.GenerateEmbeddingAsync(input);

    var closest = 
        from candidate in candidateEmbeddings
        let similarity = TensorPrimitives.CosineSimilarity(candidate.Embedding.Vector.Span, inputEmbedding.Vector.Span)
        orderby similarity descending
        select new {Test = candidate.Value, Similarity = similarity};
}