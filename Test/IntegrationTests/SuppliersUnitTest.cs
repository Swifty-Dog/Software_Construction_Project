using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System;

public class SuppliersServicesTest
{
    private readonly MyContext _context;
    private readonly SuppliersServices _service;

    public SuppliersServicesTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "SuppliersTestDB")
            .Options;

        _context = new MyContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedData();

        _service = new SuppliersServices(_context);
    }

    private void SeedData()
    {
        _context.Suppliers.AddRange(
            new Supplier
            {
                Id = 1,
                Code = "SUPP001",
                Name = "Supplier One",
                Address = "123 Main Street",
                AddressExtra = "Suite 101",
                City = "Los Angeles",
                ZipCode = "12345",
                Province = "New York",
                Country = "USA",
                ContactName = "John Doe",
                Phonenumber = "123-456-7890",
                Reference = "REF001",
                CreatedAt = new DateTime(2023, 1, 15),
                UpdatedAt = new DateTime(2023, 1, 15)
            },
            new Supplier
            {
                Id = 2,
                Code = "SUPP002",
                Name = "Supplier Two",
                Address = "456 Another Street",
                AddressExtra = "Suite 202",
                City = "Los Angeles",
                ZipCode = "67890",
                Province = "California",
                Country = "USA",
                ContactName = "Jane Smith",
                Phonenumber = "987-654-3210",
                Reference = "REF002",
                CreatedAt = new DateTime(2023, 2, 15),
                UpdatedAt = new DateTime(2023, 2, 15)
            }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllSuppliers()
    {
        var result = await _service.GetSuppliers();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetSupplierById()
    {
        var result = await _service.GetSupplierById(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP001", result.Code);
        Xunit.Assert.Equal("Supplier One", result.Name);
    }

    [Fact]
    public async Task GetSupplierByInvalidId()
    {
        var result = await _service.GetSupplierById(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task AddValidSupplier()
    {
        var newSupplier = new Supplier
        {
            Id = 3,
            Code = "SUPP003",
            Name = "Supplier Three",
            Address = "789 Tech Park",
            AddressExtra = "Suite 303",
            City = "Los Angeles",
            ZipCode = "54321",
            Province = "Texas",
            Country = "USA",
            ContactName = "Tom Johnson",
            Phonenumber = "555-555-5555",
            Reference = "REF003",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var result = await _service.AddSupplier(newSupplier);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP003", result.Code);
        Xunit.Assert.Equal(3, _context.Suppliers.Count());
    }

    [Fact]
    public async Task AddInvalidSupplierDuplicate()
    {
        var duplicateSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP001",
            Name = "Duplicate Supplier",
            Address = "123 Main Street",
            AddressExtra = "Suite 101",
            City = "Los Angeles",
            ZipCode = "12345",
            Province = "New York",
            Country = "USA",
            ContactName = "John Doe",
            Phonenumber = "123-456-7890",
            Reference = "REF001",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var result = await _service.AddSupplier(duplicateSupplier);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.Suppliers.Count());
    }

    [Fact]
    public async Task UpdateExistingSupplier()
    {
        var updatedSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP001-UPDATED",
            Name = "Updated Supplier One",
            Address = "Updated Address",
            AddressExtra = "Updated Suite",
            City = "Los Angeles",
            ZipCode = "54321",
            Province = "Updated Province",
            Country = "Updated Country",
            ContactName = "Updated Contact",
            Phonenumber = "999-999-9999",
            Reference = "REF001-UPDATED",
            CreatedAt = new DateTime(2023, 1, 15),
            UpdatedAt = DateTime.Now
        };

        var result = await _service.UpdateSupplier(1, updatedSupplier);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP001-UPDATED", result.Code);
        Xunit.Assert.Equal("Updated Supplier One", result.Name);
    }

    [Fact]
    public async Task UpdateSupplierInvalid()
    {
        var updatedSupplier = new Supplier
        {
            Id = 999,
            Code = "INVALID",
            Name = "Invalid Supplier",
            Address = "Invalid Address",
            AddressExtra = "Invalid Suite",
            City = "Los Angeles",
            ZipCode = "54321",
            Province = "Invalid Province",
            Country = "Invalid Country",
            ContactName = "Invalid Contact",
            Phonenumber = "000-000-0000",
            Reference = "REF999",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var result = await _service.UpdateSupplier(999, updatedSupplier);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSupplierValid()
    {
        var result = await _service.DeleteSupplier(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.Suppliers.Count());
    }

    [Fact]
    public async Task DeleteSupplierInvalid()
    {
        var result = await _service.DeleteSupplier(999);

        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.Suppliers.Count());
    }
}

//dotnet test --filter "FullyQualifiedName~SuppliersServicesTest" 