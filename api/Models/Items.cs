public class Item {
    public required string Uid { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string Short_Description { get; set; }
    public required string Upc_code { get; set; }
    public required string Model_number { get; set; }
    public required string Commodity_code { get; set; }
    public required int Item_line { get; set; }  
    public required int Item_group { get; set; }
    public required int item_type { get; set; }
    public required int unit_purchase_quantity { get; set; }
    public required int unit_order_quantity { get; set; }
    public required int pack_order_quantity { get; set; }
    public required int supplier_id { get; set; }
    public required string supplier_code { get; set; }
    public required string supplier_part_number { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
}
