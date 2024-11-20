using System.Text.Json.Serialization;

public class Orders_Item{
    [JsonIgnore]
    public int OrderId { get; set; }
    public required string uid { get; set; }
    public required string code { get; set; }
    public required string description { get; set; }
    public required string short_description { get; set; }
    public required string upc_code { get; set; }
    public required string model_number { get; set; }
    public required string commodity_code { get; set; }
    public required int item_line { get; set; }
    public required int item_group { get; set; }
}
