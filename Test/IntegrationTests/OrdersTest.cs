using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;

public class OrdersTest
{
    private readonly MyContext _context;
    private readonly OrdersController _controller;

    public OrdersTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "OrdersTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new OrdersServices(_context);
        _controller = new OrdersController(service);
    }

    private void SeedData()
    {
        _context.Orders.RemoveRange(_context.Orders);
        _context.SaveChanges();

        var order = new Orders
        {
            Id = 2,
            SourceId = 1,
            OrderDate = DateTime.Parse("2024-10-22T10:00:00"),
            RequestDate = DateTime.Parse("2024-10-23T10:00:00"),
            Reference = "ORD1234",
            ReferenceExtra = "ORD_EXTRA_001",
            OrderStatus = "Pending",
            Notes = "Edited. New notes for the order.",
            ShippingNotes = "Fragile items.",
            PickingNotes = "Pick items in the correct order.",
            WarehouseId = 1,
            ShipTo = 123,
            BillTo = 456,
            ShipmentId = 101,
            TotalAmount = 1050.00m,
            TotalDiscount = 60.00m,
            TotalTax = 55.00m,
            TotalSurcharge = 12.00m,
            Items = new List<OrdersItem>
            {
                new OrdersItem
                {
                    Uid = "P000001",
                    Code = "sjQ23408K",
                    Description = "Updated description for the item",
                    ShortDescription = "Updated brief description",
                    UpcCode = "6523540947122",
                    ModelNumber = "63-OFFTq0T",
                    CommodityCode = "oTo304",
                    ItemLine = 11,
                    ItemGroup = 6,
                }
            },
            CreatedAt = DateTime.Parse("2024-10-22T10:00:00"),
            UpdatedAt = DateTime.Parse("2024-10-22T12:00:00")
        };

        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetOrders()
    {
        var result = await _controller.GetOrders();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var orders = Xunit.Assert.IsType<List<Orders>>(okResult.Value);
        Xunit.Assert.NotEmpty(orders);
    }

    [Fact]
    public async Task TestGetOrderById()
    {
        var result = await _controller.GetOrderById(2);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var order = Xunit.Assert.IsType<Orders>(okResult.Value);
        Xunit.Assert.Equal("ORD1234", order.Reference);
        Xunit.Assert.Equal(2, order.Id);
    }

    [Fact]
    public async Task TestGetNonExistentOrder()
    {
        var result = await _controller.GetOrderById(9999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostOrder()
    {
        var newOrder = new Orders
        {
            Id = 3,
            SourceId = 1,
            OrderDate = DateTime.UtcNow,
            RequestDate = DateTime.UtcNow.AddDays(1),
            Reference = "ORD5678",
            ReferenceExtra = "ORD_EXTRA_002",
            OrderStatus = "Pending",
            Notes = "Test order.",
            ShippingNotes = "Handle with care.",
            PickingNotes = "Pick items in the right sequence.",
            WarehouseId = 1,
            ShipTo = 789,
            BillTo = 1011,
            ShipmentId = 102,
            TotalAmount = 1200.00m,
            TotalDiscount = 70.00m,
            TotalTax = 60.00m,
            TotalSurcharge = 15.00m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = new List<OrdersItem>
            {
                new OrdersItem
                {
                    Uid = "P000002",
                    Code = "sjQ23409K",
                    Description = "Updated description for the item",
                    ShortDescription = "Brief description",
                    UpcCode = "6523540947123",
                    ModelNumber = "63-OFFTq0U",
                    CommodityCode = "oTo305",
                    ItemLine = 12,
                    ItemGroup = 74,
                },
            },
        };

        var result = await _controller.AddOrder(newOrder);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var order = Xunit.Assert.IsType<Orders>(okResult.Value);
        Xunit.Assert.Equal("ORD5678", order.Reference);
        Xunit.Assert.Equal("Test order.", order.Notes);
    }

    [Fact]
    public async Task TestPutOrder()
    {
        var updatedOrder = new Orders
        {
            Id = 2,
            SourceId = 1,
            OrderDate = DateTime.Parse("2024-10-22T10:00:00"),
            RequestDate = DateTime.Parse("2024-10-23T10:00:00"),
            Reference = "ORD1234_UPDATED",
            ReferenceExtra = "ORD_EXTRA_001_UPDATED",
            OrderStatus = "Processed",
            Notes = "Updated order.",
            ShippingNotes = "Handle with extra care.",
            PickingNotes = "Ensure items are picked correctly.",
            WarehouseId = 1,
            ShipTo = 123,
            BillTo = 456,
            ShipmentId = 101,
            TotalAmount = 1100.00m,
            TotalDiscount = 60.00m,
            TotalTax = 55.00m,
            TotalSurcharge = 12.00m,
            Items = new List<OrdersItem>
            {
                new OrdersItem
                {
                    Uid = "P000001",
                    Code = "sjQ23408K_UPDATED",
                    Description = "Updated description for the item",
                    ShortDescription = "Updated brief description",
                    UpcCode = "6523540947122",
                    ModelNumber = "63-OFFTq0T",
                    CommodityCode = "oTo304",
                    ItemLine = 11,
                    ItemGroup = 73
                }
            },
            CreatedAt = DateTime.Parse("2024-10-22T10:00:00"),
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateOrder(2, updatedOrder);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var order = Xunit.Assert.IsType<Orders>(okResult.Value);
        Xunit.Assert.Equal("ORD1234_UPDATED", order.Reference);
        Xunit.Assert.Equal("Updated order.", order.Notes);
    }

    [Fact]
    public async Task TestDeleteOrder()
    {
        var result = await _controller.DeleteOrder(2);
        Xunit.Assert.IsType<OkObjectResult>(result);

        var getResult = await _controller.GetOrderById(2);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}
