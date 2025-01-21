using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OrdersServices : IOrdersInterface
{
    private readonly MyContext _context;

    public OrdersServices(MyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Orders>> GetAll()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();
    }


    public async Task<Orders> GetById(int id)
    {
        if (id <= 0) return null;

        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Orders> AddOrder(Orders order)
    {
        if (order == null || order.Items == null || !order.Items.Any())
            throw new Exception("Order or its Items list cannot be null.");

        var checkOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == order.Id);
        
        if( checkOrder != null)
            throw new Exception("Order already exists.");
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Orders> UpdateOrder(int id, Orders order)
    {
        if (id <= 0 || order == null) return null;

        var orderToUpdate = await _context.Orders.FindAsync(id);
        if (orderToUpdate == null) throw new Exception("Order not found.");

        orderToUpdate.SourceId = order.SourceId;
        orderToUpdate.OrderDate = order.OrderDate;
        orderToUpdate.RequestDate = order.RequestDate;
        orderToUpdate.Reference = order.Reference;
        orderToUpdate.ReferenceExtra = order.ReferenceExtra;
        orderToUpdate.OrderStatus = order.OrderStatus;
        orderToUpdate.Notes = order.Notes;
        orderToUpdate.ShippingNotes = order.ShippingNotes;
        orderToUpdate.PickingNotes = order.PickingNotes;
        orderToUpdate.WarehouseId = order.WarehouseId;
        orderToUpdate.ShipTo = order.ShipTo;
        orderToUpdate.BillTo = order.BillTo;
        orderToUpdate.ShipmentId = order.ShipmentId;
        orderToUpdate.TotalAmount = order.TotalAmount;
        orderToUpdate.TotalDiscount = order.TotalDiscount;
        orderToUpdate.TotalTax = order.TotalTax;
        orderToUpdate.TotalSurcharge = order.TotalSurcharge;
        orderToUpdate.Items = order.Items;
        orderToUpdate.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return orderToUpdate;
    }
    public async Task<bool> DeleteOrder(int id)
    {
        var isOrderUsed = await _context.Shipments.AnyAsync(s => s.OrderId == id);
        if (isOrderUsed)
        {
            throw new Exception("Order is used in a shipment and cannot be deleted.");
        }
        var orderToDelete = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (orderToDelete == null) return false;

        // Remove related items first
        _context.OrdersItems.RemoveRange(orderToDelete.Items);

        _context.Orders.Remove(orderToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}
