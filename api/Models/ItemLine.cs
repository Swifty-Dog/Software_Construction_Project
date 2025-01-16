using System.Text.Json.Serialization;

public class ItemLine
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; set; }
}
