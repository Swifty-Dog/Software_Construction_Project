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

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

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
    public async Task GetAllLocations()
    {
        var result = await _service.GetLocations();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetLocationById()
    {
        var result = await _service.GetLocationById(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(".1.0", result.Code);
        Xunit.Assert.Equal("Row: A, Rack: 1, Shelf: 0", result.Name);
    }

    [Fact]
    public async Task GetInvalidId()
    {
        var result = await _service.GetLocationById(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task AddValidLocation()
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

        var result = await _service.AddLocation(newLocation);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(".3.0", result.Code);
        Xunit.Assert.Equal(3, _context.Locations.Count());
    }

    [Fact]
    public async Task AddInvalidLocation()
    {
        var duplicateLocation = new Locations 
        { 
            Id = 1, 
            WarehouseId = 1, 
            Code = ".1.0", 
            Name = "Duplicate Test",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var result = await _service.AddLocation(duplicateLocation);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.Locations.Count());
    }

    [Fact]
    public async Task UpdateExistingLocation()
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

        var result = await _service.UpdateLocation(1, updatedLocation);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SAMI TEST", result.Code);
        Xunit.Assert.Equal("Row: A, Rack: 1, Shelf: 1", result.Name);
    }

    [Fact]
    public async Task UpdateLocationInvalid()
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

        var result = await _service.UpdateLocation(999, updatedLocation);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task DeleteLocationValid()
    {
        var result = await _service.DeleteLocation(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.Locations.Count());
    }

    [Fact]
    public async Task DeleteLocationInvalid()
    {
        var result = await _service.DeleteLocation(999);
        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.Locations.Count());
    }
}


//eerst cd test
//dotnet test --filter "FullyQualifiedName~LocationServicesTests"     <-- de laatste stuk is je class naam, ik kan het niet werkend krijgen in een aparte unittest folder dus voor nu is het dit