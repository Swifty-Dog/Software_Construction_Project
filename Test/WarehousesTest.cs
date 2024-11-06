using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

public class WarehousesTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public WarehousesTest(){
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


    // doesnt work untill we add authentication 
    // [Fact]
    // public async Task TestWarehouseAuthentication()
    // {
    //     // Attempt to fetch warehouses without authentication
    //     var response = await _clientFail.GetAsync("warehouses");
    //     Xunit.Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    // }

    [Fact]
    public async Task Test_Get_Warehouses()
    {
        // Fetch all warehouses with authentication
        var response = await _client.GetAsync("warehouses");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }

    [Fact]
    public async Task Test_Get_Warehouse(){
        int warehouse_id = 1;
        var response = await _client.GetAsync($"warehouse/{warehouse_id}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var warehouse = JsonConvert.DeserializeObject<Warehouse>(content);
        Xunit.Assert.Equal(warehouse_id, warehouse.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Warehouse(){
        int warehouse_id = 1000;
        var response = await _client.GetAsync($"warehouse/{warehouse_id}");
        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Warehouse_Invalid_Path(){
        var response = await _client.GetAsync("warehouse/abc");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // [Fact]
    // public async Task Test_Get_Locations_in_Warehouse(){
    //     int warehouse_id = 1;
    //     var response = await _client.GetAsync($"warehouse/{warehouse_id}/locations");
    //     Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //     var content = await response.Content.ReadAsStringAsync();
    //     var warehouse = JsonConvert.DeserializeObject<Warehouse>(content);
    //     var warehouse_location_length = warehouse.Locations.Count;
    //     Xunit.Assert.True(warehouse_location_length > 0);
    // }

}
