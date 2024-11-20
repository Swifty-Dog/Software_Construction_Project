using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemLinesTest
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
}