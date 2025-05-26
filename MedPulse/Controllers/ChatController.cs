using MedPulse.Services;
using Microsoft.AspNetCore.Mvc;
using PostmarkDotNet;
using PostmarkDotNet.Webhooks;

namespace MedPulse.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly IUserService _userService;
    private readonly IPostmarkService _postmarkService;

    public ChatController(ILogger<ChatController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    [HttpGet(Name = "pen")]
    public async Task<IActionResult> Get(PostmarkInboundWebhookMessage message)
    {
        try
        {
            if (message == null || String.IsNullOrEmpty(message.From))
            {
                return BadRequest("Invalid request. Message cannot be null.");
            }
            await _userService.CreateRequestContextAsync(message);
            await _postmarkService.SendEmailAsync();
            
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogCritical("An error occurred while processing the request: {Message}", e.Message);
            return StatusCode(500, $"{e.Message}");
        }
    }
}