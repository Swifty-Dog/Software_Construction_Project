using System.Text.Json.Serialization;

public class ShipmentsItem
{
    [JsonIgnore]
    public int ShippingId { get; set; }

    [JsonPropertyName("item_id")]
    public required string ItemId { get; set; }

    [JsonPropertyName("amount")]
    public required int Amount { get; set; }
}
