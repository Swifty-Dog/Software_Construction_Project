using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SuppliersTest
{
    private readonly MyContext _context;
    private readonly SuppliersController _controller;

    public SuppliersTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "SuppliersTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new SuppliersServices(_context);
        _controller = new SuppliersController(service);
    }

    private void SeedData()
    {
        _context.Suppliers.RemoveRange(_context.Suppliers);
        _context.SaveChanges();

        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Id = 1,
                Code = "SUPP001",
                Name = "Tech Supplies",
                Address = "123 Tech Park",
                AddressExtra = "Suite 101",
                City = "Los Angeles",
                ZipCode = "67890",
                Province = "California",
                Country = "USA",
                ContactName = "John Doe",
                Phonenumber = "123-456-7890",
                Reference = "REF1234",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        //AddressExtra is a must?

        _context.Suppliers.AddRange(suppliers);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetAllSuppliers()
    {
        var result = await _controller.GetSuppliers();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var suppliers = Xunit.Assert.IsType<List<Supplier>>(okResult.Value);
        Xunit.Assert.Equal(1, suppliers.Count);
    }

    [Fact]
    public async Task TestGetSupplier()
    {
        var result = await _controller.GetSupplierById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var supplier = Xunit.Assert.IsType<Supplier>(okResult.Value);

        Xunit.Assert.Equal(1, supplier.Id);
        Xunit.Assert.Equal("SUPP001", supplier.Code);
    }

    [Fact]
    public async Task TestGetNonExistentSupplier()
    {
        var result = await _controller.GetSupplierById(999);
        var notFoundResult = Xunit.Assert.IsType<NotFoundObjectResult>(result);

        Xunit.Assert.Equal("Supplier not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task TestAddSupplier()
    {
        var newSupplier = new Supplier
        {
            Id = 50000003,
            Code = "SUPP003",
            Name = "Tech Supplies",
            Address = "123 Tech Park",
            AddressExtra = "Suite 101",
            City = "Los Angeles",
            ZipCode = "67890",
            Province = "California",
            Country = "USA",
            ContactName = "Jane Doe",
            Phonenumber = "098-765-4321",
            Reference = "REF1234",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddSupplier(newSupplier);
        var createdResult = Xunit.Assert.IsType<CreatedAtActionResult>(result);
        var supplier = Xunit.Assert.IsType<Supplier>(createdResult.Value);

        Xunit.Assert.Equal("SUPP003", supplier.Code);
        Xunit.Assert.Equal("Tech Supplies", supplier.Name);
    }

    [Fact]
    public async Task TestUpdateSupplier()
    {
        var updatedSupplier = new Supplier
        {
            Id = 50000003,
            Code = "SUPP001_UPDATED",
            Name = "Tech Supplies Updated",
            Address = "123 Tech Park",
            AddressExtra = "Suite 101",
            City = "Los Angeles",
            ZipCode = "67890",
            Province = "California",
            Country = "USA",
            ContactName = "Jane Doe",
            Phonenumber = "098-765-4321",
            Reference = "REF1234_UPDATED",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateSupplier(1, updatedSupplier);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var supplier = Xunit.Assert.IsType<Supplier>(okResult.Value);

        Xunit.Assert.Equal("SUPP001_UPDATED", supplier.Code);
        Xunit.Assert.Equal("Tech Supplies Updated", supplier.Name);
    }

    [Fact]
    public async Task TestDeleteSupplier()
    {
        var result = await _controller.DeleteSupplier(1);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetSupplierById(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}
