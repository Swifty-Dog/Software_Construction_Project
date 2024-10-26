using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class WarehousesTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public WarehousesTest()
    {
        // API key for authentication
        var apiKey = "a1b2c3d4e5";

        // Set up authenticated client
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:3000/api/v1/")
        };
        _client.DefaultRequestHeaders.Add("API_KEY", apiKey);

        // Set up unauthenticated client
        _clientFail = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:3000/api/v1/")
        };
    }

    // doesnt work untill we add authentication 
    // [Fact]
    // public async Task TestWarehouseAuthentication()
    // {
    //     // Attempt to fetch warehouses without authentication
    //     var response = await _clientFail.GetAsync("warehouses");
    //     Xunit.Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    // }

    [Fact]
    public async Task TestGetWarehouses()
    {
        // Fetch all warehouses with authentication
        var response = await _client.GetAsync("warehouses");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }
}