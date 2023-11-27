using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace MonitorMe;

public static class Statistic
{
    /**
     * Prefix of statistic controller
     */
    public const string RoutePrefix = "statistic";

    /**
     * Total request received
     */
    private static int _totalTransaction;

    /**
     * Success request response
     */
    private static int _successTransaction;

    /**
     * Failed request response
     */
    private static int _failedTransaction;

    /**
     * Application serve time
     */
    private static readonly DateTime ServeTime = Process.GetCurrentProcess().StartTime;

    public static void BeforeResponse(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments($"/{RoutePrefix}"))
        {
            _totalTransaction += 1;
        }
    }

    public static void AfterResponse(HttpContext context, Exception? exception)
    {
        if (!context.Request.Path.StartsWithSegments($"/{RoutePrefix}"))
        {
            if (exception is not null || (context.Response.StatusCode / 100) > 3)
            {
                _failedTransaction += 1;
            }
            else
            {
                _successTransaction += 1;
            }
        }
    }

    public static void Reset()
    {
        _totalTransaction = 0;
        _successTransaction = 0;
        _failedTransaction = 0;
    }

    /**
     * Convert class data to an object
     */
    public static object ToObject()
    {
        return new
        {
            serve_time = ServeTime.ToString("yyyy-MM-dd hh:mm:ss"),
            uptime_second = (DateTime.Now - ServeTime).TotalSeconds,
            transaction = new
            {
                total = _totalTransaction,
                success = _successTransaction,
                failed = _failedTransaction,
                online = _totalTransaction - (_successTransaction + _failedTransaction),
            },
        };
    }
}