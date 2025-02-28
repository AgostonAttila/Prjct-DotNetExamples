using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;


namespace CleanFromScratch.Domain.Interfaces
{
    public interface IRestaurantAuthorizationService
    {
        bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);
    }
}