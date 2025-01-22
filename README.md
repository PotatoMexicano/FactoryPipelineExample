# Projeto ASP.NET - Gerar Relatório por Empresa

Este é um projeto simples em ASP.NET que demonstra como usar Dependency Injection (DI) e o padrão Factory para resolver dinamicamente instâncias de empresas com base em um identificador (`idEmpresa`) recebido via rota.

## Funcionalidades
- Cada empresa tem uma implementação específica do método `GerarRelatorioErvas`.
- A rota recebe o `idEmpresa` e utiliza uma fábrica para resolver a instância correta da empresa.
- O padrão Factory é usado para garantir flexibilidade e manutenção.

## Estrutura do Projeto

### Interfaces e Classes

#### Interface `IEmpresa`
Define o contrato que todas as empresas devem implementar:

```csharp
public interface IEmpresa
{
    List<string> GerarRelatorioErvas();
}
```

#### Implementações das Empresas
Cada empresa possui uma lógica específica para gerar relatórios:

```csharp
public class EmpresaA : IEmpresa
{
    public List<string> GerarRelatorioErvas()
    {
        return new List<string> { "Empresa A" };
    }
}

public class EmpresaB : IEmpresa
{
    public List<string> GerarRelatorioErvas()
    {
        return new List<string> { "Empresa B" };
    }
}
```

#### Fábrica `EmpresaFactory`
A fábrica resolve dinamicamente a instância com base no `idEmpresa`:

```csharp
public interface IEmpresaFactory
{
    IEmpresa GetEmpresa(int idEmpresa);
}

public class EmpresaFactory : IEmpresaFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmpresaFactory(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IEmpresa GetEmpresa(int idEmpresa)
    {
        return idEmpresa switch
        {
            1 => _serviceProvider.GetRequiredService<EmpresaA>(),
            2 => _serviceProvider.GetRequiredService<EmpresaB>(),
            _ => throw new NotImplementedException()
        };
    }
}
```

### Controller
O controller utiliza a fábrica para chamar a implementação correta:

```csharp
[ApiController]
[Route("[controller]")]
public class RelatorioController : ControllerBase
{
    private readonly IEmpresaFactory _empresaFactory;

    public RelatorioController(IEmpresaFactory empresaFactory)
    {
        _empresaFactory = empresaFactory;
    }

    [HttpGet("{idEmpresa:int}")]
    public IActionResult GerarRelatorio(int idEmpresa)
    {
        try
        {
            var empresa = _empresaFactory.GetEmpresa(idEmpresa);
            var relatorio = empresa.GerarRelatorioErvas();
            return Ok(relatorio);
        }
        catch (NotImplementedException)
        {
            return BadRequest("Empresa inválida");
        }
    }
}
```

### Configuração de DI
Registre as dependências no `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EmpresaA>();
builder.Services.AddScoped<EmpresaB>();
builder.Services.AddScoped<IEmpresaFactory, EmpresaFactory>();

builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
app.MapControllers();

app.Run();
```

## Como Executar

1. Clone o repositório:
   ```bash
   git clone <url-do-repositorio>
   ```

2. Navegue até o diretório do projeto e restaure os pacotes:
   ```bash
   cd <diretorio-do-projeto>
   dotnet restore
   ```

3. Execute o projeto:
   ```bash
   dotnet run
   ```

4. Teste a API fazendo requisições para as rotas:
   - `GET /relatorio/1` → Retorna **["Empresa A"]**
   - `GET /relatorio/2` → Retorna **["Empresa B"]**
   - `GET /relatorio/3` → Retorna **Empresa inválida**

## Dependências
- .NET 7.0 ou superior
- ASP.NET Core

## Estrutura de Pastas (Sugestão)

```
|-- Controllers
|   |-- RelatorioController.cs
|
|-- Factories
|   |-- EmpresaFactory.cs
|
|-- Interfaces
|   |-- IEmpresa.cs
|
|-- Implementations
|   |-- EmpresaA.cs
|   |-- EmpresaB.cs
|
|-- Program.cs
```

## Melhorias Futuras
- Adicionar logs para auditoria de chamadas.
- Implementar autenticação e autorização nas rotas.
- Suporte a novas empresas sem alterar a fábrica (usar `Dictionary` ou padrão `Abstract Factory`).

---
**Desenvolvido com ❤ por [Seu Nome].**

