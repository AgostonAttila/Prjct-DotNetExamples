using MediatR;

namespace CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand(int id) : IRequest
{
    public int Id { get; set; } = id;

}
