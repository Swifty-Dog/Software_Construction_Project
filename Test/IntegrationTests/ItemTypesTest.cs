using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemTypesTest 
{
    private readonly MyContext _context;
    private readonly ItemTypeController _controller;

    public ItemTypesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemTypesTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ItemTypeServices(_context);
        _controller = new ItemTypeController(service);
    }
    public void SeedData()
    {
        _context.ItemTypes.RemoveRange(_context.ItemTypes);
        _context.SaveChanges();

        var itemType = new ItemType
        {
            Id = 1,
            Name = "Tech Gadgets",
            Description = "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ItemTypes.Add(itemType);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItemTypes()
    {
        var result = await _controller.GetItemTypes();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<List<ItemType>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemType);
    }

    [Fact]
    public async Task TestGetItemTypeById()
    {
        var result = await _controller.GetItemTypesById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<ItemType>(okResult.Value);
        Xunit.Assert.Equal(1, itemType.Id);
        Xunit.Assert.Equal("Tech Gadgets", itemType.Name);
    }

    [Fact]
    public async Task TestGetNonexistentItemType()
    {
        var result = await _controller.GetItemTypesById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostItemType()
    {
        var newItemType = new ItemType
        {
            Id = 123,
            Name = "New Gadgets",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await _controller.AddItemType(newItemType);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<ItemType>(okResult.Value);
        Xunit.Assert.Equal(123, itemType.Id);
        Xunit.Assert.Equal("New Gadgets", itemType.Name);
    }

    [Fact]
    public async Task TestPutItemType()
    {
        var updatedItemType = new ItemType
        {
            Id = 1,
            Name = "Changed Gadgets",
            Description = "stuff has changed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await _controller.UpdateItemTypes(1, updatedItemType);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<ItemType>(okResult.Value);
        Xunit.Assert.Equal(1, itemType.Id);
        Xunit.Assert.Equal("Changed Gadgets", itemType.Name);
        Xunit.Assert.Equal("stuff has changed", itemType.Description);
    }

    [Fact]
    public async Task TestDeleteItemType()
    {
        var result = await _controller.DeleteItemTypes(1);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetItemTypesById(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}