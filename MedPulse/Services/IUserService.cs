using PostmarkDotNet.Webhooks;

namespace MedPulse.Services;

public interface IUserService
{
    Task CreateRequestContextAsync(PostmarkInboundWebhookMessage context);
}