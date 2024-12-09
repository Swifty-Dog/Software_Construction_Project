using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TransfersTest
{
    private readonly MyContext _context;
    private readonly TransfersController _controller;

    public TransfersTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "TransfersTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new TransfersServices(_context); // Initialize the service with the context
        _controller = new TransfersController(service); // Pass the service to the controller
    }
    private void ClearData()
    {
        _context.Set<TransfersItem>().RemoveRange(_context.Set<TransfersItem>());
        _context.Transfers.RemoveRange(_context.Transfers);
        _context.SaveChanges();
    }      

    private void SeedData()
    {
        ClearData();
        _context.Transfers.RemoveRange(_context.Transfers);
        _context.SaveChanges();

        var transfer1 = new Transfer
        {
            Id = 1,
            Reference = "TR12344",
            TransferFrom = 1234,
            TransferTo = 5678,
            TransferStatus = "completed",
            CreatedAt = DateTime.Parse("2021-08-01T00:00:00"),
            UpdatedAt = DateTime.Parse("2021-08-02T00:00:00"),
            Items = new List<TransfersItem>
            {
                new TransfersItem
                {
                    ItemId = "P007434",
                    Amount = 1
                },
                new TransfersItem
                {
                    ItemId = "P007435",
                    Amount = 2
                }
            }
        };
        var transfer2 = new Transfer
        {
            Id = 2,
            Reference = "TR12345",
            TransferFrom = 2323,
            TransferTo = 9299,
            TransferStatus = "pending",
            CreatedAt = DateTime.Parse("2021-09-01T00:00:00"),
            UpdatedAt = DateTime.Parse("2021-10-01T00:00:00"),
            Items = new List<TransfersItem>
            {
                new TransfersItem
                {
                    ItemId = "P007435",
                    Amount = 2
                },
                new TransfersItem
                {
                    ItemId = "P007436",
                    Amount = 3
                }
            }
        };

        _context.Transfers.AddRange(transfer1, transfer2);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetTransfers()
    {
        var result = await _controller.GetTransfers();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var transfers = Xunit.Assert.IsType<List<Transfer>>(okResult.Value);
        Xunit.Assert.NotEmpty(transfers);
    }

    [Fact]
    public async Task TestGetTransferById()
    {
        var result = await _controller.GetTransferById(2);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var transfer = Xunit.Assert.IsType<Transfer>(okResult.Value);
        Xunit.Assert.Equal("TR12345", transfer.Reference);
        Xunit.Assert.Equal(2, transfer.Id);
    }

    [Fact]
    public async Task TestGetNonExistentTransfer()
    {
        var result = await _controller.GetTransferById(999);
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostTransfer()
    {
        var newTransfer = new Transfer
        {
            Id = 3,
            Reference = "TR1267",
            TransferFrom  = 1000,
            TransferTo  = 2000,
            TransferStatus = "completed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = new List<TransfersItem>
            {
                new TransfersItem
                {
                    ItemId = "P007437",
                    Amount = 1
                }
            }
        };

        var result = await _controller.AddTransfer(newTransfer);
        var okResult = Xunit.Assert.IsType<CreatedAtActionResult>(result);
        var transfer = Xunit.Assert.IsType<Transfer>(okResult.Value);
        Xunit.Assert.Equal("TR1267", transfer.Reference);
    }

    [Fact]
    public async Task TestDeleteTransfer()
    {
        var result = await _controller.DeleteTransfer(1);
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetTransferById(1);
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }
}
