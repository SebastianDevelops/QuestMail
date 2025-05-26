using System.ComponentModel;
using MedPulse.Models;
using MedPulse.Repositories;
using Microsoft.SemanticKernel;

namespace MedPulse.Plugins;

public class CompanionPlugin(IUnitOfWork unitOfWork)
{
    [KernelFunction("updates_companion_description")]
    [Description(@"Updates your(companion) description which describes your (fictional)experiences, 
                 challenges, and discoveries")]
    public async Task<Companion?> UpdateCompanionDescriptionAsync(
       string description,
        [Description("The companion ID")]
        int companionId)
    {
        var response = await unitOfWork.Companions.UpdateDescriptionAsync(companionId, description);

        if (response.companion == null)
        {
            return null;
        }
        
        return response.companion;
    }
    
    [KernelFunction("get_companion")]
    [Description("Gets companion/yourself")]
    public async Task<Companion> GetCompanionAsync(
        [Description("Companion ID")]
        int companionId)
    {
        var response = await unitOfWork.Companions.GetByIdAsync(companionId);
        
        return response;
    }
}