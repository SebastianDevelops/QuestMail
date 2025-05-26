namespace MedPulse.Infrastructure;

public class Settings
{
    public AzureOpenAI AzureOpenAI { get; set; } = new();
    public GoogleGemini GoogleGemini { get; set; } = new();
    public Pinata Pinata { get; set; } = new();
    public Postmark Postmark { get; set; } = new();
}

public class AzureOpenAI
{
    public string Apikey { get; set; } = Environment.GetEnvironmentVariable("Settings.AzureOpenAI.apikey") ?? string.Empty;
    public string Model { get; set; } = Environment.GetEnvironmentVariable("Settings.AzureOpenAI.model") ?? string.Empty;
    public string Endpoint { get; set; } = Environment.GetEnvironmentVariable("Settings.AzureOpenAI.endpoint") ?? string.Empty;
}

public class GoogleGemini
{
    public string Apikey { get; set; } = Environment.GetEnvironmentVariable("Settings.GoogleGemini.apikey") ?? string.Empty;
    public string Model { get; set; } = Environment.GetEnvironmentVariable("Settings.GoogleGemini.model") ?? string.Empty;
}

public class Pinata
{
    public string JWT { get; set; } =Environment.GetEnvironmentVariable("Settings.Pinata.JWT") ?? string.Empty;
    public string BaseUrl { get; set; } = Environment.GetEnvironmentVariable("Settings.Pinata.BaseUrl") ?? string.Empty;
}

public class Postmark
{
    public string ApiKey { get; set; } = Environment.GetEnvironmentVariable("Settings.Postmark.apikey") ?? string.Empty;
    public string FromEmail { get; set; } = Environment.GetEnvironmentVariable("Settings.Postmark.fromEmail") ?? string.Empty;
    public string ReplyTo { get; set; } = Environment.GetEnvironmentVariable("Settings.Postmark.replyTo") ?? string.Empty;
}