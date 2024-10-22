using System.Text.Json.Serialization;

public class Shipments_item
{
    [JsonIgnore]
    public int ShippingId { get; set; }
    public required string ItemId { get; set; }
    public required int Amount { get; set; }
}