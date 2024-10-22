using Microsoft.EntityFrameworkCore;

public class ShipmentsServices : IShipments{

    private readonly MyContext _context;
    
    public ShipmentsServices(MyContext context){
        _context = context;
    }
    
    public async Task<IEnumerable<Shipment>> Get_Shipments(){
        return await _context.Shipments
            .Include(w => w.Items)  
            .ToListAsync();
    }

    public async Task<Shipment> Get_Shipment_By_Id(int id){
        if(id <=0)
            return null;
        return await _context.Shipments
                    .Include(t => t.Items)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<Shipments_item>> Get_Shipment_Items(int id){
        if (id <= 0) return null;

        var shipment = await _context.Shipments
                                    .Include(t => t.Items)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (shipment == null)
            return null;

        return shipment.Items;
    }

    public async Task<Shipment> Add_Shipment(Shipment shipment){
        if (shipment == null || shipment.Items == null || !shipment.Items.Any()){
            throw new ArgumentNullException("Shipment or its Items list cannot be null.");
        }
        // Get all existing items that match any of the incoming item IDs
        var checkItems = await _context.Shipments_items
            .Where(i => shipment.Items.Select(si => si.ItemId).Contains(i.ItemId))
            .ToListAsync();
        // If existing items are found, replace the incoming items with the existing ones
        foreach (var item in shipment.Items){
            var existingItem = checkItems.FirstOrDefault(ei => ei.ItemId == item.ItemId);
            if (existingItem != null){
                item.ItemId = existingItem.ItemId;
                item.Amount = existingItem.Amount;
            }
        }
        // Check if the shipment already exists by checking the Reference (since it's unique)
        var existingShipment = await _context.Shipments
            .FirstOrDefaultAsync(t => t.Id == shipment.Id);
        if (existingShipment == null){
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            foreach (var item in shipment.Items){
                item.ShippingId = shipment.Id;
            }
            await _context.SaveChangesAsync();  
            return shipment;
        }

        return null; 
    }

    public async Task<Shipment> Update_Shipment(int id, Shipment shipment){
        if(id <= 0) return null;
        var existingShipment = await Get_Shipment_By_Id(id);
        if (existingShipment == null) return null;
        existingShipment.Order_id = shipment.Order_id;
        existingShipment.Source_id = shipment.Source_id;
        existingShipment.Order_date = shipment.Order_date;
        existingShipment.Request_date = shipment.Request_date;
        existingShipment.Shipment_date = shipment.Shipment_date;
        existingShipment.Shipment_type = shipment.Shipment_type;
        existingShipment.Shipment_status = shipment.Shipment_status;
        existingShipment.Notes = shipment.Notes;
        existingShipment.Carrier_code = shipment.Carrier_code;
        existingShipment.Carrier_description = shipment.Carrier_description;
        existingShipment.Service_code = shipment.Service_code;
        existingShipment.Payment_type = shipment.Payment_type;
        existingShipment.Transfer_mode = shipment.Transfer_mode;
        existingShipment.Total_package_count = shipment.Total_package_count;
        existingShipment.Total_package_weight = shipment.Total_package_weight;
        existingShipment.Created_at = shipment.Created_at;
        existingShipment.Updated_at = shipment.Updated_at;
        existingShipment.Items = shipment.Items;
        await _context.SaveChangesAsync(); 
        return existingShipment;
    }

    public async Task<bool> Delete_Shipment(int id){
        if (id <= 0) return false;
        var existingShipment = await Get_Shipment_By_Id(id);
        if (existingShipment == null) return false;
        _context.Shipments.Remove(existingShipment);
        await _context.SaveChangesAsync();
        return true;
    }
}