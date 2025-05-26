using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedPulse.Models;

public partial class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("fullname")]
    public string? FullName { get; set; }
    [JsonPropertyName("quests")]
    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
    [JsonPropertyName("trophies")]
    public virtual ICollection<Trophy> Trophies { get; set; } = new List<Trophy>();
    [JsonPropertyName("usermessages")]
    public virtual ICollection<UserMessage> UserMessages { get; set; } = new List<UserMessage>();
}
