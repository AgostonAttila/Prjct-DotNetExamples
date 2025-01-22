using FastEndpoints;
using REPR.Repository;
using REPR.Shared.BookDtos.GetById;
using REPR.Shared.BookDtos.Post;

namespace REPR.Endpoints.Books
{   
    public class BookPostEndpoint : Endpoint<BookByIdRequest, BookByIdResponse>
    {
        private readonly FakeDb _db;

        public BookPostEndpoint(FakeDb db) => _db = db;

        public override void Configure()
        {
            Post("/api/books");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
        {
            var createdBook = await  _db.CreateBook(req.name);

            var response = new CreateBookResponse(createdBook.id, createdBook.name, createdBook.price);

            await SendCreatedAtAsync<BookGetByIdEndpoint>(new { response.id }, response, cancellation: ct);
        }

       
    }
}
