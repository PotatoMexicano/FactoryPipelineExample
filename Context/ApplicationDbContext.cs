using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pipeline.Empresa.Entities;
using Pipeline.Empresa.Repositories;

namespace Pipeline.Empresa.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.MappingProduct();
    }
}

public static class MappingEntities
{
    public static void MappingProduct(this ModelBuilder builder)
    {
        builder.Entity<Product>().HasKey(x => x.Id);
    }
}

public static class DatabaseExtensions
{
    public static async void SeedDatabase(this WebApplication app)
    {
        IServiceScope scope = app.Services.CreateScope();

        IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        CancellationTokenSource cts = new CancellationTokenSource();

        Product[] products = new[]
        {
            new Product
            {
                Id = 1,
                Description = "Batata",
                Price = 82,
                Quantity = 2000,
            },
            new Product {
                Id = 2,
                Description = "Uva",
                Price = 82,
                Quantity = 120
            }
        };

        foreach (Product product in products)
        {
            Product? productEntity = await productRepository.GetProduct(product.Id, cts.Token);
            if (productEntity == null) await productRepository.RegisterProduct(new RegisterProductDTO(product.Description!, product.Price, product.Quantity), cts.Token);
        }
    }
}