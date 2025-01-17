namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class ClientSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""name"": ""Raymond Inc"",
                ""address"": ""1296 Daniel Road Apt. 349"",
                ""city"": ""Pierceview"",
                ""zip_code"": ""28301"",
                ""province"": ""Colorado"",
                ""country"": ""United States"",
                ""contact_name"": ""Bryan Clark"",
                ""contact_phone"": ""242.732.3483x2573"",
                ""contact_email"": ""robertcharles@example.net"",
                ""created_at"": ""2010-04-28T02:22:53"",
                ""updated_at"": ""2022-02-09T20:22:35""
            }";

            // Act
            var client = JsonSerializer.Deserialize<Client>(json);

            // Assert
            Xunit.Assert.NotNull(client);
            Xunit.Assert.Equal(1, client.Id);
            Xunit.Assert.Equal("Raymond Inc", client.Name);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var client = new Client
            {
                Id = 1,
                Name = "Raymond Inc",
                Address = "1296 Daniel Road Apt. 349",
                City = "Pierceview",
                Zip = "28301",
                Province = "Colorado",
                Country = "United States",
                ContactName = "Bryan Clark",
                ContactPhone = "242.732.3483x2573",
                ContactEmail = "robertcharles@example.net",
                CreatedAt = new DateTime(2010, 4, 28, 2, 22, 53),
                UpdatedAt = new DateTime(2022, 2, 9, 20, 22, 35)
            };

            // Act
            var json = JsonSerializer.Serialize(client);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"name\":\"Raymond Inc\"", json);
            Xunit.Assert.Contains("\"zip_code\":\"28301\"", json);
        }
    }
}