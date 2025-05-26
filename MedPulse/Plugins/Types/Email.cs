namespace MedPulse.Plugins.Types;

public sealed class Email
{
    public string Subject { get; set; }
    
    public Body Body { get; set; }
}

public sealed class Body
{
    public string Paragraph1 { get; set; }
    public string Paragraph2 { get; set; }
    public string Paragraph3 { get; set; }
    public string Paragraph4 { get; set; }
} 