using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class LocationServicesTests
{
    private readonly MyContext _context;
    private readonly LocationServices _service;

    public LocationServicesTests()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "LocationTestDB")
            .Options;

        _context = new MyContext(options);
        SeedData();

        _service = new LocationServices(_context);
    }

    private void SeedData()
    {
        _context.Locations.AddRange(
            new Locations 
            { 
                Id = 1, 
                WarehouseId = 1, 
                Code = ".1.0", 
                Name = "Row: A, Rack: 1, Shelf: 0",
                CreatedAt = new DateTime(1992, 5, 15, 3, 21, 32),
                UpdatedAt = new DateTime(1992, 5, 15, 3, 21, 32)
            },
            new Locations 
            { 
                Id = 2, 
                WarehouseId = 2, 
                Code = ".2.0", 
                Name = "Row: B, Rack: 2, Shelf: 0",
                CreatedAt = new DateTime(1992, 6, 15, 3, 21, 32),
                UpdatedAt = new DateTime(1992, 6, 15, 3, 21, 32)
            }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task Get_All_Locations()
    {
        var result = await _service.GetAll();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Get_Location_By_Id()
    {
        var result = await _service.Get(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(".1.0", result.Code);
        Xunit.Assert.Equal("Row: A, Rack: 1, Shelf: 0", result.Name);
    }

    [Fact]
    public async Task Get_Invalid_Id()
    {
        var result = await _service.Get(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Add_Valid_Location()
    {
        var newLocation = new Locations 
        { 
            Id = 3, 
            WarehouseId = 3, 
            Code = ".3.0", 
            Name = "Row: C, Rack: 3, Shelf: 0",
            CreatedAt = new DateTime(1992, 7, 15, 3, 21, 32),
            UpdatedAt = new DateTime(1992, 7, 15, 3, 21, 32)
        };

        var result = await _service.Add_Location(newLocation);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(".3.0", result.Code);
        Xunit.Assert.Equal(3, _context.Locations.Count());
    }

    // [Fact]
    // public async Task Add_Location_Returns_Null_If_Location_Exists()
    // {
    //     var duplicateLocation = new Locations 
    //     { 
    //         Id = 1, 
    //         WarehouseId = 1, 
    //         Code = ".1.0", 
    //         Name = "Duplicate Test"
    //     };
    //     var result = await _service.Add_Location(duplicateLocation);

    //     Xunit.Assert.Null(result);
    //     Xunit.Assert.Equal(2, _context.Locations.Count());
    // }

    [Fact]
    public async Task Update_Existing_Location()
    {
        var updatedLocation = new Locations 
        { 
            Id = 1, 
            WarehouseId = 1, 
            Code = "SAMI TEST", 
            Name = "Row: A, Rack: 1, Shelf: 1",
            CreatedAt = new DateTime(1992, 5, 15, 3, 21, 32),
            UpdatedAt = new DateTime(2024, 10, 19, 10, 0, 0)
        };

        var result = await _service.Update_Location(1, updatedLocation);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SAMI TEST", result.Code);
        Xunit.Assert.Equal("Row: A, Rack: 1, Shelf: 1", result.Name);
    }

    [Fact]
    public async Task Update_Location_Invalid()
    {
        var updatedLocation = new Locations 
        { 
            Id = 999, 
            WarehouseId = 999, 
            Code = "INVALID_TEST", 
            Name = "Invalid Update",
            CreatedAt = new DateTime(1992, 5, 15, 3, 21, 32),
            UpdatedAt = new DateTime(2024, 10, 19, 10, 0, 0)
        };

        var result = await _service.Update_Location(999, updatedLocation);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task DeleteLocation_Valid()
    {
        var result = await _service.DeleteLocation(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.Locations.Count());
    }

    [Fact]
    public async Task DeleteLocation_Invalid()
    {
        var result = await _service.DeleteLocation(999);
        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.Locations.Count());
    }
}


//t