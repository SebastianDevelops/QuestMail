using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MedPulse.Services.Extensions;

public class ChatHistoryTruncationReducerAdapter : IChatHistoryReducer
{
    private readonly ChatHistoryTruncationReducer _inner;

    public ChatHistoryTruncationReducerAdapter(int truncatedSize)
    {
        _inner = new ChatHistoryTruncationReducer(truncatedSize);
    }

    public Task<IEnumerable<ChatMessageContent>?> ReduceAsync(ChatHistory chatHistory, CancellationToken cancellationToken)
    {
        return _inner.ReduceAsync(chatHistory, cancellationToken);
    }
}