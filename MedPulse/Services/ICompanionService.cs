namespace MedPulse.Services;

public interface ICompanionService
{
    public Task<string> GetOrSetCompanionImageUrlResponseAsync();
}