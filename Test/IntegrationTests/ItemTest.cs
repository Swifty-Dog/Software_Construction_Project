using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ItemTest
{
    private readonly MyContext _context;
    private readonly ItemController _controller;
    private readonly ItemLinesController _controllerLines;
    private readonly ItemTypeController _controllerTypes;
    private readonly ItemGroupController _controllerGroups;
    private readonly SuppliersController _controllerSuppliers;    

    public ItemTest()
    {
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(databaseName: "ItemTest")
            .Options;

        _context = new MyContext(options);
        SeedData();

        var service = new ItemServices(_context);
        _controller = new ItemController(service, null);
        var lineService = new ItemLineServices(_context);
        _controllerLines = new ItemLinesController(lineService);
        var typeService = new ItemTypeServices(_context);
        _controllerTypes = new ItemTypeController(typeService);
        var groupService = new ItemGroupService(_context);
        _controllerGroups = new ItemGroupController(groupService);
        var supplierService = new SuppliersServices(_context);
        _controllerSuppliers = new SuppliersController(supplierService);

    }

    private void SeedData()
    {
        _context.Items.RemoveRange(_context.Items);
        _context.SaveChanges();

        var item = new Item
        {
            Uid = "P000002",
            Code = "sjQ23408K",
            Description = "Face-to-face clear-thinking complexity",
            ShortDescription = "must",
            UpcCode = "6523540947122",
            ModelNumber = "63-OFFTq0T",
            CommodityCode = "oTo304",
            ItemLine = 11,
            ItemGroup = 73,
            ItemType = 14,
            UnitPurchaseQuantity = 47,
            UnitOrderQuantity = 13,
            PackOrderQuantity = 11,
            SupplierId = 34,
            SupplierCode = "SUP423",
            SupplierPartNumber = "E-86805-uTM",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Items.Add(item);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItems()
    {
        var result = await _controller.GetItems();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var items = Xunit.Assert.IsType<List<Item>>(okResult.Value);
        Xunit.Assert.NotEmpty(items);
    }

    [Fact]
    public async Task TestGetItemById()
    {
        var result = await _controller.GetItemById("P000002");
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("Face-to-face clear-thinking complexity", item.Description);
        Xunit.Assert.Equal("P000002", item.Uid);
    }

    [Fact]
    public async Task TestGetNonExistentItem()
    {
        var result = await _controller.GetItemById("abcdefg");
        Xunit.Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task TestPostItem()
    {
        var newItemLine = new ItemLine
        {
            Id = 1,
            Name = "New Gadgets",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerLines.AddItemLine(newItemLine);

        var newItemType = new ItemType
        {
            Id = 1,
            Name = "New Gadgets type",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerTypes.AddItemType(newItemType);

        var newItemGroup = new ItemGroup
        {
            Id = 1,
            Name = "Test group",
            Description = "Testtest",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerGroups.AddItemGroup(newItemGroup);

        var newSupplier = new Supplier
        {
            Id = 1,
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
        await _controllerSuppliers.AddSupplier(newSupplier);


        var newItem = new Item
        {
            Uid = "test",
            Code = "test",
            Description = "test test",
            ShortDescription = "test",
            UpcCode = "6523540947122",
            ModelNumber = "63-OFFTq0T",
            CommodityCode = "oTo304",
            ItemLine = 1,
            ItemGroup = 1,
            ItemType = 1,
            UnitPurchaseQuantity = 47,
            UnitOrderQuantity = 13,
            PackOrderQuantity = 11,
            SupplierId = 1,
            SupplierCode = "SUP423",
            SupplierPartNumber = "E-86805-uTM",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.AddItem(newItem);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("test test", item.Description);
        Xunit.Assert.Equal("test", item.Code);
    }

    [Fact]
    public async Task TestPutItem()
    {
        var newItemLine = new ItemLine
        {
            Id = 1,
            Name = "New Gadgets",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerLines.AddItemLine(newItemLine);

        var newItemType = new ItemType
        {
            Id = 1,
            Name = "New Gadgets type",
            Description = "stuff",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerTypes.AddItemType(newItemType);

        var newItemGroup = new ItemGroup
        {
            Id = 1,
            Name = "Test group",
            Description = "Testtest",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _controllerGroups.AddItemGroup(newItemGroup);

        var newSupplier = new Supplier
        {
            Id = 1,
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
        await _controllerSuppliers.AddSupplier(newSupplier);


        var updatedItem = new Item
        {
            Uid = "P000002",
            Code = "test updated",
            Description = "test update test",
            ShortDescription = "test",
            UpcCode = "6523540947122",
            ModelNumber = "63-OFFTq0T",
            CommodityCode = "oTo304",
            ItemLine = 1,
            ItemGroup = 1,
            ItemType = 1,
            UnitPurchaseQuantity = 47,
            UnitOrderQuantity = 13,
            PackOrderQuantity = 11,
            SupplierId = 1,
            SupplierCode = "SUP423",
            SupplierPartNumber = "E-86805-uTM",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _controller.UpdateItem("P000002", updatedItem);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("test update test", item.Description);
        Xunit.Assert.Equal("test updated", item.Code);
    }

    [Fact]
    public async Task TestDeleteItem()
    {
        var result = await _controller.DeleteItem("P000002");
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.GetItemById("P000002");
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    
}