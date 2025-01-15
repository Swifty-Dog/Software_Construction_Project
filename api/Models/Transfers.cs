using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Transfer
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("transfer_from")]
    public required int? TransferFrom { get; set; }

    [JsonPropertyName("transfer_to")]
    public required int TransferTo { get; set; }

    [JsonPropertyName("transfer_status")]
    public required string TransferStatus { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [JsonPropertyName("items")]
    public required List<TransfersItem> Items { get; set; } = new List<TransfersItem>();
}
