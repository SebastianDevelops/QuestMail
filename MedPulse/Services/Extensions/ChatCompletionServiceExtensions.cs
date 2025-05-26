using Microsoft.SemanticKernel.ChatCompletion;

namespace MedPulse.Services.Extensions;

internal static class ChatCompletionServiceExtensions
{
    public static IChatCompletionService UsingChatHistoryReducer(this IChatCompletionService service, IChatHistoryReducer reducer)
    {
        return new ChatCompletionServiceWithReducer(service, reducer);
    }
}