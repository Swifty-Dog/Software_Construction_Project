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

    [Fact]
    public async Task Test_Get_Item_Groups()
    {
        var result = await _controller.Get_Item_groups();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemgroups = Xunit.Assert.IsType<List<Item_group>>(okResult.Value);
        Xunit.Assert.NotEmpty(itemgroups);
    }

    [Fact]
    public async Task Test_Get_Item_Groups_By_Id()
    {
        var result = await _controller.Get_Item_group_By_Id(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemgroup = Xunit.Assert.IsType<Item_group>(okResult.Value);
        Xunit.Assert.Equal("Furniture", itemgroup.Name);
        Xunit.Assert.Equal(1, itemgroup.Id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Item_group()
    {
        var result = await _controller.Get_Item_group_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Test_Post_Item_Group()
    {
        var newItemGroup = new Item_group
        {
            Id = 3,
            Name = "Test",
            Description = "Testtest",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.Add_Item_group(newItemGroup);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var itemGroup = Xunit.Assert.IsType<Item_group>(okResult.Value);
        Xunit.Assert.Equal("Test", itemGroup.Name);
        Xunit.Assert.Equal("Testtest", itemGroup.Description);
        Xunit.Assert.Equal(3, itemGroup.Id);
    }
}

