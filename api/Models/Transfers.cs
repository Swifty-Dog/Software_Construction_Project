public class Transfer{
    public required int Id { get; set; }
    public required string Reference { get; set; }
    public required string Transfer_from { get; set; }
    public required string Transfer_to { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public required Transfers_item Items {get; set; }
    
}