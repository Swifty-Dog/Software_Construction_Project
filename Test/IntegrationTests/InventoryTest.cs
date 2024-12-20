using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class InventoryTest
{
    private readonly MyContext _context;
    private readonly InventoryController _controller;

    public InventoryTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "InventoryTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new InventoryServices(_context);
        _controller = new InventoryController(service);
    }

    private void SeedData()
    {
        _context.Inventories.RemoveRange(_context.Inventories);
        _context.InventoriesLocations.RemoveRange(_context.InventoriesLocations);
        _context.SaveChanges();

        var inventory = new Inventory
        {
           Id = 100,
            ItemId = "P000001",
            Description = "Focused transitional alliance",
            ItemReference = "nyg48736S",
            Locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { LocationId = 19800 },
                new InventoriesLocations { LocationId = 23653 },
                new InventoriesLocations { LocationId = 43523 }
            },
            TotalOnHand = 194,
            TotalExpected = 0,
            TotalOrdered = 139,
            TotalAllocated = 0,
            TotalAvailable = 55,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
        Xunit.Assert.Equal("P000001", inventory.ItemId);
        Xunit.Assert.Equal(100, inventory.Id);
    }

    [Fact]
    public async Task TestGetNonExistentInventory()
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
            Id = 101,
            ItemId = "P000002",
            Description = "Focused transitional alliance",
            ItemReference = "nyg48736S",
            Locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { LocationId = 19800 },
                new InventoriesLocations { LocationId = 23653 }
            },
            TotalOnHand = 194,
            TotalExpected = 0,
            TotalOrdered = 139,
            TotalAllocated = 0,
            TotalAvailable = 55,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
            Id = 100,
            ItemId = "P000001",
            Description = "Duplicate Inventory",
            ItemReference = "duplicateRef",
            Locations = new List<InventoriesLocations>
            {
                new InventoriesLocations { LocationId = 12345 }
            },
            TotalOnHand = 100,
            TotalExpected = 0,
            TotalOrdered = 50,
            TotalAllocated = 25,
            TotalAvailable = 25,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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