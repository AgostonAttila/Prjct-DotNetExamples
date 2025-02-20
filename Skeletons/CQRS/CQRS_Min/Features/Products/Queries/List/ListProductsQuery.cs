using CQRS_Min.Features.Products.Dtos;
using MediatR;

namespace CQRS_Min.Features.Products.Queries.List
{
    public record ListProductsQuery : IRequest<List<ProductDto>>;
}
