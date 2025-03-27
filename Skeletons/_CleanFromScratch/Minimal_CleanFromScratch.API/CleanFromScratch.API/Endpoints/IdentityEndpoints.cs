using CleanFromScratch.Application.Users.Commands.AssignUserRole;
using CleanFromScratch.Application.Users.Commands.UnassignUserRole;
using CleanFromScratch.Application.Users.Commands.UpdateUserDetails;
using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace CleanFromScratch.API.Endpoints;

public  class IdentityEndpoints : IMinimalEndpoint
{
    public  void MapRoutes( IEndpointRouteBuilder routeBuilder)
    {

        var restaurantsGroup = routeBuilder.MapGroup("api/identity")
         .WithTags("Identity");


        restaurantsGroup.MapPatch("/user", [Authorize]
        async (IMediator mediator, [FromBody] UpdateUserDetailsCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        restaurantsGroup.MapPost("/userRole", [Authorize(Roles = UserRoles.Admin)]
        async (IMediator mediator, [FromBody] AssignUserRoleCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        restaurantsGroup.MapDelete("/userRole", [Authorize(Roles = UserRoles.Admin)] async ([FromServices] IMediator mediator, [FromBody] UnassignUserRoleCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}
