using CleanFromScratch.Appplication.Restaurants.Dtos;
using FluentValidation;

namespace CleanFromScratch.Appplication.Restaurants.Validators;

public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validVategories = new List<string>
    {       
        "Italian",
        "Chinese",
        "Mexican",
        "American",
        "Polish"
    };

    public CreateRestaurantValidator()
    {
        RuleFor(x => x.Name)
            .Length(3,100);
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Desc is required");

        RuleFor(x => x.Category)
            //.Custom((value, context) =>
            //{
            //    if (!validVategories.Contains(value))
            //    {
            //        context.AddFailure("Category is not valid");
            //    }
            //})
            .Must(x => validVategories.Contains(x))            
            .WithMessage("Invalid category");

        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Postalcode is not valid");
    }
}
