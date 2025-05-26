using MedPulse.Plugins.Types;

namespace MedPulse.Services;

public interface ISemanticKernelService
{
    Task<Email?> GetResponseAsync();
}