using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedPulse.Models;

public partial class Trophy
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("createdat")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("imageurl")]
    public string? ImageUrl { get; set; }
    [JsonPropertyName("userid")]
    public int? UserId { get; set; }
    [JsonPropertyName("user")]
    public virtual User? User { get; set; }
}
