using Common;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublisherController(
    DaprClient daprClient,
    ILogger<PublisherController> logger) : ControllerBase
{
    [HttpPost("publish")]
    public async Task<IActionResult> Publish([FromBody] Order order)
    {
        logger.LogInformation("Publishing {Order}", order);
        await daprClient.PublishEventAsync(
            "pubsub",
            "order",
            order);

        return Ok();
    }
}