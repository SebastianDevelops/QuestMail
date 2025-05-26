using System.ComponentModel;
using MedPulse.Models;
using MedPulse.Repositories;
using Microsoft.SemanticKernel;

namespace MedPulse.Plugins;

public class UserPlugin(IUnitOfWork unitOfWork)
{
    [KernelFunction("creates_quest")]
    [Description("Creates a new quest for the user")]
    public async Task<Quest?> CreateQuestAsync(
        string title,
        string content, string status,
        [Description("The user ID")]
        int userId, 
        [Description("The companion ID")]
        int companionId)
    {
        var quest = new Quest
        {
            Title = title,
            Content = content,
            Status = status,
            UserId = userId,
            CompanionId = companionId
        };
        var response = await unitOfWork.Quests.CreateAsync(quest);

        if (response.Quest == null)
        {
            return null;
        }
        
        return response.Quest;
    }
    
    [KernelFunction("updates_quest")]
    [Description("Updates the quests status of the user")]
    public async Task<Quest?> UpdateQuestStatusAsync(
        string status,
        [Description("The user ID")]
        int userId)
    {
        var response = await unitOfWork.Quests.UpdateAsync(userId, status);

        if (response.Quest == null)
        {
            return null;
        }
        
        return response.Quest;
    }
    
    [KernelFunction("get_quests")]
    [Description("Gets all users quest")]
    public async Task<List<Quest?>?> GetAllUserQuestsAsync(
        [Description("User ID")]
        int userId)
    {
        var response = await unitOfWork.Quests.GetAllByUserIdAsync(userId);
        
        if(response.Count <= 0)
        {
            return null;
        }
        
        return response;
    }
    
    [KernelFunction("get_quest")]
    [Description("Gets quest by ID using all users quests to obtain quest ID")]
    public async Task<Quest?> GetQuestByIdAsync(
        [Description("Quest ID")]
        int questId)
    {
        var response = await unitOfWork.Quests.GetByIdAsync(questId);

        if (response == null)
        {
            return null;
        }
        
        return response;
    }
}