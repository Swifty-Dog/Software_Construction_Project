using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class ItemGroupsUnitTest
{
    private readonly MyContext _context;
    private readonly ItemGroupController _controller;

    public ItemGroupsUnitTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemGroupsTestDB")
            .Options;

        _context = new MyContext(options);
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        SeedData();
        
        var service = new Item_groupService(_context);
        _controller = new ItemGroupController(service);
    }

    private void SeedData()
    {
        _context.ItemGroups.RemoveRange(_context.ItemGroups);
        _context.SaveChanges();

        var itemGroups = new List<Item_group>
        {
            new Item_group
            {
                Id = 1,
                Name = "Furniture",
                Description = "Tables, chairs, and more.",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            },
            new Item_group
            {
                Id = 2,
                Name = "Appliances",
                Description = "Kitchen and home appliances.",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            }
        };

        _context.ItemGroups.AddRange(itemGroups);
        _context.SaveChanges();
    }
}