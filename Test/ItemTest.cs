using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

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


}