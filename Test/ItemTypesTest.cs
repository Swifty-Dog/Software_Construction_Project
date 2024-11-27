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
    
}