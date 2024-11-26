using System.ComponentModel.DataAnnotations;

public class Transfer{
    public required int Id { get; set; }
    public required string Reference { get; set; }
    public required int? Transfer_from { get; set; }
    public required int Transfer_to { get; set; }
    public required string Transfer_status { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public required List<Transfers_item> Items { get; set; } = new();
    
}