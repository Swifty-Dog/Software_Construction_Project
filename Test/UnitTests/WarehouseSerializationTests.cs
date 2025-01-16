namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class WarehouseSerializationTests
    {
        [Fact]
        public void WarehouseDeserializationTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""code"": ""YQZZNL56"",
                ""name"": ""Heemskerk cargo hub"",
                ""address"": ""Karlijndreef 281"",
                ""zip"": ""4002 AS"",
                ""city"": ""Heemskerk"",
                ""province"": ""Friesland"",
                ""country"": ""NL"",
                ""contact"": {
                    ""id"": 3,
                    ""name"": ""Fem Keijzer"",
                    ""phone"": ""(078) 0013363"",
                    ""email"": ""blamore@example.net""
                },
                ""created_at"": ""1983-04-13T04:59:55"",
                ""updated_at"": ""2007-02-08T20:11:00""
            }";

            // Act
            var warehouse = JsonSerializer.Deserialize<Warehouse>(json);

            // Assert
            Xunit.Assert.NotNull(warehouse);
            Xunit.Assert.Equal(1, warehouse.Id);
            Xunit.Assert.Equal("YQZZNL56", warehouse.Code);
            Xunit.Assert.Equal("Heemskerk cargo hub", warehouse.Name);
            Xunit.Assert.Equal("Karlijndreef 281", warehouse.Address);
            Xunit.Assert.Equal("4002 AS", warehouse.Zip);
            Xunit.Assert.Equal("Heemskerk", warehouse.City);
            Xunit.Assert.Equal("Friesland", warehouse.Province);
            Xunit.Assert.Equal("NL", warehouse.Country);

            Xunit.Assert.NotNull(warehouse.Contact);
            Xunit.Assert.Equal(3, warehouse.Contact.Id);
            Xunit.Assert.Equal("Fem Keijzer", warehouse.Contact.Name);
            Xunit.Assert.Equal("(078) 0013363", warehouse.Contact.Phone);
            Xunit.Assert.Equal("blamore@example.net", warehouse.Contact.Email);

            Xunit.Assert.Equal(new DateTime(1983, 4, 13, 4, 59, 55, DateTimeKind.Unspecified), warehouse.CreatedAt);
            Xunit.Assert.Equal(new DateTime(2007, 2, 8, 20, 11, 0, DateTimeKind.Unspecified), warehouse.UpdatedAt);
        }

        [Fact]
        public void WarehouseSerializationTest()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Id = 1,
                Code = "YQZZNL56",
                Name = "Heemskerk cargo hub",
                Address = "Karlijndreef 281",
                Zip = "4002 AS",
                City = "Heemskerk",
                Province = "Friesland",
                Country = "NL",
                Contact = new Contact
                {
                    Id = 3,
                    Name = "Fem Keijzer",
                    Phone = "(078) 0013363",
                    Email = "blamore@example.net"
                },
                CreatedAt = new DateTime(1983, 4, 13, 4, 59, 55, DateTimeKind.Unspecified),
                UpdatedAt = new DateTime(2007, 2, 8, 20, 11, 0, DateTimeKind.Unspecified)
            };

            // Act
            string json = JsonSerializer.Serialize(warehouse, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Assert
            Xunit.Assert.Contains("\"id\": 1", json);
            Xunit.Assert.Contains("\"code\": \"YQZZNL56\"", json);
            Xunit.Assert.Contains("\"name\": \"Heemskerk cargo hub\"", json);
            Xunit.Assert.Contains("\"address\": \"Karlijndreef 281\"", json);
            Xunit.Assert.Contains("\"zip\": \"4002 AS\"", json);
            Xunit.Assert.Contains("\"city\": \"Heemskerk\"", json);
            Xunit.Assert.Contains("\"province\": \"Friesland\"", json);
            Xunit.Assert.Contains("\"country\": \"NL\"", json);

            Xunit.Assert.Contains("\"contact\": {", json);
            Xunit.Assert.Contains("\"id\": 3", json);
            Xunit.Assert.Contains("\"name\": \"Fem Keijzer\"", json);
            Xunit.Assert.Contains("\"phone\": \"(078) 0013363\"", json);
            Xunit.Assert.Contains("\"email\": \"blamore@example.net\"", json);

            Xunit.Assert.Contains("\"created_at\": \"1983-04-13T04:59:55\"", json);
            Xunit.Assert.Contains("\"updated_at\": \"2007-02-08T20:11:00\"", json);
        }
    }
}