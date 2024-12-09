using System.Text.Json.Serialization;
public class TransfersItem
{
    [JsonIgnore]
    public int TransferId { get; set; }
    public required string ItemId { get; set; }
    public required int Amount { get; set; }
}
