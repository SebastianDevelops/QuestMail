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

    public ChatController(ILogger<ChatController> logger, IUserService userService, IPostmarkService postmarkService)
    {
        _logger = logger;
        _userService = userService;
        _postmarkService = postmarkService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Get([FromBody] PostmarkInboundWebhookMessage message)
    {
        try
        {
            if (message == null || String.IsNullOrEmpty(message.From))
            {
                return BadRequest("Invalid request. Message cannot be null.");
            }
            _logger.LogInformation("Received webhook. From: {FromEmail}, Subject: {Subject}", message.From, message.Subject);
            Console.WriteLine(message);
            await _userService.CreateRequestContextAsync(message);
            await _postmarkService.SendEmailAsync();
            
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogCritical("An error occurred while processing the request: {Message}", e.Message);
            return StatusCode(500, $"{e.Message}");
        }
    }
}