using System.Text.Json.Serialization;
public class Transfers_item
{
    [JsonIgnore]
    public int TransferId { get; set; }
    public required string Item_Id { get; set; }
    public required int Amount { get; set; }
    
}