using MediatR;

namespace CQRS_Min.Features.Products.Commands.Delete
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
