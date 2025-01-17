using System.Text.Json.Serialization;

public class Warehouse
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("code")]
    public required string Code { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("address")]
    public required string Address { get; set; }
    
    [JsonPropertyName("zip")]
    public required string Zip { get; set; }
    
    [JsonPropertyName("city")]
    public required string City { get; set; }
    
    [JsonPropertyName("province")]
    public required string Province { get; set; }
    
    [JsonPropertyName("country")]
    public required string Country { get; set; }
    
    [JsonPropertyName("contact")]
    public required Contact Contact { get; set; }
    
    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; set; }
    
    [JsonPropertyName("locations")]
    public ICollection<Locations> Locations { get; set; } = new List<Locations>();
}
