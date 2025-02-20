using FluentValidation;

namespace CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)             
            .Length(3,100);

        RuleFor(x => x.Description)
            .NotEmpty();                     
    }
}
