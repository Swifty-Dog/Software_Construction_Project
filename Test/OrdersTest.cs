using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OrdersTest{

public readonly MyContext _context;
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

    var orders = new List<Orders>
    {
        new Orders
        {  
            id: 2,  
            source_id: 1,  
            "order_date": "2024-10-22T10:00:00",  
            "request_date": "2024-10-23T10:00:00",  
            "reference": "ORD1234",  
            "reference_extra": "ORD_EXTRA_001",  
            "order_status": "Pending",  
            "notes": "Please process as soon as possible.",  
            "shipping_notes": "Fragile items.",  
            "picking_notes": "Pick items in the correct order.",  
            "warehouse_id": 1,  
            "ship_to": "456 Delivery St, Tech City, CA",  
            "bill_to": "789 Billing Rd, Tech City, CA",  
            "shipment_id": 101,  
            "total_amount": 1000.00,  
            "total_discount": 50.00,  
            "total_tax": 50.00,  
            "total_surcharge": 10.00,  
            "items": [  
                {  
                    "uid": "P000001",  
                    "code": "sjQ23408K",  
                    "description": "Face-to-face clear-thinking complexity",  
                    "short_description": "must",  
                    "upc_code": "6523540947122",  
                    "model_number": "63-OFFTq0T",  
                    "commodity_code": "oTo304",  
                    "item_line": 11,  
                    "item_group": 73  
                }  
            ],  
            "created_at": "2024-10-22T10:00:00",  
            "updated_at": "2024-10-22T10:00:00"  
        }
    }

}



}