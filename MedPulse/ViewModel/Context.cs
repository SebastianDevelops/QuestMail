using MedPulse.Models;

namespace MedPulse.ViewModel;

public static class Context
{
    public static int UserId { get; set; }
    public static int CompanionId { get; set; }
    public static string CompanionName { get; set; }
    public static string Username { get; set; }
    
    public static string ToEmail { get; set; }
    public static List<UserMessage> UserMessages { get; set; } = new List<UserMessage>();
    
}