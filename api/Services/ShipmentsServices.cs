using Microsoft.EntityFrameworkCore;

public class ShipmentsServices : IShipments
{
    private readonly MyContext _context;
    
    public ShipmentsServices(MyContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Shipment>> GetShipments()
    {
        return await _context.Shipments
            .Include(w => w.Items)  
            .ToListAsync();
    }

    public async Task<Shipment> GetShipmentById(int id)
    {

        if (id <=0)
        {
            return null;
        }
        return await _context.Shipments
                    .Include(t => t.Items)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<ShipmentsItem>> GetShipmentItems(int id)
    {
        if (id <= 0) return null;

        var shipment = await _context.Shipments
                                    .Include(t => t.Items)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (shipment == null)
        {
            return null;
        }

        return shipment.Items;
    }

    public async Task<Shipment> AddShipment(Shipment shipment)
    {
        if (shipment == null || shipment.Items == null || !shipment.Items.Any())
        {
            throw new ArgumentNullException("Shipment or its Items list cannot be null.");
        }
        // Get all existing items that match any of the incoming item IDs
        var checkItems = await _context.ShipmentsItems
            .Where(i => shipment.Items.Select(si => si.ItemId).Contains(i.ItemId))
            .ToListAsync();
        // If existing items are found, replace the incoming items with the existing ones
        foreach (var item in shipment.Items)
        {
            var existingItem = checkItems.FirstOrDefault(ei => ei.ItemId == item.ItemId);
            if (existingItem != null)
            {
                item.ItemId = existingItem.ItemId;
                item.Amount = existingItem.Amount;
            }
        }
        // Check if the shipment already exists by checking the Reference (since it's unique)
        var existingShipment = await _context.Shipments
            .FirstOrDefaultAsync(t => t.Id == shipment.Id);
        if (existingShipment == null)
        {
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            foreach (var item in shipment.Items)
            {
                item.ShippingId = shipment.Id;
            }
            await _context.SaveChangesAsync();  
            return shipment;
        }

        return null; 
    }

    public async Task<Shipment> UpdateShipment(int id, Shipment shipment)
    {
        if (id <= 0) return null;
        var existingShipment = await GetShipmentById(id);
        if (existingShipment == null) return null;
        existingShipment.OrderId = shipment.OrderId;
        existingShipment.SourceId = shipment.SourceId;
        existingShipment.OrderDate = shipment.OrderDate;
        existingShipment.RequestDate = shipment.RequestDate;
        existingShipment.ShipmentDate = shipment.ShipmentDate;
        existingShipment.ShipmentType = shipment.ShipmentType;
        existingShipment.ShipmentStatus = shipment.ShipmentStatus;
        existingShipment.Notes = shipment.Notes;
        existingShipment.CarrierCode = shipment.CarrierCode;
        existingShipment.CarrierDescription = shipment.CarrierDescription;
        existingShipment.ServiceCode = shipment.ServiceCode;
        existingShipment.PaymentType = shipment.PaymentType;
        existingShipment.TransferMode = shipment.TransferMode;
        existingShipment.TotalPackageCount = shipment.TotalPackageCount;
        existingShipment.TotalPackageWeight = shipment.TotalPackageWeight;
        existingShipment.CreatedAt = shipment.CreatedAt;
        existingShipment.UpdatedAt = shipment.UpdatedAt;
        existingShipment.Items = shipment.Items;
        await _context.SaveChangesAsync(); 
        return existingShipment;
    }

    public async Task<bool> DeleteShipment(int id)
    {
        if (id <= 0) return false;
        var existingShipment = await GetShipmentById(id);
        if (existingShipment == null) return false;
        _context.Shipments.Remove(existingShipment);
        await _context.SaveChangesAsync();
        return true;
    }
}
