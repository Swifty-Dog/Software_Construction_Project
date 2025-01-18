using System.Text.Json.Serialization;

public class Orders
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("source_id")]
    public required int SourceId { get; set; }

    [JsonPropertyName("order_date")]
    public required DateTime OrderDate { get; set; }

    [JsonPropertyName("request_date")]
    public required DateTime RequestDate { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("reference_extra")]
    public required string ReferenceExtra { get; set; }

    [JsonPropertyName("order_status")]
    public required string OrderStatus { get; set; }

    [JsonPropertyName("notes")]
    public required string Notes { get; set; }

    [JsonPropertyName("shipping_notes")]
    public required string ShippingNotes { get; set; }

    [JsonPropertyName("picking_notes")]
    public required string PickingNotes { get; set; }

    [JsonPropertyName("warehouse_id")]
    public required int WarehouseId { get; set; }

    [JsonPropertyName("ship_to")]
    public int? ShipTo { get; set; } = null;

    [JsonPropertyName("bill_to")]
    public int? BillTo { get; set; } = null;

    [JsonPropertyName("shipment_id")]
    public required int ShipmentId { get; set; }

    [JsonPropertyName("total_amount")]
    public required decimal TotalAmount { get; set; }

    [JsonPropertyName("total_discount")]
    public required decimal TotalDiscount { get; set; }

    [JsonPropertyName("total_tax")]
    public required decimal TotalTax { get; set; }

    [JsonPropertyName("total_surcharge")]
    public required decimal TotalSurcharge { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [JsonPropertyName("items")]
    public required List<OrdersItem> Items { get; set; } = new();
}
