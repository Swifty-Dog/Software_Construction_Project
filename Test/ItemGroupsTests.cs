using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemGroupsTest
{
    private readonly MyContext _context;
    private readonly ItemGroupController _controller;

    public ItemGroupsTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemGroupsTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new Item_groupService(_context);
        _controller = new ItemGroupController(service);
    }

    private void SeedData()
    {
        _context.ItemGroups.RemoveRange(_context.ItemGroups);
        _context.SaveChanges();
    }
}