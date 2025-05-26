using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MedPulse.Services.Extensions;

public interface IChatHistoryReducer
{
    Task<IEnumerable<ChatMessageContent>?> ReduceAsync(ChatHistory chatHistory, CancellationToken cancellationToken);
}