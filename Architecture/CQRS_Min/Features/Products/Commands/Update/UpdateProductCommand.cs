using CQRS_Min.Features.Products.Dtos;
using MediatR;

namespace CQRS_Min.Features.Products.Commands.Update
{    
    public record UpdateProductCommand(string Guid,string Name, string Description, decimal Price) : IRequest<bool>;
}
