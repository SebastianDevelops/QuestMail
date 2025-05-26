namespace MedPulse.Services;

public interface IPostmarkService
{
    Task SendEmailAsync();
}