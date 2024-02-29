using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OpenVisStreamer.VideoLibrary.Controllers;

[ApiController]
public class HealthController(HealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
       var report = healthCheckService.CheckHealthAsync().Result;
       
       if(report.Status == HealthStatus.Healthy)
            return Ok(report);
        else
            return StatusCode(500);
       
    }
}