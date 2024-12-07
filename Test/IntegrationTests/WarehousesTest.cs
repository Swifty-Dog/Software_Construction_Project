using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class WarehousesTest
{
    private readonly MyContext _context;
    private readonly WarehouseController _controller;

    public WarehousesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "WarehousesTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new WarehouseServices(_context); // Initialize the service with the context
        _controller = new WarehouseController(service); // Pass the service to the controller
    }

    private void SeedData()
    {
        _context.Warehouse.RemoveRange(_context.Warehouse);
        _context.Contact.RemoveRange(_context.Contact);
        _context.Locations.RemoveRange(_context.Locations);
        _context.SaveChanges();

        var contact = new Contact
        {
            Id = 2,
            Name = "Jason",
            Phone = "(078) 0013363",
            Email = "rotterdam@ger"
        };

        var warehouse = new Warehouse
        {
            Id = 50000003,
            Code = "YQZZNL56",
            Name = "Heemskerk cargo hub",
            Address = "Karlijndreef 281",
            Zip = "4002 AS",
            City = "Heemskerk",
            Province = "Friesland",
            Country = "NL",
            Contact = contact,
            Created_at = DateTime.Parse("1992-05-15T03:21:32"),
            Updated_at = DateTime.Parse("1992-05-15T03:21:32"),
            Locations = new List<Locations>
            {
                new Locations
                {
                    Id = 1,
                    WarehouseId = 50000003,
                    Code = "LOC001",
                    Name = "Main Storage",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Locations
                {
                    Id = 2,
                    WarehouseId = 50000003,
                    Code = "LOC001",
                    Name = "Main Storage",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            }
        };

        _context.Warehouse.Add(warehouse);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetWarehouses()
    {
        var result = await _controller.GetWarehouses();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var warehouses = Xunit.Assert.IsType<List<Warehouse>>(okResult.Value);
        Xunit.Assert.NotEmpty(warehouses);
    }

    [Fact]
    public async Task TestGetWarehouseById()
    {
        var result = await _controller.GetWarehouseById(50000003);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var warehouse = Xunit.Assert.IsType<Warehouse>(okResult.Value);
        Xunit.Assert.Equal("Heemskerk cargo hub", warehouse.Name);
        Xunit.Assert.Equal(50000003, warehouse.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Warehouse()
    {
        var result = await _controller.GetWarehouseById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestGetLocationsinWarehouse()
    {
        var result = await _controller.GetWarehouseLocations(50000003);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var locations = Xunit.Assert.IsType<List<Locations>>(okResult.Value);
        Xunit.Assert.Equal(2, locations.Count);
    }

    [Fact]
    public async Task TestPostWarehouse()
    {
        var newWarehouse = new Warehouse
        {
            Id = 50000004,
            Code = "NEWWARE01",
            Name = "New Warehouse",
            Address = "New Address",
            Zip = "12345",
            City = "Test City",
            Province = "Test Province",
            Country = "Test Country",
            Contact = new Contact
            {
                Id = 24,
                Name = "Test Contact",
                Phone = "123-456-7890",
                Email = "test@test.com"
            },
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.AddWarehouse(newWarehouse);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var warehouse = Xunit.Assert.IsType<Warehouse>(okResult.Value);
        Xunit.Assert.Equal("New Warehouse", warehouse.Name);
        Xunit.Assert.Equal("NEWWARE01", warehouse.Code);
    }

    [Fact]
    public async Task TestDeleteWarehouse()
    {
        var result = await _controller.DeleteWarehouse(50000003);
        Xunit.Assert.IsType<NoContentResult>(result);

        // check that the warehouse is no longer accessible
        var getResult = await _controller.GetWarehouseById(50000003);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}