using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class ItemLinesServicesTests
{
    private readonly MyContext _context;
    private readonly ItemLineServices _service;

    public ItemLinesServicesTests()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemLinesTestDB")
            .Options;

        _context = new MyContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedData();

        _service = new ItemLineServices(_context);
    }

    private void SeedData()
    {
        _context.ItemLines.AddRange(
            new ItemLine 
            { 
                Id = 1, 
                Name = "test1",
                Description = "this is test1", 
                CreatedAt = new DateTime(2022, 9, 9, 1, 1, 1),
                UpdatedAt = new DateTime(2023, 9, 9, 1, 1, 1)
            },
            new ItemLine 
            { 
                Id = 2, 
                Name = "test2",
                Description = "this is test2", 
                CreatedAt = new DateTime(2022, 9, 9, 2, 2, 2),
                UpdatedAt = new DateTime(2023, 9, 9, 2, 2, 2)
            }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task Get_All_ItemLines()
    {
        var result = await _service.GetItemLine();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Get_ItemLine_By_Id()
    {
        var result = await _service.GetItemLineById(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test1", result.Name);
        Xunit.Assert.Equal("this is test1", result.Description);
    }

    [Fact]
    public async Task Get_ItemLine_Invalid_Id()
    {
        var result = await _service.GetItemLineById(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Add_Valid_ItemLine()
    {
        var newItemLine = new ItemLine 
        { 
            Id = 3, 
                Name = "test3",
                Description = "this is test3", 
                CreatedAt = new DateTime(2022, 9, 9, 3, 3, 3),
                UpdatedAt = new DateTime(2023, 9, 9, 3, 3, 3)
        };

        var result = await _service.AddItemLine(newItemLine);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test3", result.Name);
        Xunit.Assert.Equal(3, _context.ItemLines.Count());
    }

    [Fact]
    public async Task Add_Invalid_ItemLine()
    {
        var duplicateItemLine = new ItemLine
        { 
            Id = 1, 
            Name = "test1",
            Description = "Duplicate Test1", 
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var result = await _service.AddItemLine(duplicateItemLine);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.ItemLines.Count());
    }

    [Fact]
    public async Task Update_Existing_ItemLine()
    {
        var updatedItemLine = new ItemLine 
        { 
            Id = 1, 
            Name = "test1 again",
            Description = "Test1 but yet again", 
            CreatedAt = new DateTime(2022, 9, 1, 3, 3, 3),
            UpdatedAt = new DateTime(2023, 9, 1, 3, 3, 3)
        };

        var result = await _service.UpdateItemLine(updatedItemLine, 1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("test1 again", result.Name);
        Xunit.Assert.Equal("Test1 but yet again", result.Description);
    }

    [Fact]
    public async Task Update_ItemLine_Invalid()
    {
        var updatedItemLine = new ItemLine 
        { 
            Id = 999, 
            Name = "Invalid Update",
            Description = "Invalid Update", 
            CreatedAt = new DateTime(1999, 1, 1, 1, 1, 1),
            UpdatedAt = new DateTime(2000, 1, 1, 1, 1, 1)
        };

        var result = await _service.UpdateItemLine(updatedItemLine, 999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task DeleteItemLine_Valid()
    {
        var result = await _service.DeleteItemLine(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.ItemLines.Count());
    }

    [Fact]
    public async Task DeleteItemLine_Invalid()
    {
        var result = await _service.DeleteItemLine(999);
        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.ItemLines.Count());
    }
}


//eerst cd test
//dotnet test --filter "FullyQualifiedName~LocationServicesTests"     <-- de laatste stuk is je class naam, ik kan het niet werkend krijgen in een aparte unittest folder dus voor nu is het dit