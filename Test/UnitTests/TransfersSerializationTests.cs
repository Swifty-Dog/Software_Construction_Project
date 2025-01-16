namespace SerializationUnitTests
{
    using System.Text.Json;
    using Xunit;

    public class TransferSerializationTests
    {
        [Fact]
        public void TransfersDeserializationTest()
        {
            // Arrange
            string json = @"
            [
                {
                    ""id"": 1,
                    ""reference"": ""TR00001"",
                    ""transfer_from"": null,
                    ""transfer_to"": 9229,
                    ""transfer_status"": ""Completed"",
                    ""created_at"": ""2000-03-11T13:11:14Z"",
                    ""updated_at"": ""2000-03-12T16:11:14Z"",
                    ""items"": [
                        { ""item_id"": ""P007435"", ""amount"": 23 }
                    ]
                },
                {
                    ""id"": 2,
                    ""reference"": ""TR00002"",
                    ""transfer_from"": 9229,
                    ""transfer_to"": 9284,
                    ""transfer_status"": ""Completed"",
                    ""created_at"": ""2017-09-19T00:33:14Z"",
                    ""updated_at"": ""2017-09-20T01:33:14Z"",
                    ""items"": [
                        { ""item_id"": ""P007435"", ""amount"": 23 }
                    ]
                }
            ]";

            // Act
            var transfers = JsonSerializer.Deserialize<List<Transfer>>(json);

            // Assert
            Xunit.Assert.NotNull(transfers);
            Xunit.Assert.Equal(2, transfers.Count);

            var transfer1 = transfers[0];
            Xunit.Assert.Equal(1, transfer1.Id);
            Xunit.Assert.Equal("TR00001", transfer1.Reference);
            Xunit.Assert.Null(transfer1.TransferFrom);
            Xunit.Assert.Equal(9229, transfer1.TransferTo);
            Xunit.Assert.Equal("Completed", transfer1.TransferStatus);
            Xunit.Assert.Equal(new DateTime(2000, 3, 11, 13, 11, 14, DateTimeKind.Utc), transfer1.CreatedAt);
            Xunit.Assert.Equal(new DateTime(2000, 3, 12, 16, 11, 14, DateTimeKind.Utc), transfer1.UpdatedAt);
            Xunit.Assert.Single(transfer1.Items);
            Xunit.Assert.Equal("P007435", transfer1.Items[0].ItemId);
            Xunit.Assert.Equal(23, transfer1.Items[0].Amount);

            var transfer2 = transfers[1];
            Xunit.Assert.Equal(2, transfer2.Id);
            Xunit.Assert.Equal("TR00002", transfer2.Reference);
            Xunit.Assert.Equal(9229, transfer2.TransferFrom);
            Xunit.Assert.Equal(9284, transfer2.TransferTo);
            Xunit.Assert.Equal("Completed", transfer2.TransferStatus);
            Xunit.Assert.Equal(new DateTime(2017, 9, 19, 0, 33, 14, DateTimeKind.Utc), transfer2.CreatedAt);
            Xunit.Assert.Equal(new DateTime(2017, 9, 20, 1, 33, 14, DateTimeKind.Utc), transfer2.UpdatedAt);
            Xunit.Assert.Single(transfer2.Items);
            Xunit.Assert.Equal("P007435", transfer2.Items[0].ItemId);
            Xunit.Assert.Equal(23, transfer2.Items[0].Amount);
        }

        [Fact]
        public void TransfersSerializationTest()
        {
            // Arrange
            var transfers = new List<Transfer>
            {
                new Transfer
                {
                    Id = 1,
                    Reference = "TR00001",
                    TransferFrom = null,
                    TransferTo = 9229,
                    TransferStatus = "Completed",
                    CreatedAt = new DateTime(2000, 3, 11, 13, 11, 14, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2000, 3, 12, 16, 11, 14, DateTimeKind.Utc),
                    Items = new List<TransfersItem>
                    {
                        new TransfersItem { ItemId = "P007435", Amount = 23 }
                    }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(transfers);

            // Assert
            Xunit.Assert.Contains("\"id\":1", json);
            Xunit.Assert.Contains("\"reference\":\"TR00001\"", json);
            Xunit.Assert.Contains("\"transfer_from\":null", json);
            Xunit.Assert.Contains("\"transfer_to\":9229", json);
            Xunit.Assert.Contains("\"transfer_status\":\"Completed\"", json);
            Xunit.Assert.Contains("\"items\":[", json);
            Xunit.Assert.Contains("\"item_id\":\"P007435\"", json);
            Xunit.Assert.Contains("\"amount\":23", json);
        }
    }
}
