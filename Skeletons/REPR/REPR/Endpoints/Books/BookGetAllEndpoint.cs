using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using REPR.Models;
using REPR.Repository;

namespace REPR.Endpoints
{
    //[HttpGet("api/books")]
    //[AllowAnonymous]
    public class BookGetAllEndpoint : EndpointWithoutRequest<List<Book>>
    {

        private readonly FakeDb _db;

        public BookGetAllEndpoint(FakeDb db) => _db = db;

        public override void Configure()
        {
            Get("/api/books");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var books = await _db.GetAllBooks();

            await SendAsync(books,cancellation: ct);
        }

    }
}
