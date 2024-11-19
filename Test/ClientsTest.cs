using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System.Collections.Generic;

public class ClientsTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public ClientsTest()
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

    // [Fact]
    // public async Task TestClientAuthentication()
    // {

    // }

    [Fact]
    public async Task TestGetClients()
    {
        var response = await _client.GetAsync("Clients");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }

    [Fact]
    public async Task TestGetClientById()
    {
        int clientId = 1;
        var response = await _client.GetAsync($"Client/{clientId}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var client = JsonConvert.DeserializeObject<Client>(content);

        Xunit.Assert.Equal(clientId, client.Id);
    }

    [Fact]
    public async Task TestGetNonexistentClient()
    {
        int clientId = 1000000;
        var response = await _client.GetAsync($"Client/{clientId}");

        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TestGetClientInvalidPath()
    {
        var response = await _client.GetAsync("Client/abc");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task TestPostClient() // nog bezig
    {
        var initialResponse = await _client.GetAsync("Client");
        var initialContent = await initialResponse.Content.ReadAsStringAsync();
        var initialClient = JsonConvert.DeserializeObject<List<Client>>(initialContent);
        var oldLength = initialClient.Count;

        var clientData = new
        {
            id = 123456789,
            name = "Raymond Inc",
            address = "1296 Daniel Road Apt. 349",
            city = "Pierceview",
            zip_code = "28301",
            province = "Colorado",
            country = "United States",
            contact_name = "Bryan Clark",
            contact_phone = "242.732.3483x2573",
            contact_email = "test@",
            created_at = "2010-04-28 02:22:53",
            updated_at = "2022-02-09 20:22:35"
        };

        var postResponse = await _client.PostAsJsonAsync("Client", clientData);

        Xunit.Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

        var newResponse = await _client.GetAsync("Client");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newClient = JsonConvert.DeserializeObject<List<Client>>(newContent);
        var newLength = newClient.Count;

        Xunit.Assert.True(newLength > oldLength);
    }
/*
    def test_get_empty_client_by_id(self):
        response = self.client.get('clients/500000000')
        client = response.json()
        self.assertIsNone(client)
        
    def test_post_client_success_with_length(self):
        response = self.client.get('clients')
        old_client_length = len(response.json())
        response = self.client.post('clients', json={
            "id": 1,
            "name": "Raymond Inc",
            "address": "1296 Daniel Road Apt. 349",
            "city": "Pierceview",
            "zip_code": "28301",
            "province": "Colorado",
            "country": "United States",
            "contact_name": "Bryan Clark",
            "contact_phone": "242.732.3483x2573",
            "contact_email": "test@",
            "created_at": "2010-04-28 02:22:53",
            "updated_at": "2022-02-09 20:22:35"
        })
        self.assertEqual(response.status_code, 201)
        response = self.client.get('clients')
        new_client_length = len(response.json())
        self.assertTrue(new_client_length > old_client_length)
*/
}