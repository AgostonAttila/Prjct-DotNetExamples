using Carter;
using CleanFromScratch.Application.Users.Commands.AssignUserRole;
using CleanFromScratch.Application.Users.Commands.UnassignUserRole;
using CleanFromScratch.Application.Users.Commands.UpdateUserDetails;
using CleanFromScratch.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Endpoints
{
    public class IdentityModule : CarterModule
    {
        public IdentityModule()
      : base("/api/identity")
        {
            WithTags("Identity");
            IncludeInOpenApi();        
        }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/user", [Authorize]
            async (IMediator mediator, [FromBody] UpdateUserDetailsCommand command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            });

            app.MapPost("/userRole", [Authorize(Roles = UserRoles.Admin)]
            async (IMediator mediator, [FromBody] AssignUserRoleCommand command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            });

            app.MapDelete("/userRole", [Authorize(Roles = UserRoles.Admin)] async ([FromServices] IMediator mediator, [FromBody] UnassignUserRoleCommand command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            });
        }

    }

}