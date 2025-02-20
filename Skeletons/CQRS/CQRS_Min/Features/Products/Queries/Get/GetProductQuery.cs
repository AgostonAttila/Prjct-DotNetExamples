using CQRS_Min.Features.Products.Dtos;
using MediatR;

namespace CQRS_Min.Features.Products.Queries.Get
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}
