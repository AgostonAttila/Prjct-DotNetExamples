using FastEndpoints;
using REPR.Repository;
using REPR.Shared.BookDtos.GetById;

namespace REPR.Endpoints.Books
{
    public class BookGetByIdEndpoint : Endpoint<BookByIdRequest, BookByIdResponse>
    {
        private readonly FakeDb _db;

        public BookGetByIdEndpoint(FakeDb db) => _db = db;

        public override void Configure()
        {
            Get("/api/books/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(BookByIdRequest req, CancellationToken ct)
        {
            var book = await _db.GetBookById(req.id);

            if (book is null)
                await SendNotFoundAsync();
            else
                await SendAsync(new BookByIdResponse($"{book.id} - {book.name}"));
        }      
    }
}
