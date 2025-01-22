using Microsoft.AspNetCore.Mvc;

namespace Pipeline.Empresa.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly IEmpresaFactory _empresaFactory;

    public RelatorioController(IEmpresaFactory empresaFactory)
    {
        this._empresaFactory = empresaFactory;
    }

    [HttpGet("{idEmpresa:int}")]
    public IActionResult GerarRelatorio(Int32 idEmpresa)
    {
        IEmpresa empresa = _empresaFactory.GetEmpresa(idEmpresa);
        List<String> result = empresa.GerarRelatorioErvas();

        return Ok(result);
    }
}
