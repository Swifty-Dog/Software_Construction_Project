namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class LocationsSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""warehouse_id"": 1,
                ""code"": ""A.1.0"",
                ""name"": ""Row: A, Rack: 1, Shelf: 0"",
                ""created_at"": ""1992-05-15T03:21:32"",
                ""updated_at"": ""1992-05-15T03:21:32""
            }";

            // Act
            var location = JsonSerializer.Deserialize<Locations>(json);

            // Assert
            Xunit.Assert.NotNull(location);
            Xunit.Assert.Equal(1, location.Id);
            Xunit.Assert.Equal(1, location.WarehouseId);
            Xunit.Assert.Equal("A.1.0", location.Code);
            Xunit.Assert.Equal("Row: A, Rack: 1, Shelf: 0", location.Name);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var location = new Locations
            {
                Id = 1,
                WarehouseId = 1,
                Code = "A.1.0",
                Name = "Row: A, Rack: 1, Shelf: 0",
                CreatedAt = new DateTime(1992, 5, 15, 3, 21, 32),
                UpdatedAt = new DateTime(1992, 5, 15, 3, 21, 32)
            };

            // Act
            var json = JsonSerializer.Serialize(location);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"warehouse_id\":1", json);
            Xunit.Assert.Contains("\"code\":\"A.1.0\"", json);
            Xunit.Assert.Contains("\"name\":\"Row: A, Rack: 1, Shelf: 0\"", json);
        }
    }
}