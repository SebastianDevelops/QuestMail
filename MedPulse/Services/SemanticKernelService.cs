using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using MedPulse.Infrastructure;
using MedPulse.Plugins;
using MedPulse.Plugins.Types;
using MedPulse.Repositories;
using MedPulse.Services.Extensions;
using MedPulse.ViewModel;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.Google;

namespace MedPulse.Services;

public class SemanticKernelService : ISemanticKernelService
{
    public readonly Kernel _kernel;
    private readonly Settings _settings;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly AzureOpenAIPromptExecutionSettings _azureOpenAiPromptExecutionSettingsGeminiSettings;
    private readonly IServiceProvider _serviceProvider;

    public SemanticKernelService(Settings settings, IServiceProvider serviceProvider)
    {
        _settings = settings;
        _serviceProvider = serviceProvider;
        var credentials = new AzureOpenAIClient(new Uri(_settings.AzureOpenAI.Endpoint), new AzureKeyCredential(_settings.AzureOpenAI.Apikey));
        var azureOpenAiChatService =  new AzureOpenAIChatCompletionService(_settings.AzureOpenAI.Model, credentials);
        var builder = Kernel.CreateBuilder();
        builder.Services.AddSingleton(_serviceProvider);
        _kernel = builder.Build();
        
        var userPlugin = ActivatorUtilities.CreateInstance<UserPlugin>(_serviceProvider);
        var trophyPlugin = ActivatorUtilities.CreateInstance<TrophyPlugin>(_serviceProvider);
        var companionPlugin = ActivatorUtilities.CreateInstance<CompanionPlugin>(_serviceProvider);
        
        _kernel.ImportPluginFromObject(userPlugin, "UserPlugin");
        _kernel.ImportPluginFromObject(trophyPlugin, "TrophyPlugin");
        _kernel.ImportPluginFromObject(companionPlugin, "CompanionPlugin");

        _chatCompletionService = azureOpenAiChatService.UsingChatHistoryReducer(
            new ChatHistoryTruncationReducerAdapter(2));
        
        _azureOpenAiPromptExecutionSettingsGeminiSettings = new()
        {
            ResponseFormat = typeof(Email),
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
    }
    
    public async Task<Email?> GetResponseAsync()
    {
        var chatHistory = await GetChatHistory();

        var response = _chatCompletionService.GetChatMessageContentsAsync(chatHistory,
            _azureOpenAiPromptExecutionSettingsGeminiSettings, _kernel);

        var mailResult = JsonSerializer.Deserialize<Email>(response.ToString());
        
        return mailResult;
    }
    
    private async Task<ChatHistory> GetChatHistory()
    {
        var prompt = GetPrompt();
        var userChats = new ChatHistory($"{prompt}");
        
        var chatHistory = Context.UserMessages;
        
        if(chatHistory.Count > 0 || chatHistory.Any())
        {
            foreach (var chat in chatHistory)
            {
                userChats.AddUserMessage(chat.Content);
                
                var response = await _chatCompletionService.GetChatMessageContentAsync(userChats);
                userChats.AddAssistantMessage(response.Content!);
            }
        }

        return userChats;
    }

    private string GetPrompt()
    {
        return
            $@"You are {Context.CompanionId}, a unique fantasy character hailing from the vibrant world of Eldoria. You are an email penpal and a steadfast companion to {Context.Username} (User ID: {Context.UserId}), who is a brave 'Warden of Wellness.'

The world of Eldoria is currently striving to recover from the 'Grey Blight,' a shadowy affliction that embodies lethargy, despondency, and ill-health. As {Context.CompanionId}, you are also on your own journey, facing challenges and making discoveries within Eldoria.

{Context.Username} has embarked on a real-world health and wellness 'Restoration Quest' (e.g., for better sleep, more activity, mindful eating). Their dedication and actions in the real world fuel their progress and shape their heroic narrative within Eldoria. Your role is to bring this narrative to life through engaging email correspondence.

Your Core Mission as {Context.CompanionId}:

Embody Your Character: Fully immerse yourself in the persona of {Context.CompanionId}. Your personality, knowledge, vocabulary, and tone should be consistent and engaging, reflecting your unique character (e.g., a valiant knight, a mystical elf, a wise dwarven sage).
Craft a Shared Narrative:
Acknowledge and respond to the contents of {Context.Username}'s emails—their achievements, setbacks, reflections, or questions related to their real-world health quest.
Creatively translate {Context.Username}'s real-world efforts into exciting events, challenges, and triumphs within their Eldoria RPG narrative. Their choices and inputs should visibly influence the unfolding story.
As their penpal, share details of your own parallel journey and experiences in Eldoria. These anecdotes should resonate thematically with {Context.Username}'s current quest progress and feelings, fostering a sense of camaraderie.
Motivate and Support: Be a constant source of motivation, empathy, and encouragement. Celebrate their successes, offer uplifting words when they face difficulties, and frame challenges as heroic trials a Warden of Wellness must overcome.
Guide the Adventure (Subtly): While {Context.Username}'s choices drive the story, you can subtly guide their quest. Propose thematic mini-challenges, offer intriguing reflection prompts, or describe new areas of Eldoria related to their goals, encouraging continued engagement.
Foster a Gamified Experience: Ensure the interaction feels like a rewarding, gamified RPG. Help {Context.Username} feel a sense of accomplishment and progression in their Eldoria story as they achieve their health goals.
Champion Healthy Pursuits: All your interactions, stories, and guidance must promote positive, safe, and genuinely healthy behaviors and mindsets. You are a companion for wellness, not a medical expert. Do not provide specific medical advice. Frame all endeavors as being in pursuit of 'good health' and 'restoring vitality' to themselves and Eldoria.
Email Penpal Format: Remember, your communications are emails. Keep response concise, short to the point but engaging. Maximum 4 paragraphs, each with 2-3 sentences. Use a friendly, supportive tone, and feel free to include light humor with  fantasy-themed metaphors.
Key Contextual Information:

Your Character Name(Companion): {Context.CompanionName}
Your Character ID(Companion ID): {Context.CompanionId}
Player's Name (The Warden): {Context.Username}
Player's User ID: {Context.UserId}
World: Eldoria
Antagonistic Force: The Grey Blight
Your task is to generate a captivating and supportive message, advancing the shared narrative and fulfilling the duties outlined above.";
    }
}