using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System.Collections.Generic;

public class SuppliersTest
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientFail;

    public SuppliersTest()
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
    public async Task Test_Get_All_Suppliers()
    {
        var response = await _client.GetAsync("suppliers");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Xunit.Assert.True(!string.IsNullOrEmpty(content), "Response content should not be empty");
    }

    [Fact]
    public async Task Test_Get_Supplier_By_Id()
    {
        int supplierId = 1;
        var response = await _client.GetAsync($"suppliers/{supplierId}");
        Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var supplier = JsonConvert.DeserializeObject<Supplier>(content);

        Xunit.Assert.Equal(supplierId, supplier.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Supplier()
    {
        int supplierId = 1000;
        var response = await _client.GetAsync($"suppliers/{supplierId}");
        
        Xunit.Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Test_Get_Supplier_Invalid_Path()
    {
        var response = await _client.GetAsync("suppliers/abc");
        Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task TestPostSupplier()
    {
        var initialResponse = await _client.GetAsync("suppliers");
        var initialContent = await initialResponse.Content.ReadAsStringAsync();
        var initialSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(initialContent);
        var oldLength = initialSuppliers.Count;

        var supplierData = new
        {
            id = 50000003,
            code = "SUPP003",
            name = "Tech Supplies",
            address = "123 Tech Park",
            address_extra = "Suite 101",
            zip_code = "67890",
            province = "California",
            country = "USA",
            contact_name = "Jane Doe",
            phonenumber = "098-765-4321",
            email = "janedoe@techsupplies.com",
            reference = "REF1234",
            created_at = "2024-10-22T10:00:00",
            updated_at = "2024-10-22T10:00:00"
        };

        var postResponse = await _client.PostAsJsonAsync("suppliers", supplierData);

        Xunit.Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var newResponse = await _client.GetAsync("suppliers");
        var newContent = await newResponse.Content.ReadAsStringAsync();
        var newSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(newContent);
        var newLength = newSuppliers.Count;

        Xunit.Assert.True(newLength > oldLength);
    }

    [Fact]
    public async Task TestPutSupplier()
    {
        int supplierId = 1;

        var supplierData = new
        {
            id = supplierId,
            code = "SUPP001_UPDATED",
            name = "Tech Supplies Updated",
            address = "456 Tech Lane",
            address_extra = "Apt 101",
            zip_code = "54321",
            province = "California",
            country = "USA",
            contact_name = "John Doe Updated",
            phonenumber = "555-6789",
            email = "johndoe.updated@techsupplies.com",
            reference = "REF1234_UPDATED",
            created_at = "2024-01-01T10:00:00",
            updated_at = "2024-10-22T12:00:00"
        };

        var putResponse = await _client.PutAsJsonAsync($"suppliers/{supplierId}", supplierData);
        Xunit.Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var getResponse = await _client.GetAsync($"suppliers/{supplierId}");
        var content = await getResponse.Content.ReadAsStringAsync();
        var updatedSupplier = JsonConvert.DeserializeObject<Supplier>(content);

        Xunit.Assert.Equal("SUPP001_UPDATED", updatedSupplier.Code);
    }

    [Fact]
    public async Task TestDeleteSupplier()
    {
        int supplierId = 50000003;
        var deleteResponse = await _client.DeleteAsync($"suppliers/{supplierId}");
        Xunit.Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}

