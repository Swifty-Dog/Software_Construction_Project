using System.Text.Json.Serialization;

public class OrdersItem
{
    [JsonIgnore]
    public int OrderId { get; set; }
    public required string Uid { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string ShortDescription { get; set; }
    public required string UpcCode { get; set; }
    public required string ModelNumber { get; set; }
    public required string CommodityCode { get; set; }
    public required int ItemLine { get; set; }
    public required int ItemGroup { get; set; }
}
