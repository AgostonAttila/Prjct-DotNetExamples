using CQRS_Min.Domain;
using CQRS_Min.Features.Products.Commands.Create;
using CQRS_Min.Features.Products.Commands.Delete;
using CQRS_Min.Features.Products.Commands.Update;
using CQRS_Min.Features.Products.Notifications;
using CQRS_Min.Features.Products.Queries.Get;
using CQRS_Min.Features.Products.Queries.List;
using CQRS_Min.Persistence;
using MediatR;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
    var product = await mediatr.Send(new GetProductQuery(id));
    if (product == null) return Results.NotFound();
    return Results.Ok(product);
});

app.MapGet("/products", async (ISender mediatr) =>
{
    var products = await mediatr.Send(new ListProductsQuery());
    return Results.Ok(products);
});

app.MapPost("/products", async (CreateProductCommand command, ISender mediatr) =>
{
    var productId = await mediatr.Send(command);
    if (Guid.Empty == productId) return Results.BadRequest();
    //notificatopn
    await mediatr.Publish(new ProductCreatedNotification(productId));
    return Results.Created($"/products/{productId}", new { id = productId });
});

app.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
    await mediatr.Send(new DeleteProductCommand(id));
    return Results.NoContent();
});

app.MapPatch("/products", async (UpdateProductCommand command, ISender mediatr) =>
{
    var result = await mediatr.Send(command);
    if (!result) return Results.BadRequest();
    return Results.Ok(result);
});

app.Run();

