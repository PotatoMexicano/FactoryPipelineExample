using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pipeline.Empresa.Context;
using Pipeline.Empresa.Entities;
using Pipeline.Empresa.Pipeline;
using Pipeline.Empresa.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pipeline.Empresa;

public class Program
{
    public static void Main(String[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("http://localhost:5001");

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Warning)
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .Enrich.WithExceptionDetails()
            .CreateLogger();

        builder.Host.UseSerilog(Log.Logger);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("InMemoryDb"));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        Byte[] secretJwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtToken:Secret").Value ?? String.Empty);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretJwtToken),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        builder.Services.AddScoped<EmpresaA>();
        builder.Services.AddScoped<EmpresaB>();
        builder.Services.AddScoped<EmpresaGenerica>();
        builder.Services.AddScoped<IEmpresaFactory, EmpresaFactory>();

        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.RespectNullableAnnotations = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.SeedDatabase();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        try
        {
            app.Run();
        }
        finally
        {
            Log.Information("Server shutting down...");
            Log.CloseAndFlush();
        }
    }

}
