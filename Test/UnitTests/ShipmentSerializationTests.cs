namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ShipmentSerializationTests
    {
        [Fact]
        public void ShipmentsDeserializationTest()
        {
            // Arrange
            string json = @"
            [
                {
                    ""id"": 1,
                    ""order_id"": 1,
                    ""source_id"": 33,
                    ""order_date"": ""2000-03-09"",
                    ""request_date"": ""2000-03-11"",
                    ""shipment_date"": ""2000-03-13"",
                    ""shipment_type"": ""I"",
                    ""shipment_status"": ""Pending"",
                    ""notes"": ""Zee vertrouwen klas rots heet lachen oneven begrijpen."",
                    ""carrier_code"": ""DPD"",
                    ""carrier_description"": ""Dynamic Parcel Distribution"",
                    ""service_code"": ""Fastest"",
                    ""payment_type"": ""Manual"",
                    ""transfer_mode"": ""Ground"",
                    ""total_package_count"": 31,
                    ""total_package_weight"": 594.42,
                    ""created_at"": ""2000-03-10T11:11:14Z"",
                    ""updated_at"": ""2000-03-11T13:11:14Z"",
                    ""items"": [
                        { ""item_id"": ""P007435"", ""amount"": 23 },
                        { ""item_id"": ""P009557"", ""amount"": 1 }
                    ]
                }
            ]";

            // Act
            var shipments = JsonSerializer.Deserialize<List<Shipment>>(json);

            // Assert
            Xunit.Assert.NotNull(shipments);
            Xunit.Assert.Single(shipments);
            var shipment = shipments[0];

            Xunit.Assert.Equal(1, shipment.Id);
            Xunit.Assert.Equal(1, shipment.OrderId);
            Xunit.Assert.Equal(33, shipment.SourceId);
            Xunit.Assert.Equal(new DateOnly(2000, 3, 9), shipment.OrderDate);
            Xunit.Assert.Equal(new DateOnly(2000, 3, 11), shipment.RequestDate);
            Xunit.Assert.Equal(new DateOnly(2000, 3, 13), shipment.ShipmentDate);
            Xunit.Assert.Equal("I", shipment.ShipmentType);
            Xunit.Assert.Equal("Pending", shipment.ShipmentStatus);
            Xunit.Assert.Equal("Zee vertrouwen klas rots heet lachen oneven begrijpen.", shipment.Notes);
            Xunit.Assert.Equal("DPD", shipment.CarrierCode);
            Xunit.Assert.Equal("Dynamic Parcel Distribution", shipment.CarrierDescription);
            Xunit.Assert.Equal("Fastest", shipment.ServiceCode);
            Xunit.Assert.Equal("Manual", shipment.PaymentType);
            Xunit.Assert.Equal("Ground", shipment.TransferMode);
            Xunit.Assert.Equal(31, shipment.TotalPackageCount);
            Xunit.Assert.Equal(594.42f, shipment.TotalPackageWeight);
            Xunit.Assert.Equal(new DateTime(2000, 3, 10, 11, 11, 14, DateTimeKind.Utc), shipment.CreatedAt);
            Xunit.Assert.Equal(new DateTime(2000, 3, 11, 13, 11, 14, DateTimeKind.Utc), shipment.UpdatedAt);
            Xunit.Assert.Equal(2, shipment.Items.Count);
            Xunit.Assert.Equal("P007435", shipment.Items[0].ItemId);
            Xunit.Assert.Equal(23, shipment.Items[0].Amount);
            Xunit.Assert.Equal("P009557", shipment.Items[1].ItemId);
            Xunit.Assert.Equal(1, shipment.Items[1].Amount);
        }

        [Fact]
        public void ShipmentsSerializationTest()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment
                {
                    Id = 1,
                    OrderId = 1,
                    SourceId = 33,
                    OrderDate = new DateOnly(2000, 3, 9),
                    RequestDate = new DateOnly(2000, 3, 11),
                    ShipmentDate = new DateOnly(2000, 3, 13),
                    ShipmentType = "I",
                    ShipmentStatus = "Pending",
                    Notes = "Zee vertrouwen klas rots heet lachen oneven begrijpen.",
                    CarrierCode = "DPD",
                    CarrierDescription = "Dynamic Parcel Distribution",
                    ServiceCode = "Fastest",
                    PaymentType = "Manual",
                    TransferMode = "Ground",
                    TotalPackageCount = 31,
                    TotalPackageWeight = 594.42f,
                    CreatedAt = new DateTime(2000, 3, 10, 11, 11, 14, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2000, 3, 11, 13, 11, 14, DateTimeKind.Utc),
                    Items = new List<ShipmentsItem>
                    {
                        new ShipmentsItem { ItemId = "P007435", Amount = 23 },
                        new ShipmentsItem { ItemId = "P009557", Amount = 1 }
                    }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(shipments);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"order_id\":1", json);
            Xunit.Assert.Contains("\"items\":[", json);
            Xunit.Assert.Contains("\"item_id\":\"P007435\"", json);
            Xunit.Assert.Contains("\"amount\":23", json);
        }
    }
}
