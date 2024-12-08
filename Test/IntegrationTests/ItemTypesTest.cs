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

        var service = new Item_TypeServices(_context);
        _controller = new ItemTypeController(service);
    }
    public void SeedData()
    {
        _context.ItemTypes.RemoveRange(_context.ItemTypes);
        _context.SaveChanges();

        var itemType = new Item_type
        {
            Id = 1,
            Name = "Tech Gadgets",
            Description = "",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        _context.ItemTypes.Add(itemType);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItemTypes()
    {
        var result = await _controller.GetItem_types();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<List<Item_type>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemType);
    }

    [Fact]
    public async Task TestGetItemTypeById()
    {
        var result = await _controller.GetItem_types_By_Id(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<Item_type>(okResult.Value);
        Xunit.Assert.Equal(1, itemType.Id);
        Xunit.Assert.Equal("Tech Gadgets", itemType.Name);
    }

    [Fact]
    public async Task TestGetNonexistentItemType()
    {
        var result = await _controller.GetItem_types_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostItemType()
    {
        var newItemType = new Item_type
        {
            Id = 123,
            Name = "New Gadgets",
            Description = "stuff",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        var result = await _controller.AddItem_types(newItemType);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<Item_type>(okResult.Value);
        Xunit.Assert.Equal(123, itemType.Id);
        Xunit.Assert.Equal("New Gadgets", itemType.Name);
    }

    [Fact]
    public async Task TestPutItemType()
    {
        var updatedItemType = new Item_type
        {
            Id = 1,
            Name = "Changed Gadgets",
            Description = "stuff has changed",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        var result = await _controller.UpdateItem_types(1, updatedItemType);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemType = Xunit.Assert.IsType<Item_type>(okResult.Value);
        Xunit.Assert.Equal(1, itemType.Id);
        Xunit.Assert.Equal("Changed Gadgets", itemType.Name);
        Xunit.Assert.Equal("stuff has changed", itemType.Description);
    }

    [Fact]
    public async Task TestDeleteItemType()
    {
        var result = await _controller.DeleteItem_types(1);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetItem_types_By_Id(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    

}