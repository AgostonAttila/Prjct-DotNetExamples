using MediatR;

namespace CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommand : IRequest<int>
{
    //[StringLength(100,MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    //[Required(ErrorMessage ="Insert valid category")]
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    //[EmailAddress(ErrorMessage ="Please provide a valid phone number")]
    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }

    //[RegularExpression(@"^\d{2}-\d{3}$",ErrorMessage ="Wrong postal code")]
    public string? PostalCode { get; set; }
}
