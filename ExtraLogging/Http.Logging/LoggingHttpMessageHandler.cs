using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Http.Logging;

public class LoggingHttpMessageHandler : DelegatingHandler
{
    public LoggingHttpMessageHandler(
        ILogger logger,
        IOptions<HttpClientLoggingOptions> options)
    {
        Logger = logger;
        Options = options.Value;
    }

    public ILogger Logger { get; }

    public HttpClientLoggingOptions Options { get; }

    [DebuggerStepThrough]
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // generate correlation id
        Guid correlationId = Guid.NewGuid();

        await LogRequest(correlationId, request, cancellationToken);

        // Measure execution time
        Stopwatch stopwatch = new();
        stopwatch.Start();

        // execute request 
        HttpResponseMessage response;
        try
        {
            // execute request
            response = await base.SendAsync(request, cancellationToken);

            // stop measuring time 
            stopwatch.Stop();
        }
        catch (Exception exception)
        {
            // stop measuring time 
            stopwatch.Stop();

            // tracing response
            if (!Logger.IsEnabled(LogLevel.Error)) throw;

            StringBuilder responseText = new();
            responseText.AppendLine($"Response #{correlationId}");
            responseText.AppendLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
            responseText.AppendLine($"Exception: {exception.Message}");

            // tracing output
            Logger.LogInformation(responseText.ToString());

            // re-throw
            throw;
        }

        await LogResponse(correlationId, stopwatch.ElapsedMilliseconds, response, cancellationToken);
        
        return response;
    }

    private async ValueTask LogRequest(Guid correlationId, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // check log level
        if (!Logger.IsEnabled(LogLevel.Trace)) return;

        // tracing request
        StringBuilder requestText = new();
        requestText.AppendLine($"Request #{correlationId}");

        // parse URI
        Uri? requestUri = request.RequestUri;
        requestText.AppendLine($"Host: {requestUri?.Host}");
        requestText.AppendLine($"Path: {requestUri?.AbsolutePath}");
        requestText.AppendLine($"QueryString: {requestUri?.Query}");
        requestText.AppendLine($"Method: {request.Method}");
        requestText.AppendLine($"Scheme: {requestUri?.Scheme}");

        // tracing request headers
        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            foreach (string value in header.Value)
            {
                requestText.AppendLine($"{header.Key}: {value}");
            }
        }

        // tracing request contents 
        if (IsSupportLoggingContentType(request.Content?.Headers.ContentType))
        {
            // read body
            string requestBody = await request.Content.ReadAsStringAsync(cancellationToken);

            // truncate too long bodies
            if (requestBody.Length >= Options.MaxBodyLength)
            {
                requestBody = requestBody[..Options.MaxBodyLength] + "... <Truncated>";
            }

            // append log
            requestText.AppendLine("RequestBody:");
            requestText.AppendLine(requestBody);
        }
        else
        {
            // append log 
            requestText.AppendLine($"RequestBody: <{(request.Content is null ? "Empty" : "Not Logged")}>");
        }

        // tracing output
        Logger.LogInformation(requestText.ToString());
    }

    private async ValueTask LogResponse(Guid correlationId, long receivingTime, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        // check log level
        if (!Logger.IsEnabled(LogLevel.Trace)) return;

        // tracing response
        StringBuilder responseText = new();
        responseText.AppendLine($"Response #{correlationId}");
        responseText.AppendLine($"Elapsed: {receivingTime}ms");
        responseText.AppendLine($"StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}");

        // get response headers
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = response.Headers;

        // add content response headers
        headers = headers.Concat(response.Content.Headers);

        // tracing response headers
        foreach (KeyValuePair<string, IEnumerable<string>> header in headers.OrderBy(item => item.Key))
        {
            foreach (string value in header.Value)
            {
                responseText.AppendLine($"{header.Key}: {value}");
            }
        }

        // tracing response contents
        if (IsSupportLoggingContentType(response.Content.Headers.ContentType))
        {
            // fetch body 
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            // truncate too long bodies
            if (responseBody.Length >= Options.MaxBodyLength)
            {
                responseBody = responseBody[..Options.MaxBodyLength] + "... <Truncated>";
            }

            // append log 
            responseText.AppendLine("ResponseBody:");
            responseText.AppendLine(responseBody);
        }
        else
        {
            // append log 
            responseText.AppendLine("ResponseBody: <Not Logged>");
        }

        // tracing output
        Logger.LogInformation(responseText.ToString());
    }

    private bool IsSupportLoggingContentType([NotNullWhen(true)] MediaTypeHeaderValue? contentType)
    {
        if (contentType?.MediaType is not { } mediaType) return false;

        if (mediaType.EndsWith("/json") ||
            mediaType.EndsWith("/xml") ||
            mediaType.StartsWith("text/"))
        {
            return true;
        }

        // for multipart form data we allow body logging only
        // when explicitly specified
        if (mediaType.Contains("multipart/form-data"))
        {
            return Options.LogMultipartFormData;
        }

        // allow body logging 
        return true;
    }
}