using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

public class TransfersTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public TransfersTest()
    {
        // API key for authentication
        var apiKey = "a1b2c3d4e5";
        
        // Set up authenticated client
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5000/api/v1/")
        };
        _client.DefaultRequestHeaders.Add("Api-Key", apiKey);

        // Set up unauthenticated client
        _clientFail = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5000/api/v1/")
        };
    }

    [Fact]
    public async Task Test_Transfer_Authentication()
    {
        // Attempt to fetch transfers without authentication
        var response = await _clientFail.GetAsync("transfers");
        Xunit.Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Transfers()
    {
        // Fetch all transfers with authentication
        var response = await _client.GetAsync("transfers");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }

    [Fact]
    public async Task Test_Get_Transfer_By_Id()
    {
        int transfer_id = 1;
        var response = await _client.GetAsync($"transfer/{transfer_id}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var transfer = JsonConvert.DeserializeObject<Transfer>(content);
        Xunit.Assert.Equal(transfer_id, transfer.Id);  
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Transfer()
    {
        int transfer_id = 999;
        var response = await _client.GetAsync($"transfer/{transfer_id}");
        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Transfer_Invalid_Path()
    {
        var response = await _client.GetAsync("transfer/invalid");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Test_Post_Transfer()
    {
        var initialResponse = await _client.GetAsync("transfers");
        var initialContent = await initialResponse.Content.ReadAsStringAsync(); 
        var initialTransfers = JsonConvert.DeserializeObject<List<Transfer>>(initialContent);
        int oldLength = initialTransfers.Count;
        int newTransferId = oldLength + 1;
        var transferData = new
        {
            id = newTransferId,
            reference = "TR12345",
            transfer_from = 2323,
            transfer_to = 9299,
            transfer_status = "pending",
            created_at = "2021-09-01T00:00:00",
            updated_at = "2021-10-01T00:00:00",
            items = new List<Transfers_item>{
                new Transfers_item
                {
                    Item_Id = "P007435",
                    Amount = 2
                },
                new Transfers_item
                {
                    Item_Id = "P007436",
                    Amount = 3
                },
            }
        };
        var postResponse = await _client.PostAsJsonAsync("transfers", transferData);
        Xunit.Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

        var newResponse = await _client.GetAsync("transfers");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newTransfers = JsonConvert.DeserializeObject<List<Transfer>>(newContent);
        int newLength = newTransfers.Count;
        Xunit.Assert.True(newLength > oldLength);
    }

    [Fact]
    public async Task Test_Delete_Transfer()
    {
        var initialResponse = await _client.GetAsync("transfers");
        var initialContent = await initialResponse.Content.ReadAsStringAsync();
        var initialTransfers = JsonConvert.DeserializeObject<List<Transfer>>(initialContent);
        int oldLength = initialTransfers.Count;
        int transfer_id = 1;
        var deleteResponse = await _client.DeleteAsync($"transfer/{transfer_id}");
        Xunit.Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var newResponse = await _client.GetAsync("transfers");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newTransfers = JsonConvert.DeserializeObject<List<Transfer>>(newContent);
        int newLength = newTransfers.Count;
        Xunit.Assert.True(newLength < oldLength);
    }

}