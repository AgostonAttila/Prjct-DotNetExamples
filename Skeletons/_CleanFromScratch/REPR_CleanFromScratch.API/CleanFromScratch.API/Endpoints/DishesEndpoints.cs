using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanFromScratch.API.Endpoints;

public static class DishesEndpoints
{
    public static IEndpointRouteBuilder MapDishesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dishes").WithTags("Dishes");

        group.MapGet("/", GetAllDishes)
             .WithName("GetDishes")
             .WithOpenApi();

        group.MapGet("/{id}", GetDishById)
             .WithName("GetDishById")
             .WithOpenApi();

        group.MapPost("/", CreateDish)
             .WithName("CreateDish")
             .WithOpenApi();

        group.MapPut("/{id}", UpdateDish)
             .WithName("UpdateDish")
             .WithOpenApi();

        group.MapDelete("/{id}", DeleteDish)
             .WithName("DeleteDish")
             .WithOpenApi();

        return app;
    }

    private static async Task<Ok<List<DishResponse>>> GetAllDishes()
    {
        // Simulating some dishes for demonstration
        var dishes = new List<DishResponse>
        {
            new(1, "Spaghetti Carbonara", "Italian pasta dish", 12.99m),
            new(2, "Chicken Tikka Masala", "Indian curry dish", 14.99m)
        };

        return TypedResults.Ok(dishes);
    }

    private static async Task<Results<Ok<DishResponse>, NotFound>> GetDishById(int id)
    {
        // Simulating finding a dish
        var dish = new DishResponse(id, "Spaghetti Carbonara", "Italian pasta dish", 12.99m);
        
        if (dish == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(dish);
    }

    private static async Task<Created<DishResponse>> CreateDish(DishRequest request)
    {
        // Simulating dish creation
        var dish = new DishResponse(
            1, 
            request.Name, 
            request.Description, 
            request.Price);

        return TypedResults.Created($"/api/dishes/{dish.Id}", dish);
    }

    private static async Task<Results<Ok<DishResponse>, NotFound>> UpdateDish(int id, DishRequest request)
    {
        // Simulating dish update
        var dish = new DishResponse(
            id,
            request.Name,
            request.Description,
            request.Price);

        return TypedResults.Ok(dish);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteDish(int id)
    {
        // Simulating deletion
        return TypedResults.NoContent();
    }
}

public record DishRequest(string Name, string Description, decimal Price);

public record DishResponse(int Id, string Name, string Description, decimal Price);