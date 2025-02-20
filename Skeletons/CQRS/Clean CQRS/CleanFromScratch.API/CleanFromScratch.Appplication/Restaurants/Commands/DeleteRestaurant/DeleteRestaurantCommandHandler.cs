using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanFromScratch.Domain.Exceptions;

namespace CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting a restaurant with id: {REstaurantId}",request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        await restaurantsRepository.Delete(restaurant);       
    }
}
