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
    public async Task<IActionResult> Get()
    {
        try
        {
            var sampleMessage = new PostmarkInboundWebhookMessage
            {
                From = "user@example.com",
                FromName = "John Doe",
                FromFull = new FromFull { Email = "user@example.com", Name = "John Doe" },
                To = "companion@medpulse.com",
                ToFull = new List<PostmarkDotNet.ToFull> 
                { 
                    new() { Email = "companion@medpulse.com", Name = "Fantasy Companion" } 
                },
                Subject = "My wellness journey update",
                TextBody = "Dear Companion, I managed to complete my exercise goals this week and got good sleep. What's new in Eldoria?",
                MessageID = Guid.NewGuid(),
                Date = DateTime.UtcNow.ToString("R"),
                Headers = new List<Header>
                {
                    new() { Name = "Content-Type", Value = "text/plain" }
                }
            };
            
            if (sampleMessage == null || String.IsNullOrEmpty(sampleMessage.From))
            {
                return BadRequest("Invalid request. Message cannot be null.");
            }
            await _userService.CreateRequestContextAsync(sampleMessage);
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