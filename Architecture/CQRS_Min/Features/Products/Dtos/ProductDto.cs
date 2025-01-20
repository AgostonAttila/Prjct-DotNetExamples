namespace CQRS_Min.Features.Products.Dtos
{
    public record ProductDto(Guid Id, string Name, string Description, decimal Price);
}
