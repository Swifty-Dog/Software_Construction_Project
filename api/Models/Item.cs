public class Item {
    public required string Uid { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string ShortDescription { get; set; }
    public required string UpcCode { get; set; }
    public required string ModelNumber { get; set; }
    public required string CommodityCode { get; set; }
    public int? ItemLine { get; set; } = null;
    public int? ItemGroup { get; set; } = null;
    public int? ItemType { get; set; } = null;
    public required int UnitPurchaseQuantity { get; set; }
    public required int UnitOrderQuantity { get; set; }
    public required int PackOrderQuantity { get; set; }
    public int? SupplierId { get; set; }
    public required string SupplierCode { get; set; }
    public required string SupplierPartNumber { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
