using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using api.Migrations;

public class ItemTest
{
    private readonly MyContext _context;
    private readonly ItemController _controller;

    public ItemTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ItemServices(_context);
        _controller = new ItemController(service);
    }

    private void SeedData()
    {
        _context.Items.RemoveRange(_context.Items);
        _context.SaveChanges();

        var item = new Item
        {
            Uid = "P000002",
            Code = "sjQ23408K",
            Description = "Face-to-face clear-thinking complexity",
            Short_Description = "must",
            Upc_code = "6523540947122",
            Model_number = "63-OFFTq0T",
            Commodity_code = "oTo304",
            Item_line = 11,
            Item_group = 73,
            item_type = 14,
            unit_purchase_quantity = 47,
            unit_order_quantity = 13,
            pack_order_quantity = 11,
            supplier_id = 34,
            supplier_code = "SUP423",
            supplier_part_number = "E-86805-uTM",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        _context.Items.Add(item);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItems()
    {
        var result = await _controller.Get_Items();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var items = Xunit.Assert.IsType<List<Item>>(okResult.Value);
        Xunit.Assert.NotEmpty(items);
    }

    [Fact]
    public async Task TestGetItemById()
    {
        var result = await _controller.Get_Item_By_Id("P000002");
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("Face-to-face clear-thinking complexity", item.Description);
        Xunit.Assert.Equal("P000002", item.Uid);
    }

    [Fact]
    public async Task TestGetNonExistentItem()
    {
        var result = await _controller.Get_Item_By_Id("abcdefg");
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }
}