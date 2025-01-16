namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ItemSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""uid"": ""P000001"",
                ""code"": ""sjQ23408K"",
                ""description"": ""Face-to-face clear-thinking complexity"",
                ""short_description"": ""must"",
                ""upc_code"": ""6523540947122"",
                ""model_number"": ""63-OFFTq0T"",
                ""commodity_code"": ""oTo304"",
                ""item_line"": 11,
                ""item_group"": 73,
                ""item_type"": 14,
                ""unit_purchase_quantity"": 47,
                ""unit_order_quantity"": 13,
                ""pack_order_quantity"": 11,
                ""supplier_id"": 34,
                ""supplier_code"": ""SUP423"",
                ""supplier_part_number"": ""E-86805-uTM"",
                ""created_at"": ""2015-02-19T16:08:24"",
                ""updated_at"": ""2015-09-26T06:37:56""
            }";

            // Act
            var item = JsonSerializer.Deserialize<Item>(json);

            // Assert
            Xunit.Assert.NotNull(item);
            Xunit.Assert.Equal("P000001", item.Uid);
            Xunit.Assert.Equal(11, item.ItemLine);
            Xunit.Assert.Equal("SUP423", item.SupplierCode);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var item = new Item
            {
                Uid = "P000001",
                Code = "sjQ23408K",
                Description = "Face-to-face clear-thinking complexity",
                ShortDescription = "must",
                UpcCode = "6523540947122",
                ModelNumber = "63-OFFTq0T",
                CommodityCode = "oTo304",
                ItemLine = 11,
                ItemGroup = 73,
                ItemType = 14,
                UnitPurchaseQuantity = 47,
                UnitOrderQuantity = 13,
                PackOrderQuantity = 11,
                SupplierId = 34,
                SupplierCode = "SUP423",
                SupplierPartNumber = "E-86805-uTM",
                CreatedAt = new DateTime(2015, 2, 19, 16, 8, 24),
                UpdatedAt = new DateTime(2015, 9, 26, 6, 37, 56)
            };

            // Act
            var json = JsonSerializer.Serialize(item);

            // Assert
            Xunit.Assert.Contains("\"uid\":\"P000001\"", json);
            Xunit.Assert.Contains("\"code\":\"sjQ23408K\"", json);
            Xunit.Assert.Contains("\"item_line\":11", json);
            Xunit.Assert.Contains("\"supplier_code\":\"SUP423\"", json);
        }
    }
}