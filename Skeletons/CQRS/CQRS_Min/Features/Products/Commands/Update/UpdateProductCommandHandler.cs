using CQRS_Min.Domain;
using CQRS_Min.Features.Products.Commands.Create;
using CQRS_Min.Features.Products.Dtos;
using CQRS_Min.Persistence;
using MediatR;

namespace CQRS_Min.Features.Products.Commands.Update
{

    public class UpdateProductCommandHandler(AppDbContext context) : IRequestHandler<UpdateProductCommand, bool>
    {
        public async Task<bool> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(command.Guid);
            if (product == null)
            {
                return false;
            }

            context.Products.Update(product);

            await context.SaveChangesAsync();
            return true;
        }
    }
}
