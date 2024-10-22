public class Shipment{
    public required int Id { get; set; }
    public required int Order_id { get; set; }
    public required int Source_id { get; set; }
    public required DateOnly Order_date { get; set; }
    public required DateOnly Request_date { get; set; }
    public required DateOnly Shipment_date { get; set; }
    public required string Shipment_type { get; set; }
    public required string Shipment_status { get; set; }
    public required string Notes { get; set; }
    public required string Carrier_code { get; set; }
    public required string Carrier_description { get; set; }
    public required string Service_code { get; set; }
    public required string Payment_type { get; set; }
    public required string Transfer_mode { get; set; }
    public required int Total_package_count { get; set; }
    public required float Total_package_weight { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public required List<Shipments_item> Items { get; set; } = new();
}

