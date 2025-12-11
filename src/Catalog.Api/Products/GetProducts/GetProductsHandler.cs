
namespace Catalog.Api.Products.GetProducts;


public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
public class GetProductsQueryHandler(IDocumentSession documentSession, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetProductsQuery");

        var products =await documentSession.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
         
    }
}
