using System.ComponentModel;
using MedPulse.Models;
using MedPulse.Repositories;
using Microsoft.SemanticKernel;

namespace MedPulse.Plugins;

public class TrophyPlugin(IUnitOfWork unitOfWork)
{
    [KernelFunction("creates_trophy")]
    [Description("Creates a new trophy for the user based on the quest completion")]
    public async Task<Trophy?> CreateTrophyAsync(
        string title,
        string description, string status,
        [Description("The user ID")]
        int userId)
    {
        var trophy = new Trophy
        {
            Name = title,
            Description = description,
            CreatedAt = DateTime.Now,
            UserId = userId
        };
        var response = await unitOfWork.Trophies.CreateAsync(trophy);

        if (response.Trophy == null)
        {
            return null;
        }
        
        return response.Trophy;
    }
    
    [KernelFunction("get_trophies")]
    [Description("Gets all users trophies for completed quests")]
    public async Task<List<Trophy>?> GetAllUserTrophiesAsync(
        [Description("User ID")]
        int userId)
    {
        var response = await unitOfWork.Trophies.GetAllByUserIdAsync(userId);
        
        if(response?.Count <= 0)
        {
            return null;
        }
        
        return response;
    }
    
    [KernelFunction("get_trophy")]
    [Description("Gets trophy by ID using all users trophies to obtain trophy ID")]
    public async Task<Trophy?> GetQuestByIdAsync(
        [Description("Trophy ID")]
        int questId)
    {
        var response = await unitOfWork.Trophies.GetByIdAsync(questId);

        if (response == null)
        {
            return null;
        }
        
        return response;
    }
}