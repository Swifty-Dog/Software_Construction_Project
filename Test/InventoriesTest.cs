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
            Id = 100,
            Item_id = "P000001",
            Description = "Face-to-face clear-thinking complexity",
            Item_reference = "sjQ23408K",
            Locations = new List<Inventories_locations>
            {
                new Inventories_locations { LocationId = 3211 },
                new Inventories_locations { LocationId = 24700 },
                new Inventories_locations { LocationId = 14123 }
            },
            Total_on_hand = 262,
            Total_expected = 0,
            Total_ordered = 80,
            Total_allocated = 41,
            Total_available = 141,
            Created_at = DateTime.Parse("2015-02-19 16:08:24"),
            Updated_at = DateTime.Parse("2015-09-26 06:37:56")
        };

        _context.Inventories.Add(inventory);
        _context.SaveChanges();
    }

    [Fact]
    public async Task Test_Get_Inventories()
    {
        var result = await _controller.Get_Inventories();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var inventories = Xunit.Assert.IsType<List<Inventory>>(okResult.Value);
        Xunit.Assert.NotEmpty(inventories);
    }

    [Fact]
    public async Task Test_Get_Inventory_By_Id()
    {
        var result = await _controller.Get_Inventory_By_Id(100);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var inventory = Xunit.Assert.IsType<Inventory>(okResult.Value);
        Xunit.Assert.Equal("P000001", inventory.Item_id);
        Xunit.Assert.Equal(100, inventory.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Inventory()
    {
        var result = await _controller.Get_Inventory_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Test_Get_Inventory_Locations()
    {
        var result = await _controller.Get_Inventory_Locations(100);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var locations = Xunit.Assert.IsType<List<Inventories_locations>>(okResult.Value);
        Xunit.Assert.Equal(3, locations.Count);
    }

    [Fact]
    public async Task TestPostInventory()
    {
        var newInventory = new Inventory
        {
            Id = 101,
            Item_id = "P000002",
            Description = "Focused transitional alliance",
            Item_reference = "nyg48736S",
            Locations = new List<Inventories_locations>
            {
                new Inventories_locations { LocationId = 19800 },
                new Inventories_locations { LocationId = 23653 }
            },
            Total_on_hand = 194,
            Total_expected = 0,
            Total_ordered = 139,
            Total_allocated = 0,
            Total_available = 55,
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
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
            Item_id = "P000001",
            Description = "Duplicate Inventory",
            Item_reference = "duplicateRef",
            Locations = new List<Inventories_locations>
            {
                new Inventories_locations { LocationId = 12345 }
            },
            Total_on_hand = 100,
            Total_expected = 0,
            Total_ordered = 50,
            Total_allocated = 25,
            Total_available = 25,
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.AddInventory(existingInventory);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Inventory with the same id already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task TestDeleteInventory()
    {
        var result = await _controller.Delete_Inventory(100);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.Get_Inventory_By_Id(100);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    [Fact]
    public async Task TestDeleteNonExistentInventory()
    {
        var result = await _controller.Delete_Inventory(9999);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Inventory could not be deleted or does not exist.", badRequestResult.Value);
    }
}