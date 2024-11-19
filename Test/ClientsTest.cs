using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
public class ClientsTest
{
    private readonly MyContext _context;
    private readonly ClientController _controller;

    public ClientsTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ClientTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ClientServices(_context);
        _controller = new ClientController(service);
    }
    private void SeedData()
    {
        _context.Client.RemoveRange(_context.Client);
        _context.SaveChanges();

        var client = new Client
        {
            Id = 1,
            Name = "Raymond Inc",
            Address = "1296 Daniel Road Apt. 349",
            City = "Pierceview",
            Zip = "28301",
            Province = "Colorado",
            Country = "United States",
            Contact_name = "Bryan Clark",
            Contact_phone = "242.732.3483x2573",
            Contact_email = "test@",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        _context.Client.Add(client);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetClients()
    {
        var result = await _controller.Get_Clients();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var clients = Xunit.Assert.IsType<List<Client>>(okResult.Value);
        Xunit.Assert.NotEmpty(clients);
    }

    [Fact]
    public async Task TestGetClientById()
    {
        var result = await _controller.Get_Client_By_Id(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var client = Xunit.Assert.IsType<Client>(okResult.Value);
        Xunit.Assert.Equal("Raymond Inc", client.Name);
        Xunit.Assert.Equal(1, client.Id);
    }

    [Fact]
    public async Task TestGetNonexistentClient()
    {
        var result = await _controller.Get_Client_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }
    
     [Fact]
     public async Task TestPostClient()
     {
        var newclient = new Client
        {
            Id = 123,
            Name = "new inc",
            Address = "new addres",
            City = "new city",
            Zip = "111",
            Province = "new province",
            Country = "new country",
            Contact_name = "new name",
            Contact_phone = "06 12345678",
            Contact_email = "test@",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        var result = await _controller.Add_Client(newclient);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var client = Xunit.Assert.IsType<Client>(okResult.Value);
        Xunit.Assert.Equal(123, client.Id);
        Xunit.Assert.Equal("new inc", client.Name);
     }
    
    [Fact]
    public async Task TestPutClient()
    {
        var updatedClient = new Client
        {
            Id = 1,
            Name = "changed inc",
            Address = "changed addres",
            City = "changed city",
            Zip = "999",
            Province = "changed province",
            Country = "changed country",
            Contact_name = "new name",
            Contact_phone = "06 12345678",
            Contact_email = "test@",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.Update_Client(1, updatedClient);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var client = Xunit.Assert.IsType<Client>(okResult.Value);
        Xunit.Assert.Equal("changed inc", client.Name);
        Xunit.Assert.Equal("999", client.Zip);
    } 
}