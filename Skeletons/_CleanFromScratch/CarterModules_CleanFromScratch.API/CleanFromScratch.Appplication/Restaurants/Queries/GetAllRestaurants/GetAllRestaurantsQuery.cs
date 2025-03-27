using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Application.Common;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using MediatR;

namespace CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

}
