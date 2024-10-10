#pragma warning disable CS8618

using System.Text.Json.Serialization;

namespace ConceptNet.Models;

public class LinkedDataEntity
{
    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("@context")]
    public string[] Context { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }
}
