using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ShipmentsTest
{
    private readonly MyContext _context;
    private readonly ShipmentsController _controller;

    public ShipmentsTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ShipmentsTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ShipmentsServices(_context);
        _controller = new ShipmentsController(service);
    }

    private void SeedData()
    {
        _context.Shipments.RemoveRange(_context.Shipments);
        _context.ShipmentsItems.RemoveRange(_context.ShipmentsItems);
        _context.SaveChanges();

        var shipment = new Shipment
        {
            Id = 1,
            OrderId = 3,
            SourceId = 52,
            OrderDate = new DateOnly(1973, 1, 28),
            RequestDate = new DateOnly(1973, 1, 30),
            ShipmentDate = new DateOnly(1973, 2, 1),
            ShipmentType = "I",
            ShipmentStatus = "Pending",
            Notes = "Hoog genot springen afspraak mond bus.",
            CarrierCode = "DHL",
            CarrierDescription = "DHL Express",
            ServiceCode = "NextDay",
            PaymentType = "Automatic",
            TransferMode = "Ground",
            TotalPackageCount = 29,
            TotalPackageWeight = 463,
            CreatedAt = DateTime.Parse("1973-01-28T20:09:11Z"),
            UpdatedAt = DateTime.Parse("1973-01-29T22:09:11Z"),
            Items = new List<ShipmentsItem>
            {
                new ShipmentsItem { ItemId = "P010669", Amount = 16 }
            }
        };

        _context.Shipments.Add(shipment);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetShipments()
    {
        var result = await _controller.GetShipments();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var shipments = Xunit.Assert.IsType<List<Shipment>>(okResult.Value);
        Xunit.Assert.NotEmpty(shipments);
    }

    [Fact]
    public async Task TestGetShipmentById()
    {
        var result = await _controller.GetShipmentById(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var shipment = Xunit.Assert.IsType<Shipment>(okResult.Value);
        Xunit.Assert.Equal(1, shipment.Id);
        Xunit.Assert.Equal(3, shipment.OrderId);
    }

    [Fact]
    public async Task TestGetNonExistentShipment()
    {
        var result = await _controller.GetShipmentById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestGetShipmentItems()
    {
        var result = await _controller.GetShipmentItems(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var items = Xunit.Assert.IsType<List<ShipmentsItem>>(okResult.Value);
        //Xunit.Assert.Equal(1, items.Count);
        Xunit.Assert.Single(items);
        var firstItem = items.First();
        Xunit.Assert.Equal("P010669", firstItem.ItemId);
        Xunit.Assert.Equal(16, firstItem.Amount);
    }

    [Fact]
    public async Task TestPostShipment()
    {
        var newShipment = new Shipment
        {
            Id = 2,
            OrderId = 4,
            SourceId = 61,
            OrderDate = new DateOnly(2022, 5, 10),
            RequestDate = new DateOnly(2022, 5, 12),
            ShipmentDate = new DateOnly(2022, 5, 14),
            ShipmentType = "E",
            ShipmentStatus = "Delivered",
            Notes = "Shipment created for testing.",
            CarrierCode = "FedEx",
            CarrierDescription = "FedEx Ground",
            ServiceCode = "Standard",
            PaymentType = "Prepaid",
            TransferMode = "Air",
            TotalPackageCount = 10,
            TotalPackageWeight = 50,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = new List<ShipmentsItem>
            {
                new ShipmentsItem { ItemId = "P020202", Amount = 5 },
                new ShipmentsItem { ItemId = "P030303", Amount = 5 }
            }
        };

        var result = await _controller.AddShipment(newShipment);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.Equal("Shipment and items added successfully", okResult.Value);
    }

    [Fact]
    public async Task TestPostExistingShipment()
    {
        var existingShipment = new Shipment
        {
            Id = 1,
            OrderId = 3,
            SourceId = 52,
            OrderDate = new DateOnly(1973, 1, 28),
            RequestDate = new DateOnly(1973, 1, 30),
            ShipmentDate = new DateOnly(1973, 2, 1),
            ShipmentType = "I",
            ShipmentStatus = "Pending",
            Notes = "Duplicate Shipment",
            CarrierCode = "DHL",
            CarrierDescription = "DHL Express",
            ServiceCode = "NextDay",
            PaymentType = "Automatic",
            TransferMode = "Ground",
            TotalPackageCount = 29,
            TotalPackageWeight = 463,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = new List<ShipmentsItem>
            {
                new ShipmentsItem { ItemId = "P010669", Amount = 16 }
            }
        };

        var result = await _controller.AddShipment(existingShipment);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Shipment with the same id already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task TestDeleteShipment()
    {
        var result = await _controller.DeleteShipment(1);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.GetShipmentById(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    [Fact]
    public async Task TestDeleteNonExistentShipment()
    {
        var result = await _controller.DeleteShipment(9999);
        var notFoundResult = Xunit.Assert.IsType<NotFoundObjectResult>(result);
        Xunit.Assert.Equal("Shipment not found or already deleted.", notFoundResult.Value);
    }
}
