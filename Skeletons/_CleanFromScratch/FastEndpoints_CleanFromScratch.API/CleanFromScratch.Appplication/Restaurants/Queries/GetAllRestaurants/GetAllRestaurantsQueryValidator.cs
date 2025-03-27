using CleanFromScratch.Appplication.Restaurants.Dtos;
using FluentValidation;

namespace CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];
    private string[] allowSortByColumn = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize).
            Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",",allowPageSizes)}]");

        RuleFor(x => x.SortBy).
           Must(value => allowSortByColumn.Contains(value))
           .When(x => !string.IsNullOrEmpty(x.SortBy))
           .WithMessage($"Sort by is optional , or must be in [{string.Join(",", allowPageSizes)}]");
    }
}
