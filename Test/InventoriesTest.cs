using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class InventoriesTest
{
    private readonly MyContext _context;
    private readonly InventoriesController _controller;

    public InventoriesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "InventoriesTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new InventoriesServices(_context);
        _controller = new InventoriesController(service);
    }

    private void SeedData()
    {
        _context.Inventories.RemoveRange(_context.Inventories);
        _context.Inventories_Locations.RemoveRange(_context.Inventories_Locations);
        _context.SaveChanges();

        var inventory = new Inventory
        {
            id = 100,
            itemId = "P000001",
            description = "Face-to-face clear-thinking complexity",
            itemReference = "sjQ23408K",
            locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { locationId = 3211 },
                new InventoriesLocations { locationId = 24700 },
                new InventoriesLocations { locationId = 14123 }
            },
            totalOnHand = 262,
            totalExpected = 0,
            totalOrdered = 80,
            totalAllocated = 41,
            totalAvailable = 141,
            createdAt = DateTime.Parse("2015-02-19 16:08:24"),
            updatedAt = DateTime.Parse("2015-09-26 06:37:56")
        };

        _context.Inventories.Add(inventory);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetInventories()
    {
        var result = await _controller.GetInventories();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var inventories = Xunit.Assert.IsType<List<Inventory>>(okResult.Value);
        Xunit.Assert.NotEmpty(inventories);
    }

    [Fact]
    public async Task TestGetInventoryById()
    {
        var result = await _controller.GetInventoryById(100);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var inventory = Xunit.Assert.IsType<Inventory>(okResult.Value);
        Xunit.Assert.Equal("P000001", inventory.itemId);
        Xunit.Assert.Equal(100, inventory.id);
    }

    [Fact]
    public async Task TestGet_Non_Existent_Inventory()
    {
        var result = await _controller.GetInventoryById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestGetInventoryLocations()
    {
        var result = await _controller.GetInventoryLocations(100);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var locations = Xunit.Assert.IsType<List<InventoriesLocations>>(okResult.Value);
        Xunit.Assert.Equal(3, locations.Count);
    }

    [Fact]
    public async Task TestPostInventory()
    {
        var newInventory = new Inventory
        {
            id = 101,
            itemId = "P000002",
            description = "Focused transitional alliance",
            itemReference = "nyg48736S",
            locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { locationId = 19800 },
                new InventoriesLocations { locationId = 23653 }
            },
            totalOnHand = 194,
            totalExpected = 0,
            totalOrdered = 139,
            totalAllocated = 0,
            totalAvailable = 55,
            createdAt = DateTime.UtcNow,
            updatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddInventory(newInventory);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.Equal("Inventory and locations added successfully", okResult.Value);
    }

    [Fact]
    public async Task TestPostExistingInventory()
    {
        var existingInventory = new Inventory
        {
            id = 100,
            itemId = "P000001",
            description = "Duplicate Inventory",
            itemReference = "duplicateRef",
            locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { locationId = 12345 }
            },
            totalOnHand = 100,
            totalExpected = 0,
            totalOrdered = 50,
            totalAllocated = 25,
            totalAvailable = 25,
            createdAt = DateTime.UtcNow,
            updatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddInventory(existingInventory);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Inventory with the same id already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task TestDeleteInventory()
    {
        var result = await _controller.DeleteInventory(100);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.GetInventoryById(100);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    [Fact]
    public async Task TestDeleteNonExistentInventory()
    {
        var result = await _controller.DeleteInventory(9999);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Inventory could not be deleted or does not exist.", badRequestResult.Value);
    }
}