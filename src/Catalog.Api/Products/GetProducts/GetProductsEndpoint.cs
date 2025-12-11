
namespace Catalog.Api.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
            async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());
                return Results.Ok(result);
            }).
            WithName("GetProducts").
            Produces<GetProductsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GetProducts")
            .WithDescription("Get Products List");



     }
}
