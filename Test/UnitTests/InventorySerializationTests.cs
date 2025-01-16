namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class InventorySerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""item_id"": ""P000001"",
                ""description"": ""Face-to-face clear-thinking complexity"",
                ""item_reference"": ""sjQ23408K"",
                ""locations"": [
                    3211,
                    24700,
                    14123,
                    19538,
                    31071,
                    24701,
                    11606,
                    11817
                ],
                ""total_on_hand"": 262,
                ""total_expected"": 0,
                ""total_ordered"": 80,
                ""total_allocated"": 41,
                ""total_available"": 141,
                ""created_at"": ""2015-02-19T16:08:24"",
                ""updated_at"": ""2015-09-26T06:37:56""
            }";

            // Act
            var inventory = JsonSerializer.Deserialize<Inventory>(json);

            // Assert
            Xunit.Assert.NotNull(inventory);
            Xunit.Assert.Equal(1, inventory.Id);
            Xunit.Assert.Equal("P000001", inventory.ItemId);
            Xunit.Assert.Equal(8, inventory.Locations.Count);
            Xunit.Assert.Contains(3211, inventory.Locations);
            Xunit.Assert.Equal(262, inventory.TotalOnHand);
            Xunit.Assert.Equal(new DateTime(2015, 2, 19, 16, 8, 24), inventory.CreatedAt);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var inventory = new Inventory
            {
                Id = 1,
                ItemId = "P000001",
                Description = "Face-to-face clear-thinking complexity",
                ItemReference = "sjQ23408K",
                Locations = new List<int> { 3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817 },
                TotalOnHand = 262,
                TotalExpected = 0,
                TotalOrdered = 80,
                TotalAllocated = 41,
                TotalAvailable = 141,
                CreatedAt = new DateTime(2015, 2, 19, 16, 8, 24),
                UpdatedAt = new DateTime(2015, 9, 26, 6, 37, 56)
            };

            // Act
            var json = JsonSerializer.Serialize(inventory);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"item_id\":\"P000001\"", json);
            Xunit.Assert.Contains("\"locations\":[3211,24700,14123,19538,31071,24701,11606,11817]", json);
            Xunit.Assert.Contains("\"total_on_hand\":262", json);
            Xunit.Assert.Contains("\"created_at\":\"2015-02-19T16:08:24\"", json);
        }
    }
}
