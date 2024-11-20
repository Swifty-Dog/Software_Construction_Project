using System;
using System.Collections.Generic;

public class Orders
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string ReferenceExtra { get; set; }
    public required string OrderStatus { get; set; }
    public required string Notes { get; set; }
    public required string ShippingNotes { get; set; }
    public required string PickingNotes { get; set; }
    public required int WarehouseId { get; set; }
    public required int ShipTo { get; set; }
    public required int BillTo { get; set; }
    public required int ShipmentId { get; set; }
    public required decimal TotalAmount { get; set; }
    public required decimal TotalDiscount { get; set; }
    public required decimal TotalTax { get; set; }
    public required decimal TotalSurcharge { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required List<Orders_Item> Items { get; set; }

}

