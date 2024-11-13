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

}