using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tests.Utilities;

public class SuppliersServicesUnitTests
{
    private readonly Mock<MyContext> _mockContext;
    private readonly Mock<DbSet<Supplier>> _mockDbSet;
    private readonly SuppliersServices _suppliersServices;

    public SuppliersServicesUnitTests()
    {
        // Create DbContextOptions for the mocked context
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database for isolated tests
            .Options;

        // Initialize the mocked DbSet and context
        _mockDbSet = new Mock<DbSet<Supplier>>();
        _mockContext = new Mock<MyContext>(options) { CallBase = true }; // Pass options to the constructor

        var mockData = new List<Supplier>
        {
            new Supplier
            {
                Id = 1,
                Code = "SUP001",
                Name = "Global Supplies Inc",
                Address = "123 Supplier Lane",
                AddressExtra = "Suite 100",
                ZipCode = "12345",
                Province = "New York",
                Country = "United States",
                ContactName = "Alice Johnson",
                Phonenumber = "555-6789",
                City = "SupplyCity",
                Reference = "Ref001",
                CreatedAt = new DateTime(2022, 12, 1),
                UpdatedAt = new DateTime(2023, 1, 1)
            },
            new Supplier
            {
                Id = 2,
                Code = "SUP002",
                Name = "Tech World",
                Address = "456 Tech Street",
                AddressExtra = null,
                ZipCode = "67890",
                Province = "California",
                Country = "United States",
                ContactName = "Bob Smith",
                Phonenumber = "555-1234",
                City = "TechCity",
                Reference = "Ref002",
                CreatedAt = new DateTime(2022, 12, 15),
                UpdatedAt = new DateTime(2023, 2, 1)
            }
        };

        // Use DbSetMockExtensions to mock DbSet
        _mockDbSet.ReturnsDbSet(mockData);
        _mockContext.Setup(c => c.Suppliers).Returns(_mockDbSet.Object);

        // Create SuppliersServices instance
        _suppliersServices = new SuppliersServices(_mockContext.Object);
    }

    [Fact]
    public async Task TestGetAllSuppliers()
    {
        // Act
        var suppliers = await _suppliersServices.GetSuppliers();

        // Assert
        Xunit.Assert.NotNull(suppliers);
        Xunit.Assert.Equal(2, suppliers.Count());
    }

    [Fact]
    public async Task TestGetSupplierById()
    {
        // Act
        var supplier = await _suppliersServices.GetSupplierById(1);

        // Assert
        Xunit.Assert.NotNull(supplier);
        Xunit.Assert.Equal(1, supplier.Id);
        Xunit.Assert.Equal("Global Supplies Inc", supplier.Name);
    }

    [Fact]
    public async Task TestGetNonExistentSupplierById()
    {
        // Act
        var supplier = await _suppliersServices.GetSupplierById(99);

        // Assert
        Xunit.Assert.Null(supplier);
    }

    [Fact]
    public async Task TestAddSupplier()
    {
        // Arrange
        var newSupplier = new Supplier
        {
            Id = 3,
            Code = "SUP003",
            Name = "Innovative Supplies",
            Address = "789 Innovation Way",
            AddressExtra = "Building A",
            ZipCode = "55555",
            Province = "Nevada",
            Country = "United States",
            ContactName = "Jane Doe",
            Phonenumber = "123-456-7890",
            City = "InnovationCity",
            Reference = "Ref003",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var addedSupplier = await _suppliersServices.AddSupplier(newSupplier);

        // Assert
        Xunit.Assert.NotNull(addedSupplier);
        Xunit.Assert.Equal(newSupplier.Code, addedSupplier.Code);
        _mockDbSet.Verify(d => d.Add(It.IsAny<Supplier>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestAddExistingSupplier()
    {
        // Arrange
        var existingSupplier = new Supplier
        {
            Id = 1,
            Code = "SUP001",
            Name = "Global Supplies Inc",
            Address = "123 Supplier Lane",
            AddressExtra = "Suite 100",
            ZipCode = "12345",
            Province = "New York",
            Country = "United States",
            ContactName = "Alice Johnson",
            Phonenumber = "555-6789",
            City = "SupplyCity",
            Reference = "Ref001",
            CreatedAt = new DateTime(2022, 12, 1),
            UpdatedAt = new DateTime(2023, 1, 1)
        };

        // Act
        var addedSupplier = await _suppliersServices.AddSupplier(existingSupplier);

        // Assert
        Xunit.Assert.Null(addedSupplier);
        _mockDbSet.Verify(d => d.Add(It.IsAny<Supplier>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task TestUpdateSupplier()
    {
        // Arrange
        var updatedSupplier = new Supplier
        {
            Id = 1,
            Code = "SUP001",
            Name = "Updated Supplies",
            Address = "Updated Address",
            AddressExtra = "Updated Extra",
            ZipCode = "99999",
            Province = "Updated Province",
            Country = "Updated Country",
            ContactName = "Updated Contact",
            Phonenumber = "Updated Phone",
            City = "Updated City",
            Reference = "Updated Ref",
            CreatedAt = new DateTime(2022, 12, 1),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _suppliersServices.UpdateSupplier(1, updatedSupplier);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Updated Supplies", result.Name);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestUpdateNonExistentSupplier()
    {
        // Arrange
        var updatedSupplier = new Supplier
        {
            Id = 99,
            Code = "SUP099",
            Name = "Nonexistent",
            Address = "N/A",
            AddressExtra = "N/A",
            ZipCode = "N/A",
            Province = "N/A",
            Country = "N/A",
            ContactName = "N/A",
            Phonenumber = "N/A",
            City = "N/A",
            Reference = "N/A",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _suppliersServices.UpdateSupplier(99, updatedSupplier);

        // Assert
        Xunit.Assert.Null(result);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task TestDeleteSupplier()
    {
        // Act
        var result = await _suppliersServices.DeleteSupplier(1);

        // Assert
        Xunit.Assert.True(result);
        _mockDbSet.Verify(d => d.Remove(It.IsAny<Supplier>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestDeleteNonExistentSupplier()
    {
        // Act
        var result = await _suppliersServices.DeleteSupplier(99);

        // Assert
        Xunit.Assert.False(result);
        _mockDbSet.Verify(d => d.Remove(It.IsAny<Supplier>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }
}
