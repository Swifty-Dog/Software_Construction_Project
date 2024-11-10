using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    public async Task Test_Get_Warehouse_By_Id()
    {
        int warehouse_id = 1;
        var response = await _client.GetAsync($"warehouse/{warehouse_id}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var warehouse = JsonConvert.DeserializeObject<Warehouse>(content);

        Xunit.Assert.Equal(warehouse_id, warehouse.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Warehouse()
    {
        int warehouse_id = 1000;
        var response = await _client.GetAsync($"warehouse/{warehouse_id}");
        
        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Warehouse_Invalid_Path()
    {
        var response = await _client.GetAsync("warehouse/abc");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public async Task Test_Get_Locations_in_Warehouse()
    {
    int warehouseId = 1;
    var response = await _client.GetAsync($"warehouse/{warehouseId}/locations");
    Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var locations = JsonConvert.DeserializeObject<List<Locations>>(content); // Assuming Location is the correct type

    // Check that the list contains one or more locations
    Xunit.Assert.NotNull(locations);
    Xunit.Assert.True(locations.Count > 0);
    }

    // Does not work if there is already a warehouse with that id
    [Fact]
    public async Task TestPostWarehouse()
    {
        var initialResponse = await _client.GetAsync("warehouses");
        var initialContent = await initialResponse.Content.ReadAsStringAsync();
        var initialWarehouses = JsonConvert.DeserializeObject<List<Warehouse>>(initialContent);
        var oldLength = initialWarehouses.Count;

        var warehouseData = new
        {
            id = 50000003,
            code = "YQZZNL56",
            name = "Heemskerk cargo hub",
            address = "Karlijndreef 281",
            zip = "4002 AS",
            city = "Heemskerk",
            province = "Friesland",
            country = "NL",
            contact = new
            {
                id = 23,
                name = "Jason",
                phone = "(078) 0013363",
                email = "rotterdam@ger"
            },
            created_at = "1992-05-15T03:21:32",
            updated_at = "1992-05-15T03:21:32"
        };

        var postResponse = await _client.PostAsJsonAsync("Warehouse", warehouseData);
        Xunit.Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

        var newResponse = await _client.GetAsync("warehouses");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newWarehouses = JsonConvert.DeserializeObject<List<Warehouse>>(newContent);
        var newLength = newWarehouses.Count;

        Xunit.Assert.True(newLength > oldLength);
    }

    // cant now work if there is not a warehouse with "that" id
    [Fact]
    public async Task TestDeleteWarehouse()
    {
        int warehouseId = 50000002; 
        var deleteResponse = await _client.DeleteAsync($"Warehouse/{warehouseId}");
        Xunit.Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        
    }


}
