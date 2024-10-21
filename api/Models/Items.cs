public class Item {
    public required string Uid { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string Short_Description { get; set; }
    public required string Upc_code { get; set; }
    public required string Model_number { get; set; }
    public required string Commodity_code { get; set; }
    public required Item_line Item_line { get; set; }  
    public required Item_group Item_group { get; set; }
    public required Item_type item_type { get; set; }
    public required string unit_purchase_quantity { get; set; }
    public required string unit_order_quantity { get; set; }
    public required string pack_order_quantity { get; set; }
    public required Supplier supplier_id { get; set; }
    public required Supplier supplier_code { get; set; }
    public required Supplier supplier_part_number { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
}