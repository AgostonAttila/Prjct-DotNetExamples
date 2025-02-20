using AutoMapper;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Exceptions;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;


public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a restaurant with id: {RestaurantId} with {UpdateREstaurant}",request.Id,request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        mapper.Map(request, restaurant);   

        await restaurantsRepository.SaveChanges();       
    }
}

