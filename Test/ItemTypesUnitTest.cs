using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class ItemTypesServicesTests
{
    private readonly MyContext _context;
    private readonly Item_TypeServices _service;

    public ItemTypesServicesTests()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemTypesTestDB")
            .Options;

        _context = new MyContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedData();

        _service = new Item_TypeServices(_context);
    }

    private void SeedData()
    {
        _context.ItemTypes.AddRange(
            new Item_type 
            { 
                Id = 1, 
                Name = "test1",
                Description = "this is test1", 
                Created_at = new DateTime(2001, 9, 9, 1, 1, 1),
                Updated_at = new DateTime(2008, 9, 9, 1, 1, 1)
            },
            new Item_type 
            { 
                Id = 2, 
                Name = "test2",
                Description = "this is test2", 
                Created_at = new DateTime(2001, 9, 9, 2, 2, 2),
                Updated_at = new DateTime(2008, 9, 9, 2, 2, 2)
            }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task Get_All_ItemTypes()
    {
        var result = await _service.GetItem_types();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Get_ItemType_By_Id()
    {
        var result = await _service.GetItem_types_By_Id(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test1", result.Name);
        Xunit.Assert.Equal("this is test1", result.Description);
    }

    [Fact]
    public async Task Get_ItemType_Invalid_Id()
    {
        var result = await _service.GetItem_types_By_Id(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Add_Valid_ItemType()
    {
        var newItemType = new Item_type 
        { 
            Id = 3, 
                Name = "test3",
                Description = "this is test3", 
                Created_at = new DateTime(2001, 9, 9, 3, 3, 3),
                Updated_at = new DateTime(2008, 9, 9, 3, 3, 3)
        };

        var result = await _service.AddItem_types(newItemType);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test3", result.Name);
        Xunit.Assert.Equal(3, _context.ItemTypes.Count());
    }

    [Fact]
    public async Task Add_Invalid_ItemType()
    {
        var duplicateItemType = new Item_type
        { 
            Id = 1, 
            Name = "test1",
            Description = "Duplicate Test1", 
            Created_at = DateTime.Now,
            Updated_at = DateTime.Now
        };
        var result = await _service.AddItem_types(duplicateItemType);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.ItemTypes.Count());
    }

    [Fact]
    public async Task Update_Existing_ItemType()
    {
        var updatedItemType = new Item_type 
        { 
            Id = 1, 
            Name = "test1 again",
            Description = "Test1 but yet again", 
            Created_at = new DateTime(2001, 9, 1, 3, 3, 3),
            Updated_at = new DateTime(2008, 9, 1, 3, 3, 3)
        };

        var result = await _service.UpdateItem_types(updatedItemType);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test1 again", result.Name);
        Xunit.Assert.Equal("Test1 but yet again", result.Description);
    }

    [Fact]
    public async Task Update_ItemType_Invalid()
    {
        var updatedItemType = new Item_type 
        { 
            Id = 999, 
            Name = "Invalid Update",
            Description = "Invalid Update", 
            Created_at = new DateTime(2002, 1, 1, 1, 1, 1),
            Updated_at = new DateTime(2007, 1, 1, 1, 1, 1)
        };

        var exception = await Xunit.Assert.ThrowsAsync<Exception>(() => _service.UpdateItem_types(updatedItemType));

        Xunit.Assert.Equal("Item_types not found or has been deleted.", exception.Message);

        // var result = await _service.UpdateItem_types(updatedItemType);

        // Xunit.Assert.Null(result);

        // LET OP!:
        // zelfde met ItemLines, omdat dit NULL wordt, gooit de service een exception
        // "Item_types not found or has been deleted."
    }

    [Fact]
    public async Task DeleteItemType_Valid()
    {
        var result = await _service.DeleteItem_types(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.ItemTypes.Count());
    }

    [Fact]
    public async Task DeleteItemType_Invalid()
    {
        var result = await _service.DeleteItem_types(999);
        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.ItemTypes.Count());
    }
}


//eerst cd test
//dotnet test --filter "FullyQualifiedName~ItemTypesServicesTests"     <-- de laatste stuk is je class naam, ik kan het niet werkend krijgen in een aparte unittest folder dus voor nu is het dit