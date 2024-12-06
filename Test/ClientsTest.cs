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
            ContactName = "Bryan Clark",
            ContactPhone = "242.732.3483x2573",
            ContactEmail = "test@",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Client.Add(client);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetClients()
    {
        var result = await _controller.GetClients();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var clients = Xunit.Assert.IsType<List<Client>>(okResult.Value);
        Xunit.Assert.NotEmpty(clients);
    }

    [Fact]
    public async Task TestGetClientById()
    {
        var result = await _controller.GetClientById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var client = Xunit.Assert.IsType<Client>(okResult.Value);
        Xunit.Assert.Equal("Raymond Inc", client.Name);
        Xunit.Assert.Equal(1, client.Id);
    }

    [Fact]
    public async Task TestGetNonexistentClient()
    {
        var result = await _controller.GetClientById(9999);
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
            ContactName = "new name",
            ContactPhone = "06 12345678",
            ContactEmail = "test@",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await _controller.AddClient(newclient);
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
            ContactName = "new name",
            ContactPhone = "06 12345678",
            ContactEmail = "test@",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateClient(1, updatedClient);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var client = Xunit.Assert.IsType<Client>(okResult.Value);
        Xunit.Assert.Equal("changed inc", client.Name);
        Xunit.Assert.Equal("999", client.Zip);
    } 
}