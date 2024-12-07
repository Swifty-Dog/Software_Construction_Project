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
        _context.Shipments_items.RemoveRange(_context.Shipments_items);
        _context.SaveChanges();

        var shipment = new Shipment
        {
            Id = 1,
            Order_id = 3,
            Source_id = 52,
            Order_date = new DateOnly(1973, 1, 28),
            Request_date = new DateOnly(1973, 1, 30),
            Shipment_date = new DateOnly(1973, 2, 1),
            Shipment_type = "I",
            Shipment_status = "Pending",
            Notes = "Hoog genot springen afspraak mond bus.",
            Carrier_code = "DHL",
            Carrier_description = "DHL Express",
            Service_code = "NextDay",
            Payment_type = "Automatic",
            Transfer_mode = "Ground",
            Total_package_count = 29,
            Total_package_weight = 463,
            Created_at = DateTime.Parse("1973-01-28T20:09:11Z"),
            Updated_at = DateTime.Parse("1973-01-29T22:09:11Z"),
            Items = new List<Shipments_item>
            {
                new Shipments_item { ItemId = "P010669", Amount = 16 }
            }
        };

        _context.Shipments.Add(shipment);
        _context.SaveChanges();
    }

    [Fact]
    public async Task Test_Get_Shipments()
    {
        var result = await _controller.Get_Shipments();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var shipments = Xunit.Assert.IsType<List<Shipment>>(okResult.Value);
        Xunit.Assert.NotEmpty(shipments);
    }

    [Fact]
    public async Task Test_Get_Shipment_By_Id()
    {
        var result = await _controller.Get_Shipment_By_Id(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var shipment = Xunit.Assert.IsType<Shipment>(okResult.Value);
        Xunit.Assert.Equal(1, shipment.Id);
        Xunit.Assert.Equal(3, shipment.Order_id);
    }

    [Fact]
    public async Task Test_Get_Non_Existent_Shipment()
    {
        var result = await _controller.Get_Shipment_By_Id(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Test_Get_Shipment_Items()
    {
        var result = await _controller.Get_Shipment_Items(1);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var items = Xunit.Assert.IsType<List<Shipments_item>>(okResult.Value);
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
            Order_id = 4,
            Source_id = 61,
            Order_date = new DateOnly(2022, 5, 10),
            Request_date = new DateOnly(2022, 5, 12),
            Shipment_date = new DateOnly(2022, 5, 14),
            Shipment_type = "E",
            Shipment_status = "Delivered",
            Notes = "Shipment created for testing.",
            Carrier_code = "FedEx",
            Carrier_description = "FedEx Ground",
            Service_code = "Standard",
            Payment_type = "Prepaid",
            Transfer_mode = "Air",
            Total_package_count = 10,
            Total_package_weight = 50,
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow,
            Items = new List<Shipments_item>
            {
                new Shipments_item { ItemId = "P020202", Amount = 5 },
                new Shipments_item { ItemId = "P030303", Amount = 5 }
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
            Order_id = 3,
            Source_id = 52,
            Order_date = new DateOnly(1973, 1, 28),
            Request_date = new DateOnly(1973, 1, 30),
            Shipment_date = new DateOnly(1973, 2, 1),
            Shipment_type = "I",
            Shipment_status = "Pending",
            Notes = "Duplicate Shipment",
            Carrier_code = "DHL",
            Carrier_description = "DHL Express",
            Service_code = "NextDay",
            Payment_type = "Automatic",
            Transfer_mode = "Ground",
            Total_package_count = 29,
            Total_package_weight = 463,
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow,
            Items = new List<Shipments_item>
            {
                new Shipments_item { ItemId = "P010669", Amount = 16 }
            }
        };

        var result = await _controller.AddShipment(existingShipment);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Shipment with the same id already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task TestDeleteShipment()
    {
        var result = await _controller.Delete_Shipment(1);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.Get_Shipment_By_Id(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    [Fact]
    public async Task TestDeleteNonExistentShipment()
    {
        var result = await _controller.Delete_Shipment(9999);
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal("Shipment could not be deleted or does not exist.", badRequestResult.Value);
    }
}
