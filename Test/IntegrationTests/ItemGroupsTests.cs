using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemGroupsTest
{
    private readonly MyContext _context;
    private readonly ItemGroupController _controller;

    public ItemGroupsTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemGroupsTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ItemGroupService(_context);
        _controller = new ItemGroupController(service);
    }

    private void SeedData()
    {
        _context.ItemGroups.RemoveRange(_context.ItemGroups);
        _context.SaveChanges();

        var itemGroups = new List<ItemGroup>
        {
            new ItemGroup
            {
                Id = 1,
                Name = "Furniture",
                Description = "Tables, chairs, and more.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ItemGroup
            {
                Id = 2,
                Name = "Appliances",
                Description = "Kitchen and home appliances.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _context.ItemGroups.AddRange(itemGroups);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItemGroups()
    {
        var result = await _controller.GetItemGroups();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemGroups = Xunit.Assert.IsType<List<ItemGroup>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemGroups);
    }

    [Fact]
    public async Task TestGetItemGroupsById()
    {
        var result = await _controller.GetItemGroupById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemGroup = Xunit.Assert.IsType<ItemGroup>(okResult.Value);
        Xunit.Assert.Equal("Furniture", itemGroup.Name);
        Xunit.Assert.Equal(1, itemGroup.Id);
    }

    [Fact]
    public async Task TestGetNonExistentItemGroup()
    {
        var result = await _controller.GetItemGroupById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostItemGroup()
    {
        var newItemGroup = new ItemGroup
        {
            Id = 3,
            Name = "Test",
            Description = "Testtest",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddItemGroup(newItemGroup);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemGroup = Xunit.Assert.IsType<ItemGroup>(okResult.Value);
        Xunit.Assert.Equal("Test", itemGroup.Name);
        Xunit.Assert.Equal("Testtest", itemGroup.Description);
        Xunit.Assert.Equal(3, itemGroup.Id);
    }

    [Fact]

    public async Task TestPutItemGroup()
    {
        var updatedItemGroup = new ItemGroup
        {
            Id = 1,
            Name = "Test_updated_put",
            Description = "Testtest",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateItemGroup(1, updatedItemGroup);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemGroup = Xunit.Assert.IsType<ItemGroup>(okResult.Value);
        Xunit.Assert.Equal("Test_updated_put", itemGroup.Name);
        Xunit.Assert.Equal(1,itemGroup.Id);

        //TIJDELIET NIET WERKBAAR?
    }

    [Fact]

    public async Task DeleteItemGroup()
    {
        var result = await _controller.DeleteItemGroup(2);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetItemGroupById(2);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);

    }
    //VOLGENSMIJ OMDAT ITEMGROUPSERVICES BOOLS RETURNT BIJ DELETE, DIT MOETEN WE BIJ VERDERE VALIDATIE CHECK DOEN
}

