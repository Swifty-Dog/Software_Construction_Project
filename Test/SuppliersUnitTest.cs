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
                Address_extra = "Suite 101",
                Zip_code = "12345",
                Province = "New York",
                Country = "USA",
                Contact_name = "John Doe",
                Phonenumber = "123-456-7890",
                Email = "john.doe@supplierone.com",
                Reference = "REF001",
                Created_at = new DateTime(2023, 1, 15),
                Updated_at = new DateTime(2023, 1, 15)
            },
            new Supplier
            {
                Id = 2,
                Code = "SUPP002",
                Name = "Supplier Two",
                Address = "456 Another Street",
                Address_extra = "Suite 202",
                Zip_code = "67890",
                Province = "California",
                Country = "USA",
                Contact_name = "Jane Smith",
                Phonenumber = "987-654-3210",
                Email = "jane.smith@supplierstwo.com",
                Reference = "REF002",
                Created_at = new DateTime(2023, 2, 15),
                Updated_at = new DateTime(2023, 2, 15)
            }
        );
        _context.SaveChanges();
 
 
    }
     [Fact]
    public async Task Get_All_Suppliers()
    {
        var result = await _service.GetAll();

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Get_Supplier_By_Id()
    {
        var result = await _service.Get(1);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP001", result.Code);
        Xunit.Assert.Equal("Supplier One", result.Name);
    }

    [Fact]
    public async Task Get_Supplier_By_Invalid_Id()
    {
        var result = await _service.Get(999);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Add_Valid_Supplier()
    {
        var newSupplier = new Supplier
        {
            Id = 3,
            Code = "SUPP003",
            Name = "Supplier Three",
            Address = "789 Tech Park",
            Address_extra = "Suite 303",
            Zip_code = "54321",
            Province = "Texas",
            Country = "USA",
            Contact_name = "Tom Johnson",
            Phonenumber = "555-555-5555",
            Email = "tom.johnson@supplierthree.com",
            Reference = "REF003",
            Created_at = DateTime.Now,
            Updated_at = DateTime.Now
        };

        var result = await _service.AddSupplier(newSupplier);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP003", result.Code);
        Xunit.Assert.Equal(3, _context.Suppliers.Count());
    }

    [Fact]
    public async Task Add_Invalid_Supplier_Duplicate()
    {
        var duplicateSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP001",
            Name = "Duplicate Supplier",
            Address = "123 Main Street",
            Address_extra = "Suite 101",
            Zip_code = "12345",
            Province = "New York",
            Country = "USA",
            Contact_name = "John Doe",
            Phonenumber = "123-456-7890",
            Email = "duplicate@supplier.com",
            Reference = "REF001",
            Created_at = DateTime.Now,
            Updated_at = DateTime.Now
        };

        var result = await _service.AddSupplier(duplicateSupplier);

        Xunit.Assert.Null(result);
        Xunit.Assert.Equal(2, _context.Suppliers.Count());
    }

    [Fact]
    public async Task Update_Existing_Supplier()
    {
        var updatedSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP001-UPDATED",
            Name = "Updated Supplier One",
            Address = "Updated Address",
            Address_extra = "Updated Suite",
            Zip_code = "54321",
            Province = "Updated Province",
            Country = "Updated Country",
            Contact_name = "Updated Contact",
            Phonenumber = "999-999-9999",
            Email = "updated@supplierone.com",
            Reference = "REF001-UPDATED",
            Created_at = new DateTime(2023, 1, 15),
            Updated_at = DateTime.Now
        };

        var result = await _service.UpdateSupplier(1, updatedSupplier);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("SUPP001-UPDATED", result.Code);
        Xunit.Assert.Equal("Updated Supplier One", result.Name);
    }

    [Fact]
    public async Task Update_Supplier_Invalid()
    {
        var updatedSupplier = new Supplier
        {
            Id = 999,
            Code = "INVALID",
            Name = "Invalid Supplier",
            Address = "Invalid Address",
            Address_extra = "Invalid Suite",
            Zip_code = "54321",
            Province = "Invalid Province",
            Country = "Invalid Country",
            Contact_name = "Invalid Contact",
            Phonenumber = "000-000-0000",
            Email = "invalid@supplier.com",
            Reference = "REF999",
            Created_at = DateTime.Now,
            Updated_at = DateTime.Now
        };

        var result = await _service.UpdateSupplier(999, updatedSupplier);

        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task Delete_Supplier_Valid()
    {
        var result = await _service.DeleteSupplier(1);

        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, _context.Suppliers.Count());
    }

    [Fact]
    public async Task Delete_Supplier_Invalid()
    {
        var result = await _service.DeleteSupplier(999);

        Xunit.Assert.False(result);
        Xunit.Assert.Equal(2, _context.Suppliers.Count());
    }
}

//dotnet test --filter "FullyQualifiedName~SuppliersServicesTest" 