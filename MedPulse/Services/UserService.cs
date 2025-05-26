using MedPulse.Models;
using MedPulse.Repositories;
using MedPulse.ViewModel;
using PostmarkDotNet.Webhooks;

namespace MedPulse.Services;

public class UserService(IUnitOfWork unitOfWork, ISemanticKernelService semanticKernelService) : IUserService
{
    private async Task <User?> GetUserByEmailAsync(PostmarkInboundWebhookMessage context)
    {
        var userResponse = await unitOfWork.Users.FindByEmailAsync(context.From);
        if (userResponse == null)
        {
            return await InitNewUserAsync(context.From, context.FromName, context.FromFull.Name);
        }
        
        return userResponse;
    }
    
    private async Task<User> InitNewUserAsync(string email, string? name = null, string? fullName = null)
    {
        // Implementation for creating a user
        var user = new User
        {
            Email = email,
            Name = name,
            FullName = fullName
        };
        var companion = new Companion()
        {
            Name = CompanionNameGenService.GenCompanionName(),
            Description = "Companion created by MedPulse",
        };

        var userResponse = await unitOfWork.Users.CreateUserAsync(user);
        var companionResponse = await unitOfWork.Companions.CreateAsync(companion);

        var quest = new Quest()
        {
            UserId = userResponse.User?.Id,
            CompanionId = companionResponse.Companion?.Id,
            Title = "Getting Started",
            Content = "Embark on a journey to your new and improved self"
        };
        await unitOfWork.Quests.CreateAsync(quest);
        
        Context.UserId = userResponse.User.Id;
        Context.CompanionId = companionResponse.Companion.Id;
        Context.Username = name;
        
        return user;
    }
    
    public async Task CreateRequestContextAsync(PostmarkInboundWebhookMessage context)
    {
        var user = await GetUserByEmailAsync(context);
        if (user == null)
        {
            throw new Exception("Failed to create user context.");
        }
        
        var quests = await unitOfWork.Quests.GetAllByUserIdAsync(user.Id);
        var quest = quests.FirstOrDefault(x => x.UserId == user.Id);
    
        var companion = await unitOfWork.Companions.GetByIdAsync(quest.CompanionId.Value);
        var chatMessages = await unitOfWork.UserMessages.GetByUserIdAsync(user.Id);
        var chatMessage = new UserMessage()
        {
            UserId = user.Id,
            Content = $@"Subject:{context.Subject}{Environment.NewLine}-----------{Environment.NewLine}{context.TextBody}",
        };
        var chatMessageResponse = await unitOfWork.UserMessages.CreateAsync(chatMessage);
        if (chatMessages.Count > 0)
        {
            chatMessages[chatMessages.Count - 1] = chatMessage;
        }
        else
        {
            chatMessages.Add(chatMessageResponse.Message!);       
        }
        Context.UserId = user.Id;
        Context.ToEmail = user.Email;
        Context.CompanionId = companion.Id;
        Context.Username = user.Name;
        Context.UserMessages = chatMessages;
        Context.CompanionName = companion.Name;
        
        var response = await semanticKernelService.GetResponseAsync();
    }
}