namespace MedPulse.Services;

public interface ICompanionService
{
    public Task<string> GetCompanionImageBase64();
}