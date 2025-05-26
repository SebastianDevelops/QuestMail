using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedPulse.Models;

public partial class Companion
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    [JsonPropertyName("quests")]
    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
}
