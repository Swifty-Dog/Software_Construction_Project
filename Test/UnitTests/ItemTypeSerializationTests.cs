namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ItemTypeSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 0,
                ""name"": ""Laptop"",
                ""description"": """",
                ""created_at"": ""2001-11-02T23:02:40"",
                ""updated_at"": ""2008-07-01T04:09:17""
            }";

            // Act
            var itemType = JsonSerializer.Deserialize<ItemType>(json);

            // Assert
            Xunit.Assert.NotNull(itemType);
            Xunit.Assert.Equal(0, itemType.Id);
            Xunit.Assert.Equal("Laptop", itemType.Name);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var itemType = new ItemType
            {
                Id = 0,
                Name = "Laptop",
                Description = "",
                CreatedAt = new DateTime(2001, 11, 2, 23, 2, 40),
                UpdatedAt = new DateTime(2008, 7, 1, 4, 9, 17)
            };

            // Act
            var json = JsonSerializer.Serialize(itemType);

            // Assert
            Xunit.Assert.Contains("\"id\":0", json);
            Xunit.Assert.Contains("\"name\":\"Laptop\"", json);
        }
    }
}