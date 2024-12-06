public class Inventory {
    public required int id { get; set; }
    public required string itemId { get; set; }
    public required string description { get; set; }
    public required string itemReference { get; set; }
    public required List<InventoriesLocations> locations { get; set; }
    public required int totalOnHand { get; set; }
    public required int totalExpected { get; set; }
    public required int totalOrdered { get; set; }
    public required int totalAllocated { get; set; }
    public required int totalAvailable { get; set; }
    public required DateTime createdAt { get; set; }
    public required DateTime updatedAt { get; set; }
}