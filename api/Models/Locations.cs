public class Locations
{
    public required int Id { get; set; }
    public required int WarehouseId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

    //public virtual Warehouses Warehouse { get; set; }
}