using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemLinesTest // data in de database en wat hier staat en de rest file nog vergelijken 
{
    private readonly MyContext _context;
    private readonly ItemLinesController _controller;

    public ItemLinesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemLinesTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ItemLineServices(_context);
        _controller = new ItemLinesController(service);
    }
    public void SeedData()
    {
        _context.ItemLines.RemoveRange(_context.ItemLines);
        _context.SaveChanges();

        var itemLine = new ItemLine
        {
            Id = 1,
            Name = "Tech Gadgets",
            Description = "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ItemLines.Add(itemLine);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItemLine()
    {
        var result = await _controller.GetItemLines();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<List<ItemLine>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemLine);
    }

    [Fact]
    public async Task TestGetItemLineById()
    {
        var result = await _controller.GetItemLineById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<ItemLine>(okResult.Value);
        Xunit.Assert.Equal(1, itemLine.Id);
        Xunit.Assert.Equal("Tech Gadgets", itemLine.Name);
    }

    [Fact]
    public async Task TestGetNonexistentItemLine()
    {
        var result = await _controller.GetItemLineById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

// to do in client test: test delete
    [Fact]
    public async Task TestPostItemLine()
    {
        var newItemLine = new ItemLine
        {
            Id = 123,
            Name = "New Gadgets",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await _controller.AddItemLine(newItemLine);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<ItemLine>(okResult.Value);
        Xunit.Assert.Equal(123, itemLine.Id);
        Xunit.Assert.Equal("New Gadgets", itemLine.Name);
    }

    [Fact]
    public async Task TestPutItemLine()
    {
        var updatedItemline = new ItemLine
        {
            Id = 1,
            Name = "Changed Gadgets",
            Description = "stuff has changed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await _controller.UpdateItemLine(1, updatedItemline);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<ItemLine>(okResult.Value);
        Xunit.Assert.Equal(1, itemLine.Id);
        Xunit.Assert.Equal("Changed Gadgets", itemLine.Name);
        Xunit.Assert.Equal("stuff has changed", itemLine.Description);
    }

    [Fact]
    public async Task TestDeleteItemLine()
    {
        var result = await _controller.DeleteItemLine(1);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetItemLineById(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

}