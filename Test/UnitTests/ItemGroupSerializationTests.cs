namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ItemGroupSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""name"": ""Furniture"",
                ""description"": """",
                ""created_at"": ""2019-09-22T15:51:07"",
                ""updated_at"": ""2022-05-18T13:49:28""
            }";

            // Act
            var itemGroup = JsonSerializer.Deserialize<ItemGroup>(json);

            // Assert
            Xunit.Assert.NotNull(itemGroup);
            Xunit.Assert.Equal(1, itemGroup.Id);
            Xunit.Assert.Equal("Furniture", itemGroup.Name);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var itemGroup = new ItemGroup
            {
                Id = 1,
                Name = "Furniture",
                Description = "",
                CreatedAt = new DateTime(2019, 9, 22, 15, 51, 7),
                UpdatedAt = new DateTime(2022, 5, 18, 13, 49, 28)
            };

            // Act
            var json = JsonSerializer.Serialize(itemGroup);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"name\":\"Furniture\"", json);
        }
    }
}