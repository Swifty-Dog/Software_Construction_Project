using System.Text.Json.Serialization;

public class ShipmentsItem
{
    [JsonIgnore]
    public int ShippingId { get; set; }
    public required string ItemId { get; set; }
    public required int Amount { get; set; }
}
