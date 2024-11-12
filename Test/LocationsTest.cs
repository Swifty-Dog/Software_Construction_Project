using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System.Collections.Generic;

public class LocationsTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public LocationsTest()
    {
        var apiKey = "a1b2c3d4e5";

        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5000/api/v1/")
        };
        _client.DefaultRequestHeaders.Add("Api-Key", apiKey);

        _clientFail = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5000/api/v1/")
        };
    }

    [Fact]
    public async Task Test_Get_Locations()
    {
        var response = await _client.GetAsync("locations");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }

    [Fact]
    public async Task Test_Get_Location_By_Id()
    {
        int location_id = 1;
        var response = await _client.GetAsync($"locations/{location_id}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var location = JsonConvert.DeserializeObject<Locations>(content);

        Xunit.Assert.Equal(location_id, location.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Location()
    {
        int location_id = 1000;
        var response = await _client.GetAsync($"locations/{location_id}");
        
        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Location_Invalid_Path()
    {
        var response = await _client.GetAsync("locations/abc");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task TestPostLocation()
    {
        var initialResponse = await _client.GetAsync("locations");
        var initialContent = await initialResponse.Content.ReadAsStringAsync();
        var initialLocations = JsonConvert.DeserializeObject<List<Locations>>(initialContent);
        var oldLength = initialLocations.Count;

        var locationData = new
        {
            id = 50000003,
            warehouseId = 2, 
            code = ".1.0",
            name = "Row: A, Rack: 1, Shelf: 0",
            createdAt = "1992-05-15T03:21:32",
            updatedAt = "1992-05-15T03:21:32"
        };

        var postResponse = await _client.PostAsJsonAsync("locations", locationData);

        Xunit.Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

        var newResponse = await _client.GetAsync("locations");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newLocations = JsonConvert.DeserializeObject<List<Locations>>(newContent);
        var newLength = newLocations.Count;

        Xunit.Assert.True(newLength > oldLength);
    }

    [Fact]
    public async Task TestPutLocation()
    {
        int locationId = 1;

        var locationData = new
        {
            id = locationId,
            warehouseId = 1,
            code = "SAMI TEST",
            name = "Row: A, Rack: 1, Shelf: 1",
            createdAt = "1992-05-15T03:21:32",
            updatedAt = "2024-10-19T10:00:00"
        };

        var putResponse = await _client.PutAsJsonAsync($"locations/{locationId}", locationData);
        Xunit.Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var getResponse = await _client.GetAsync($"locations/{locationId}");
        var content = await getResponse.Content.ReadAsStringAsync();
        var updatedLocation = JsonConvert.DeserializeObject<Locations>(content);

        Xunit.Assert.Equal("SAMI TEST", updatedLocation.Code);
    }

    [Fact]
    public async Task TestDeleteLocation()
    {
        int locationId = 50000003;
        var deleteResponse = await _client.DeleteAsync($"locations/{locationId}");
        Xunit.Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }
}
