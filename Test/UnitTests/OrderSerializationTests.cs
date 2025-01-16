namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class OrderSerializationTests
    {
        [Fact]
        public void OrdersDeserializationTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""source_id"": 33,
                ""order_date"": ""2019-04-03T11:33:15Z"",
                ""request_date"": ""2019-04-07T11:33:15Z"",
                ""reference"": ""ORD00001"",
                ""reference_extra"": ""Bedreven arm straffen bureau."",
                ""order_status"": ""Delivered"",
                ""notes"": ""Voedsel vijf vork heel."",
                ""shipping_notes"": ""Buurman betalen plaats bewolkt."",
                ""picking_notes"": ""Ademen fijn volgorde scherp aardappel op leren."",
                ""warehouse_id"": 18,
                ""ship_to"": 4562,
                ""bill_to"": 7863,
                ""shipment_id"": 1,
                ""total_amount"": 9905.13,
                ""total_discount"": 150.77,
                ""total_tax"": 372.72,
                ""total_surcharge"": 77.6,
                ""created_at"": ""2019-04-03T11:33:15Z"",
                ""updated_at"": ""2019-04-05T07:33:15Z"",
                ""items"": [
                    {
                        ""uid"": ""P007435"",
                        ""code"": ""Item001"",
                        ""description"": ""Item 1 Description"",
                        ""short_description"": ""Item 1"",
                        ""upc_code"": ""123456789012"",
                        ""model_number"": ""MN001"",
                        ""commodity_code"": ""CC001"",
                        ""item_line"": 1,
                        ""item_group"": 1
                    },
                    {
                        ""uid"": ""P009557"",
                        ""code"": ""Item002"",
                        ""description"": ""Item 2 Description"",
                        ""short_description"": ""Item 2"",
                        ""upc_code"": ""987654321098"",
                        ""model_number"": ""MN002"",
                        ""commodity_code"": ""CC002"",
                        ""item_line"": 2,
                        ""item_group"": 2
                    }
                ]
            }";

            // Act
            var orders = JsonSerializer.Deserialize<Orders>(json);

            // Assert
            Xunit.Assert.NotNull(orders);
            Xunit.Assert.Equal(1, orders.Id);
            Xunit.Assert.Equal(33, orders.SourceId);
            Xunit.Assert.Equal("ORD00001", orders.Reference);
            Xunit.Assert.Equal("Delivered", orders.OrderStatus);
            Xunit.Assert.Equal(2, orders.Items.Count);
            Xunit.Assert.Equal("P007435", orders.Items[0].Uid);
            Xunit.Assert.Equal("Item002", orders.Items[1].Code);
        }

        [Fact]
        public void OrdersSerializationTest()
        {
            // Arrange
            var orders = new Orders
            {
                Id = 1,
                SourceId = 33,
                OrderDate = DateTime.Parse("2019-04-03T11:33:15Z"),
                RequestDate = DateTime.Parse("2019-04-07T11:33:15Z"),
                Reference = "ORD00001",
                ReferenceExtra = "Bedreven arm straffen bureau.",
                OrderStatus = "Delivered",
                Notes = "Voedsel vijf vork heel.",
                ShippingNotes = "Buurman betalen plaats bewolkt.",
                PickingNotes = "Ademen fijn volgorde scherp aardappel op leren.",
                WarehouseId = 18,
                ShipTo = 4562,
                BillTo = 7863,
                ShipmentId = 1,
                TotalAmount = 9905.13m,
                TotalDiscount = 150.77m,
                TotalTax = 372.72m,
                TotalSurcharge = 77.6m,
                CreatedAt = DateTime.Parse("2019-04-03T11:33:15Z"),
                UpdatedAt = DateTime.Parse("2019-04-05T07:33:15Z"),
                Items = new List<OrdersItem>
                {
                    new OrdersItem
                    {
                        Uid = "P007435",
                        Code = "Item001",
                        Description = "Item 1 Description",
                        ShortDescription = "Item 1",
                        UpcCode = "123456789012",
                        ModelNumber = "MN001",
                        CommodityCode = "CC001",
                        ItemLine = 1,
                        ItemGroup = 1
                    },
                    new OrdersItem
                    {
                        Uid = "P009557",
                        Code = "Item002",
                        Description = "Item 2 Description",
                        ShortDescription = "Item 2",
                        UpcCode = "987654321098",
                        ModelNumber = "MN002",
                        CommodityCode = "CC002",
                        ItemLine = 2,
                        ItemGroup = 2
                    }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(orders);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"items\":[", json);
            Xunit.Assert.Contains("\"uid\":\"P007435\"", json);
            Xunit.Assert.Contains("\"uid\":\"P009557\"", json);
            Xunit.Assert.Contains("\"total_amount\":9905.13", json);
        }
    }
}