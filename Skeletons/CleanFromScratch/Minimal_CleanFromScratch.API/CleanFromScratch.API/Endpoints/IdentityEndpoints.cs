using CleanFromScratch.Application.Users.Commands.AssignUserRole;
using CleanFromScratch.Application.Users.Commands.UnassignUserRole;
using CleanFromScratch.Application.Users.Commands.UpdateUserDetails;
using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace CleanFromScratch.API.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
     


        endpoints.MapPatch("/user", [Authorize]
        async (IMediator mediator, UpdateUserDetailsCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        endpoints.MapPost("/userRole", [Authorize(Roles = UserRoles.Admin)]
        async (IMediator mediator, AssignUserRoleCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        endpoints.MapDelete("/userRole", [Authorize(Roles = UserRoles.Admin)] async (IMediator mediator, UnassignUserRoleCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}
