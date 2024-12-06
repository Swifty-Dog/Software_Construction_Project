public class InventoriesLocations
{
    [System.Text.Json.Serialization.JsonIgnore]
    public int inventoryId { get; set; }
    public required int locationId { get; set; }

        // Since you have a one-to-many relationship (one inventory can have many locations), you can keep
        // this class as is, except you may want to add a navigation property back to Inventory if needed
    // public Inventory? Inventory { get; set; }  // Navigation property (optional)
}