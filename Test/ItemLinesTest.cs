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

        var service = new Item_lineServices(_context);
        _controller = new ItemLinesController(service);
    }
    public void SeedData()
    {
        _context.ItemLines.RemoveRange(_context.ItemLines);
        _context.SaveChanges();

        var itemLine = new Item_line
        {
            Id = 0,
            Name = "Tech Gadgets",
            Description = "",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        _context.ItemLines.Add(itemLine);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItemLine()
    {
        var result = await _controller.Get_Item_groups();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<List<Item_line>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemLine);
    }

    [Fact]
    public async Task TestGetItemLineById()
    {
        var result = await _controller.Get_Item_group_By_Id(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemLine = Xunit.Assert.IsType<Item_line>(okResult.Value);
        Xunit.Assert.Equal(1, itemLine.Id);
        Xunit.Assert.Equal("Tech Gadgets", itemLine.Name);
    }

    [Fact]
    public async Task TestGetNonexistentItemLine()
    {
        var result = await _controller.Get_Item_group_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

}