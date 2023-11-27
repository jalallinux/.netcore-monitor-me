using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MonitorMe;

public class MonitorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MonitorMiddleware> _logger;

    public MonitorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory?.CreateLogger<MonitorMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            Statistic.BeforeResponse(context);
            await this._next(context);
            Statistic.AfterResponse(context, null);
        }
        catch (Exception exception)
        {
            Statistic.AfterResponse(context, exception);
            throw;
        }
    }
}

public static class MonitorMiddlewareExtensions
{
    public static IApplicationBuilder UseMonitorMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MonitorMiddleware>();
    }
}