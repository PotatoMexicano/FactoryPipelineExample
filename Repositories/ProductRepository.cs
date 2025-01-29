using Microsoft.EntityFrameworkCore;
using Pipeline.Empresa.Context;
using Pipeline.Empresa.Entities;

namespace Pipeline.Empresa.Repositories;

public interface IProductRepository
{
    Task<Product?> GetProduct(Int32 id, CancellationToken cancellation);
    Task<ICollection<Product>> GetProducts(CancellationToken cancellation);
    Task<Product?> RegisterProduct(RegisterProductDTO request, CancellationToken cancellation);
}

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<Product?> GetProduct(Int32 id, CancellationToken cancellation)
    {
        return await context.Products.Where(x => x.Id == id).FirstOrDefaultAsync(cancellation);
    }

    public async Task<ICollection<Product>> GetProducts(CancellationToken cancellation)
    {
        return await context.Products.ToArrayAsync(cancellation);
    }

    public async Task<Product?> RegisterProduct(RegisterProductDTO request, CancellationToken cancellation)
    {
        Product product = new Product { Description = request.Description, Price = request.Price, Quantity = request.Quantity };

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellation);

        return product;
    }
}
