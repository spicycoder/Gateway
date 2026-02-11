using Common;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriberController(ILogger<SubscriberController> logger) : ControllerBase
{
    [HttpPost("subscribe")]
    [Topic("pubsub", "order")]
    public async Task<IActionResult> Subscribe([FromBody] Order order)
    {
        logger.LogInformation("Received {Order}", order);
        return Ok();
    }
}