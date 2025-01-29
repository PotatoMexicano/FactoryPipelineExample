using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pipeline.Empresa.Constants;
using Pipeline.Empresa.Entities;
using Pipeline.Empresa.Pipeline;
using Pipeline.Empresa.Repositories;

namespace Pipeline.Empresa.Controllers;
[Route("api/product")]
[ApiController]
public class ProductController(IProductRepository repository, IEmpresaFactory empresaFactory) : ControllerBase
{
    [HttpGet("{idProduct:int}", Name = "GetProduct")]
    [Authorize]
    public async Task<ActionResult<Product?>> GetProduct(Int32 idProduct, [FromQuery] Int32? idEmpresa = null, CancellationToken cancellation = default)
    {
        Product? product = await repository.GetProduct(idProduct, cancellation);

        if (product == null) return NotFound(new ProblemDetails { Detail = "Produto não encontrado." });

        IEmpresa empresa = empresaFactory.GetEmpresa(idEmpresa);
        ProductDTO customDTO = empresa.GenerateDTO(product);

        return Ok(customDTO);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ICollection<Product>>> GetProducts([FromQuery] Int32? idEmpresa = null, CancellationToken cancellation = default)
    {
        ICollection<Product> products = await repository.GetProducts(cancellation);

        IEmpresa empresa = empresaFactory.GetEmpresa(idEmpresa);
        ICollection<ProductDTO> customDTOs = empresa.GenerateDTOs(products);

        return Ok(customDTOs);
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult<Product>> RegisterProduct([FromBody] RegisterProductDTO request, CancellationToken cancellation = default)
    {
        Product? entity = await repository.RegisterProduct(request, cancellation);

        if (entity == null) return BadRequest(new ProblemDetails { Detail = "Falha ao registrar o produto." });

        return CreatedAtRoute("GetProduct", new { idProduct = entity.Id }, entity);
    }
}