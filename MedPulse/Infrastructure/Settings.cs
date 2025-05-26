namespace MedPulse.Infrastructure;

public class Settings
{
    public AzureOpenAI AzureOpenAI { get; set; } = null!;
    public GoogleGemini GoogleGemini { get; set; } = null!;
    public Pinata Pinata { get; set; } = null!;
    public Postmark Postmark { get; set; } = null!;
}

public class AzureOpenAI
{
    public string Apikey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Endpoint { get; set; } = String.Empty;
}

public class GoogleGemini
{
    public string Apikey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}

public class Pinata
{
    public string JWT { get; set; } = String.Empty;
    public string BaseUrl { get; set; } = String.Empty;
}

public class Postmark
{
    public string ApiKey { get; set; } = String.Empty;
    public string FromEmail { get; set; } = String.Empty;
    public string ReplyTo { get; set; }
}