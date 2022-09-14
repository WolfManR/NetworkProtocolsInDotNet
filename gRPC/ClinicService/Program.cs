using ClinicService.Data;
using ClinicService.Services;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<ClinicContext>((provider, options) => options
    .UseLazyLoadingProxies()
    .UseSqlServer(provider.GetRequiredService<IConfiguration>()["Settings:DatabaseOptions:ConnectionString"]));

ConfigureLogs(builder.Services, builder.Host);


var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseHttpLogging(); // Not support for gRPC
app.UseWhen(
    ctx => ctx.Request.ContentType != "application/grpc",
    b => b.UseHttpLogging());

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

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