using System.ComponentModel.DataAnnotations;

public class Transfer
{
    public required int Id { get; set; }
    public required string Reference { get; set; }
    public required int? TransferFrom { get; set; }
    public required int TransferTo { get; set; }
    public required string TransferStatus { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required List<TransfersItem> Items { get; set; } = new();
}
