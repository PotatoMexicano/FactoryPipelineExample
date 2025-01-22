namespace Pipeline.Empresa;

public class Program
{
    public static void Main(String[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<EmpresaA>();
        builder.Services.AddScoped<EmpresaB>();
        builder.Services.AddScoped<IEmpresaFactory, EmpresaFactory>();

        // Add services to the container.

        builder.Services.AddControllers();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
