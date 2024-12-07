using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class LocationsTest
{
    private readonly MyContext _context;
    private readonly LocationController _controller;

    public LocationsTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "LocationsTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new LocationServices(_context);
        _controller = new LocationController(service);
    }

    private void SeedData()
    {
        _context.Locations.RemoveRange(_context.Locations);
        _context.SaveChanges();

        var location = new Locations
        {
            Id = 1,
            WarehouseId = 50000003,
            Code = "LOC001",
            Name = "Main Storage",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Locations.Add(location);
        _context.SaveChanges();
    }

    [Fact]
    public async Task Test_Get_Locations()
    {
        var result = await _controller.GetAllLocations();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var locations = Xunit.Assert.IsType<List<Locations>>(okResult.Value);
        Xunit.Assert.NotEmpty(locations);
    }

    [Fact]
    public async Task Test_Get_Location_By_Id()
    {
        var result = await _controller.GetLocation(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var location = Xunit.Assert.IsType<Locations>(okResult.Value);
        Xunit.Assert.Equal("Main Storage", location.Name);
        Xunit.Assert.Equal(1, location.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Location()
    {
        var result = await _controller.GetLocation(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostLocation()
    {
        var newLocation = new Locations
        {
            Id = 50000004,
            WarehouseId = 50000003,
            Code = "LOC002",
            Name = "Secondary Storage",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddLocation(newLocation);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var location = Xunit.Assert.IsType<Locations>(okResult.Value);
        Xunit.Assert.Equal("Secondary Storage", location.Name);
        Xunit.Assert.Equal("LOC002", location.Code);
    }

    [Fact]
    public async Task TestPutLocation()
    {
        var updatedLocation = new Locations
        {
            Id = 1,
            WarehouseId = 50000003,
            Code = "LOC001_UPDATED",
            Name = "Main Storage Updated",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateLocation(1, updatedLocation);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var location = Xunit.Assert.IsType<Locations>(okResult.Value);
        Xunit.Assert.Equal("LOC001_UPDATED", location.Code);
        Xunit.Assert.Equal("Main Storage Updated", location.Name);
    }

    [Fact]
    public async Task TestDeleteLocation()
    {
        var result = await _controller.DeleteLocation(1);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.GetLocation(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}
