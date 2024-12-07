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
        _controller = new ItemController(service);
        var lineService = new ItemLineServices(_context);
        _controllerLines = new ItemLinesController(lineService);
        var typeService = new Item_TypeServices(_context);
        _controllerTypes = new ItemTypeController(typeService);
        var groupService = new Item_groupService(_context);
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
            Short_Description = "must",
            Upc_code = "6523540947122",
            Model_number = "63-OFFTq0T",
            Commodity_code = "oTo304",
            Item_line = 11,
            Item_group = 73,
            item_type = 14,
            unit_purchase_quantity = 47,
            unit_order_quantity = 13,
            pack_order_quantity = 11,
            supplier_id = 34,
            supplier_code = "SUP423",
            supplier_part_number = "E-86805-uTM",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        _context.Items.Add(item);
        _context.SaveChanges();
    }

    [Fact]
    public async Task TestGetItems()
    {
        var result = await _controller.Get_Items();
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var items = Xunit.Assert.IsType<List<Item>>(okResult.Value);
        Xunit.Assert.NotEmpty(items);
    }

    [Fact]
    public async Task TestGetItemById()
    {
        var result = await _controller.Get_Item_By_Id("P000002");
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("Face-to-face clear-thinking complexity", item.Description);
        Xunit.Assert.Equal("P000002", item.Uid);
    }

    [Fact]
    public async Task TestGetNonExistentItem()
    {
        var result = await _controller.Get_Item_By_Id("abcdefg");
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

        var newItemType = new Item_type
        {
            Id = 1,
            Name = "New Gadgets type",
            Description = "stuff",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerTypes.AddItem_types(newItemType);

        var newItemGroup = new Item_group
        {
            Id = 1,
            Name = "Test group",
            Description = "Testtest",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerGroups.Add_Item_group(newItemGroup);

        var newSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP003",
            Name = "Tech Supplies",
            Address = "123 Tech Park",
            Address_extra = "Suite 101",
            Zip_code = "67890",
            Province = "California",
            Country = "USA",
            Contact_name = "Jane Doe",
            Phonenumber = "098-765-4321",
            Email = "janedoe@techsupplies.com",
            Reference = "REF1234",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerSuppliers.AddSupplier(newSupplier);


        var newItem = new Item
        {
            Uid = "test",
            Code = "test",
            Description = "test test",
            Short_Description = "test",
            Upc_code = "6523540947122",
            Model_number = "63-OFFTq0T",
            Commodity_code = "oTo304",
            Item_line = 1,
            Item_group = 1,
            item_type = 1,
            unit_purchase_quantity = 47,
            unit_order_quantity = 13,
            pack_order_quantity = 11,
            supplier_id = 1,
            supplier_code = "SUP423",
            supplier_part_number = "E-86805-uTM",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.Add_Item(newItem);
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

        var newItemType = new Item_type
        {
            Id = 1,
            Name = "New Gadgets type",
            Description = "stuff",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerTypes.AddItem_types(newItemType);

        var newItemGroup = new Item_group
        {
            Id = 1,
            Name = "Test group",
            Description = "Testtest",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerGroups.Add_Item_group(newItemGroup);

        var newSupplier = new Supplier
        {
            Id = 1,
            Code = "SUPP003",
            Name = "Tech Supplies",
            Address = "123 Tech Park",
            Address_extra = "Suite 101",
            Zip_code = "67890",
            Province = "California",
            Country = "USA",
            Contact_name = "Jane Doe",
            Phonenumber = "098-765-4321",
            Email = "janedoe@techsupplies.com",
            Reference = "REF1234",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };
        await _controllerSuppliers.AddSupplier(newSupplier);


        var updatedItem = new Item
        {
            Uid = "P000002",
            Code = "test updated",
            Description = "test update test",
            Short_Description = "test",
            Upc_code = "6523540947122",
            Model_number = "63-OFFTq0T",
            Commodity_code = "oTo304",
            Item_line = 1,
            Item_group = 1,
            item_type = 1,
            unit_purchase_quantity = 47,
            unit_order_quantity = 13,
            pack_order_quantity = 11,
            supplier_id = 1,
            supplier_code = "SUP423",
            supplier_part_number = "E-86805-uTM",
            Created_at = DateTime.UtcNow,
            Updated_at = DateTime.UtcNow
        };

        var result = await _controller.Update_Item("P000002", updatedItem);
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var item = Xunit.Assert.IsType<Item>(okResult.Value);
        Xunit.Assert.Equal("test update test", item.Description);
        Xunit.Assert.Equal("test updated", item.Code);
    }

    [Fact]
    public async Task TestDeleteItem()
    {
        var result = await _controller.Delete_Item("P000002");
        Xunit.Assert.IsType<NoContentResult>(result);

        var getResult = await _controller.Get_Item_By_Id("P000002");
        Xunit.Assert.IsType<NotFoundObjectResult>(getResult);
    }

    
}