using Catalog.Api.Publisher;

namespace Catalog.Api.Products.CreateProduct;


public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProdutcHandler(IDocumentSession documentSession,  EmailPublisher emailPublisher) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        documentSession.Store(product);
        await documentSession.SaveChangesAsync(cancellationToken);
       
       await emailPublisher.Publish("mirtalibemirli217@gmail.com");
        return new CreateProductResult( product.Id);
            
            }
}
