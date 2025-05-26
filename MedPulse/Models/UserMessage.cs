using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedPulse.Models;

public partial class UserMessage
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    [JsonPropertyName("userid")]
    public int? UserId { get; set; }
    [JsonPropertyName("user")]
    public virtual User? User { get; set; }
}
