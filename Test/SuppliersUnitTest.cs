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
}