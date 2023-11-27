## Using steps

### 1. Apply middleware in `Program.cs`
```csharp
using MonitorMe;
...
app.UseMonitorMiddleware();
```

### 2. Extend from `StatisticsController`
```csharp
[ApiController]
public class StatisticController: StatisticsController
{
    ...
}
```