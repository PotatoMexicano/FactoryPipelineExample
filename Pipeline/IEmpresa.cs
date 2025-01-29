using Pipeline.Empresa.Entities;

namespace Pipeline.Empresa.Pipeline;

public interface IEmpresa
{
    ProductDTO GenerateDTO(Product product);
    ICollection<ProductDTO> GenerateDTOs(ICollection<Product> products);
}

public class EmpresaA : IEmpresa
{
    public ProductDTO GenerateDTO(Product product)
    {
        return new ProductDTO
        {
            Price = product.Price / (Double)100,
            CreatedAt = product.CreatedAt,
            Description = product.Description?.ToLower(),
            Id = product.Id,
            Quantity = product.Quantity,
            PriceTotal = product.Price * (Double)product.Quantity / 100
        };
    }

    public ICollection<ProductDTO> GenerateDTOs(ICollection<Product> products)
    {
        return products.Select(GenerateDTO).ToArray();
    }
}

public class EmpresaB : IEmpresa
{
    public ProductDTO GenerateDTO(Product product)
    {
        return new ProductDTO
        {
            Price = product.Price / (Double)100,
            Description = product.Description?.ToUpper(),
            Quantity = product.Quantity,
            PriceTotal = product.Price * (Double)product.Quantity / 100
        };
    }

    public ICollection<ProductDTO> GenerateDTOs(ICollection<Product> products)
    {
        return products.Select(GenerateDTO).ToArray();
    }
}

public class EmpresaGenerica : IEmpresa
{
    public ProductDTO GenerateDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Description = product.Description,
            Quantity = product.Quantity,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
        };
    }

    public ICollection<ProductDTO> GenerateDTOs(ICollection<Product> products)
    {
        return products.Select(GenerateDTO).ToArray();
    }
}

public interface IEmpresaFactory
{
    IEmpresa GetEmpresa(Int32? idEmpresa);
}

public class EmpresaFactory : IEmpresaFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmpresaFactory(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IEmpresa GetEmpresa(Int32? idEmpresa)
    {
        if (!idEmpresa.HasValue) return _serviceProvider.GetRequiredService<EmpresaGenerica>();

        return idEmpresa switch
        {
            1 => _serviceProvider.GetRequiredService<EmpresaA>(),
            2 => _serviceProvider.GetRequiredService<EmpresaB>(),
            _ => _serviceProvider.GetRequiredService<EmpresaGenerica>(),
        };
    }
}
