using System.Text;
using ClinicService.Data;
using ClinicService.Data.Storage;
using ClinicService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

RegisterData(builder.Services);
ConfigureLogs(builder.Services, builder.Host);

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

builder.Services
    .AddAuthorization()
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationService.TokenSecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

//app.UseHttpLogging(); // Not support for gRPC
app.UseWhen(
    ctx => ctx.Request.ContentType != "application/grpc",
    b => b.UseHttpLogging());

app.MapGrpcService<ClinicAuthenticationService>();
app.MapGrpcService<ClinicClientService>();
app.MapGrpcService<ClinicPetService>();

app.Run();


static void ConfigureLogs(IServiceCollection services, IHostBuilder host)
{
    services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
        logging.RequestHeaders.Add("Authorization");
        logging.RequestHeaders.Add("X-Real-IP");
        logging.RequestHeaders.Add("X-Forwarded-For");
    });


    host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();

    }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
}

static void RegisterData(IServiceCollection services)
{
    services.AddDbContext<ClinicContext>((provider, options) => options
        .UseLazyLoadingProxies()
        .UseSqlServer(provider.GetRequiredService<IConfiguration>()["Settings:DatabaseOptions:ConnectionString"]));

    services
        .AddScoped<IPetRepository, PetRepository>()
        .AddScoped<IConsultationRepository, ConsultationRepository>()
        .AddScoped<IClientRepository, ClientRepository>();
}