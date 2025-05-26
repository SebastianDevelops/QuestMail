namespace MedPulse.Infrastructure;

public class Settings
{
    public AzureOpenAI AzureOpenAI { get; }
    public GoogleGemini GoogleGemini { get; }
    public Pinata Pinata { get; }
    public Postmark Postmark { get; }
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