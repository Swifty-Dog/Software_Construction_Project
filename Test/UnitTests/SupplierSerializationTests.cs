namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class SupplierSerializationTests
    {
        [Fact]
        public void DeserializeJsonTest()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""code"": ""SUP0001"",
                ""name"": ""Lee, Parks and Johnson"",
                ""address"": ""5989 Sullivan Drives"",
                ""address_extra"": ""Apt. 996"",
                ""city"": ""Port Anitaburgh"",
                ""zip_code"": ""91688"",
                ""province"": ""Illinois"",
                ""country"": ""Czech Republic"",
                ""contact_name"": ""Toni Barnett"",
                ""phonenumber"": ""363.541.7282x36825"",
                ""reference"": ""LPaJ-SUP0001"",
                ""created_at"": ""1971-10-20T18:06:17"",
                ""updated_at"": ""1985-06-08T00:13:46""
            }";

            // Act
            var supplier = JsonSerializer.Deserialize<Supplier>(json);

            // Assert
            Xunit.Assert.NotNull(supplier);
            Xunit.Assert.Equal(1, supplier.Id);
            Xunit.Assert.Equal("SUP0001", supplier.Code);
            Xunit.Assert.Equal("Lee, Parks and Johnson", supplier.Name);
            Xunit.Assert.Equal("5989 Sullivan Drives", supplier.Address);
            Xunit.Assert.Equal("Toni Barnett", supplier.ContactName);
        }

        [Fact]
        public void SerializeJsonTest()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = 1,
                Code = "SUP0001",
                Name = "Lee, Parks and Johnson",
                Address = "5989 Sullivan Drives",
                AddressExtra = "Apt. 996",
                City = "Port Anitaburgh",
                ZipCode = "91688",
                Province = "Illinois",
                Country = "Czech Republic",
                ContactName = "Toni Barnett",
                Phonenumber = "363.541.7282x36825",
                Reference = "LPaJ-SUP0001",
                CreatedAt = new DateTime(1971, 10, 20, 18, 6, 17),
                UpdatedAt = new DateTime(1985, 6, 8, 0, 13, 46)
            };

            // Act
            var json = JsonSerializer.Serialize(supplier);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"code\":\"SUP0001\"", json);
            Xunit.Assert.Contains("\"name\":\"Lee, Parks and Johnson\"", json);
            Xunit.Assert.Contains("\"contact_name\":\"Toni Barnett\"", json);
        }
    }
}
