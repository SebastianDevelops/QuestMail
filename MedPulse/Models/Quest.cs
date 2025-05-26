using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedPulse.Models;

public partial class Quest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("content")]
    public string? Content { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("userid")]
    public int? UserId { get; set; }
    [JsonPropertyName("companionid")]
    public int? CompanionId { get; set; }
    [JsonPropertyName("companion")]
    public virtual Companion? Companion { get; set; }
    [JsonPropertyName("user")]
    public virtual User? User { get; set; }
}
