using Microsoft.AspNetCore.Mvc;
using Pipeline.Empresa.Pipeline;

namespace Pipeline.Empresa.Controllers;
[Route("api/relatorio")]
[ApiController]
public class RelatorioController(IEmpresaFactory empresaFactory) : ControllerBase
{
    [HttpGet("{idEmpresa:int}")]
    public IActionResult GerarRelatorio(Int32 idEmpresa)
    {
        IEmpresa empresa = empresaFactory.GetEmpresa(idEmpresa);
        List<String> result = empresa.GerarRelatorioErvas();

        return Ok(result);
    }
}
