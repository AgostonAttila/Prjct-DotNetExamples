﻿using CQRS_Min.Domain;
using CQRS_Min.Persistence;
using MediatR;

namespace CQRS_Min.Features.Products.Commands.Create
{
    public class CreateProductCommandHandler(AppDbContext context) : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product(command.Name, command.Description, command.Price);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product.Id;
        }
    }
}
