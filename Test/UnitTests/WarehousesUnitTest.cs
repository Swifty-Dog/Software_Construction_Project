using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class WarehouseServicesTest
{
    private readonly MyContext _context;
    private readonly WarehouseServices _service;

    public WarehouseServicesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "WarehouseTestDB")
            .Options;

        _context = new MyContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedData();

        _service = new WarehouseServices(_context);
    }

    private void SeedData()
    {
        _context.Contact.AddRange(
            new Contact
            {
                Id = 1,
                Name = "John Doe",
                Phone = "123-456-7890",
                Email = "john.doe@warehouseone.com"
            },
            new Contact
            {
                Id = 2,
                Name = "Jane Smith",
                Phone = "987-654-3210",
                Email = "jane.smith@warehousetwo.com"
            }
        );
        
        _context.Warehouse.AddRange(
            new Warehouse
            {
                Id = 1,
                Code = "WH001",
                Name = "Warehouse One",
                Address = "123 Warehouse Rd",
                Zip = "12345",
                City = "New York",
                Province = "New York",
                Country = "USA",
                CreatedAt = new DateTime(2023, 1, 15),
                UpdatedAt = new DateTime(2023, 1, 15),
                Contact = _context.Contact.Find(1)
            },
            new Warehouse
            {
                Id = 2,
                Code = "WH002",
                Name = "Warehouse Two",
                Address = "456 Another Warehouse Rd",
                Zip = "67890",
                City = "Los Angeles",
                Province = "California",
                Country = "USA",
                CreatedAt = new DateTime(2023, 2, 15),
                UpdatedAt = new DateTime(2023, 2, 15),
                Contact = _context.Contact.Find(2)
            }
        );
        
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetWarehouseById()
    {
        var result = await _service.GetWarehouseById(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("WH001", result.Code);
        Xunit.Assert.Equal("Warehouse One", result.Name);
        Xunit.Assert.NotNull(result.Contact);
        Xunit.Assert.Equal("John Doe", result.Contact.Name);
    }

    [Fact]
    public async Task GetWarehouseByIdInvalid()
    {
        var result = await _service.GetWarehouseById(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task AddValidWarehouse()
    {
        var newWarehouse = new Warehouse
        {
            Id = 5,
            Code = "WH003",
            Name = "Warehouse Three",
            Address = "789 Tech Park",
            Zip = "54321",
            City = "Texas",
            Province = "Texas",
            Country = "USA",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Contact = _context.Contact.Find(1)
        };

        var result = await _service.AddWarehouse(newWarehouse);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("WH003", result.Code);
        Xunit.Assert.Equal(3, _context.Warehouse.Count());
    }

    [Fact]
    public async Task AddDuplicateWarehouse()
    {
        var duplicateWarehouse = new Warehouse
        {
            Id = 1,
            Code = "WH001",
            Name = "Warehouse One Duplicate", 
            Address = "123 Main Street", 
            Zip = "12345", 
            City = "New York", 
            Province = "New York", 
            Country = "USA", 
            CreatedAt = DateTime.Now, 
            UpdatedAt = DateTime.Now, 
            Contact = _context.Contact.Find(1)
        };

        var result = await _service.AddWarehouse(duplicateWarehouse);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.Warehouse.Count());
    }

    [Fact]
    public async Task UpdateExistingWarehouse()
    {
        var updatedWarehouse = new Warehouse
        {
            Id = 1,
            Code = "WH001-UPDATED",
            Name = "Updated Warehouse One",
            Address = "Updated Address",
            Zip = "54321",
            City = "Updated City",
            Province = "Updated Province",
            Country = "Updated Country",
            CreatedAt = new DateTime(2023, 1, 15),
            UpdatedAt = DateTime.Now,
            Contact = _context.Contact.Find(2)
        };

        var result = await _service.UpdateWarehouse(1, updatedWarehouse);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("WH001-UPDATED", result.Code);
        Xunit.Assert.Equal("Updated Warehouse One", result.Name);
        Xunit.Assert.Equal("Jane Smith", result.Contact.Name);
    }

    [Fact]
    public async Task DeleteWarehouseValid()
    {
        var result = await _service.DeleteWarehouse(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.Warehouse.Count());
    }

    [Fact]
    public async Task DeleteWarehouseInvalid()
    {
        var result = await _service.DeleteWarehouse(999);

        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.Warehouse.Count());
    }
}

//dotnet test --filter "FullyQualifiedName~WarehouseServicesTest" 