namespace Pipeline.Empresa;

public interface IEmpresa
{
    List<String> GerarRelatorioErvas();
}

public class EmpresaA : IEmpresa
{
    public List<String> GerarRelatorioErvas()
    {
        return ["Empresa A"];
    }
}

public class EmpresaB : IEmpresa
{
    public List<String> GerarRelatorioErvas()
    {
        return ["Empresa B"];
    }
}

public interface IEmpresaFactory
{
    IEmpresa GetEmpresa(Int32 idEmpresa);
}

public class EmpresaFactory : IEmpresaFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmpresaFactory(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IEmpresa GetEmpresa(Int32 idEmpresa)
    {
        return idEmpresa switch
        {
            1 => _serviceProvider.GetRequiredService<EmpresaA>(),
            2 => _serviceProvider.GetRequiredService<EmpresaB>(),
            _ => throw new NotImplementedException()
        };
    }
}
