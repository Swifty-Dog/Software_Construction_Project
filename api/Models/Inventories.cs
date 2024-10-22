public class Inventory {
    public required int Id { get; set; }
    public required string Item_id { get; set; }
    public required string Description { get; set; }
    public required string Item_reference { get; set; }
    public required List<Inventories_locations> Locations { get; set; }
    //public required ICollection<Inventories_locations> Locations { get; set; }
    public required int Total_on_hand { get; set; }
    public required int Total_expected { get; set; }
    public required int Total_ordered { get; set; }
    public required int Total_allocated { get; set; }
    public required int Total_available { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
}