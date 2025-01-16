using System.Text.Json.Serialization;

public class Shipment
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("order_id")]
    public required int OrderId { get; set; }

    [JsonPropertyName("source_id")]
    public required int SourceId { get; set; }

    [JsonPropertyName("order_date")]
    public required DateOnly OrderDate { get; set; }

    [JsonPropertyName("request_date")]
    public required DateOnly RequestDate { get; set; }

    [JsonPropertyName("shipment_date")]
    public required DateOnly ShipmentDate { get; set; }

    [JsonPropertyName("shipment_type")]
    public required string ShipmentType { get; set; }

    [JsonPropertyName("shipment_status")]
    public required string ShipmentStatus { get; set; }

    [JsonPropertyName("notes")]
    public required string Notes { get; set; }

    [JsonPropertyName("carrier_code")]
    public required string CarrierCode { get; set; }

    [JsonPropertyName("carrier_description")]
    public required string CarrierDescription { get; set; }

    [JsonPropertyName("service_code")]
    public required string ServiceCode { get; set; }

    [JsonPropertyName("payment_type")]
    public required string PaymentType { get; set; }

    [JsonPropertyName("transfer_mode")]
    public required string TransferMode { get; set; }

    [JsonPropertyName("total_package_count")]
    public required int TotalPackageCount { get; set; }

    [JsonPropertyName("total_package_weight")]
    public required float TotalPackageWeight { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [JsonPropertyName("items")]
    public required List<ShipmentsItem> Items { get; set; } = new();
}
