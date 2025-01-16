using System.Text.Json.Serialization;

public class OrdersItem
{
    [JsonIgnore]
    public int OrderId { get; set; }

    [JsonPropertyName("uid")]
    public required string Uid { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("short_description")]
    public required string ShortDescription { get; set; }

    [JsonPropertyName("upc_code")]
    public required string UpcCode { get; set; }

    [JsonPropertyName("model_number")]
    public required string ModelNumber { get; set; }

    [JsonPropertyName("commodity_code")]
    public required string CommodityCode { get; set; }

    [JsonPropertyName("item_line")]
    public required int ItemLine { get; set; }

    [JsonPropertyName("item_group")]
    public required int ItemGroup { get; set; }
}
