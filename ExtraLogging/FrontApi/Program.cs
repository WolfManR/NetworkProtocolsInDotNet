using FrontApi.Services;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

using NLog.Web;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.RequestHeaders.Add("Authorization");
    logging.RequestHeaders.Add("X-Real-IP");
    logging.RequestHeaders.Add("X-Forwarded-For");
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();

}).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

builder.Services.AddHttpClientLogging();

builder.Services.AddHttpClient<RootApiClient>()
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
    onRetry: (response, sleepDuration, attemptNumber, context) =>
    {
        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
        logger?.LogError(
            response.Exception ?? new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
            "(attempt: {attemptNumber}) RootServiceClient request exception.", attemptNumber);
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpLogging();

app.MapGet(
        "/weatherforecast",
        async ([FromServices] RootApiClient client) => Results.Ok(await client.GetWeather()))
.WithName("GetWeatherForecast");

app.Run();