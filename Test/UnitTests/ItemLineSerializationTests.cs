namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ItemLineSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 0,
                ""name"": ""Tech Gadgets"",
                ""description"": """",
                ""created_at"": ""2022-08-18T07:05:25"",
                ""updated_at"": ""2023-05-15T15:44:28""
            }";

            // Act
            var itemLine = JsonSerializer.Deserialize<ItemGroup>(json);

            // Assert
            Xunit.Assert.NotNull(itemLine);
            Xunit.Assert.Equal(0, itemLine.Id);
            Xunit.Assert.Equal("Tech Gadgets", itemLine.Name);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var itemLine = new ItemGroup
            {
                Id = 0,
                Name = "Tech Gadgets",
                Description = "",
                CreatedAt = new DateTime(2022, 8, 18, 7, 5, 25),
                UpdatedAt = new DateTime(2023, 5, 15, 15, 44, 28)
            };

            // Act
            var json = JsonSerializer.Serialize(itemLine);

            // Assert
            Xunit.Assert.Contains("\"id\":0", json);
            Xunit.Assert.Contains("\"name\":\"Tech Gadgets\"", json);
        }
    }
}