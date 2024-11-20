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

    }
}