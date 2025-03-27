using CleanFromScratch.Application.Users.Commands.AssignUserRole;
using CleanFromScratch.Application.Users.Commands.UnassignUserRole;
using CleanFromScratch.Application.Users.Commands.UpdateUserDetails;
using CleanFromScratch.Domain.Constants;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;

public class UpdateUserDetailsEndpoint(IMediator _mediator) : Endpoint<UpdateUserDetailsCommand>
{
   
    public override void Configure()
    {
        Patch("api/user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserDetailsCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendNoContentAsync(ct);
    }
}

[Authorize(Roles = UserRoles.Admin)]
public class AssignUserRoleEndpoint(IMediator _mediator) : Endpoint<AssignUserRoleCommand>
{
      public override void Configure()
    {
        Post("api/userRole");
    }

    public override async Task HandleAsync(AssignUserRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendNoContentAsync(ct);
    }
}

[Authorize(Roles = UserRoles.Admin)]
public class UnassignUserRoleEndpoint(IMediator _mediator) : Endpoint<UnassignUserRoleCommand>
{
   

    public override void Configure()
    {
        Delete("api/userRole");
    }

    public override async Task HandleAsync(UnassignUserRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendNoContentAsync(ct);
    }
}
