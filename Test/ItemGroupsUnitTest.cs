using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

public class ItemGroupsUnitTest
{
    private readonly MyContext _context;
    private readonly Item_groupService _service;
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

        _service = new Item_groupService(_context);
        _controller = new ItemGroupController(_service);
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
    public async Task Get_All_Item_Groups()
    {
        var result = await _service.Get_Item_groups();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Get_Item_Group_By_Id()
    {
        var result = await _service.Get_Item_group_By_Id(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Furniture", result.Name);
        Xunit.Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task Get_Item_Group_By_Invalid_Id()
    {
        var result = await _service.Get_Item_group_By_Id(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Add_Valid_Item_Group()
    {
        var newItemGroup = new Item_group
        {
            Id = 3,
            Name = "Test Group",
            Description = "Test Description",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _service.AddItemGroup(newItemGroup);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Test Group", result.Name);
        Xunit.Assert.Equal(3, _context.ItemGroups.Count());
    }

    [Fact]
    public async Task Add_Invalid_Item_Group_Duplicate()
    {
        var duplicateItemGroup = new Item_group
        {
            Id = 1,
            Name = "Furniture",
            Description = "Duplicate Group",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _service.AddItemGroup(duplicateItemGroup);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.ItemGroups.Count());
    }

    [Fact]
    public async Task Update_Existing_Item_Group()
    {
        var updatedItemGroup = new Item_group
        {
            Id = 1,
            Name = "Updated Furniture",
            Description = "Updated Description",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _service.Update_Item_group(updatedItemGroup);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Updated Furniture", result.Name);
        Xunit.Assert.Equal("Updated Description", result.Description);
    }


    [Fact]
    public async Task Update_Item_Group_Invalid()
    {
        var updatedItemGroup = new Item_group
        {
            Id = 999,
            Name = "Invalid Group",
            Description = "Invalid Description",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _service.Update_Item_group(updatedItemGroup);

        Xunit.Assert.Null(result);
        //IS NOT WORKING CORRECTLY
    }
    
    [Fact]
    public async Task Delete_Item_Group_Valid()
    {
        var result = await _service.Delete_Item_group(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.ItemGroups.Count());
    }

    [Fact]
    public async Task Delete_Item_Group_Invalid()
    {
        var result = await _service.Delete_Item_group(999);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.ItemGroups.Count());
    }
    //IS NOT WORKING CORRECTLY
}


//dotnet test --filter "FullyQualifiedName~ItemGroupsUnitTest" 