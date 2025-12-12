
using Catalog.Api.Exceptions;

namespace Catalog.Api.Products.GetById;
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);


public class GetProductByIdQueryHandler (IDocumentSession documentSession): IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await documentSession.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        return new GetProductByIdResult(product);
    }
}
