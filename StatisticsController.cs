using Microsoft.AspNetCore.Mvc;

namespace MonitorMe;

[Route($"{Statistic.RoutePrefix}/[action]")]
public abstract class StatisticsController
{
    [HttpGet]
    public object List()
    {
        return Statistic.ToObject();
    }
    
    [HttpPut]
    public void Reset()
    {
        Statistic.Reset();
    }
}