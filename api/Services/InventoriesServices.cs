using Microsoft.EntityFrameworkCore;

public class InventoriesServices : I_Inventories{

    private readonly MyContext _context;
    
    public InventoriesServices(MyContext context){
        _context = context;
    }
    
    public async Task<IEnumerable<Inventory>> Get_Inventories(){
        return await _context.Inventories
            .Include(w => w.Locations)  
            .ToListAsync();
    }

    public async Task<Inventory> Get_Inventory_By_Id(int id){
        if(id <=0)
            return null;
        return await _context.Inventories
                    .Include(t => t.Locations)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<Inventories_locations>> Get_Inventory_Locations(int id){
        if (id <= 0) return null;

        var inventory = await _context.Inventories
                                    .Include(t => t.Locations)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (inventory == null)
            return null;

        return inventory.Locations;
    }

    public async Task<Inventory> Add_Inventory(Inventory inventory){
        if (inventory == null || inventory.Locations == null || !inventory.Locations.Any()){
            throw new ArgumentNullException("Inventory or its Locations list cannot be null.");
        }
        // Get all existing locations that match any of the incoming location IDs
        var checkLocations = await _context.Inventories_Locations
            .Where(i => inventory.Locations.Select(il => il.LocationId).Contains(i.LocationId))
            .ToListAsync();
        // If existing locations are found, replace the incoming locations with the existing ones
        foreach (var location in inventory.Locations){
            var existingLocation = checkLocations.FirstOrDefault(el => el.LocationId == location.LocationId);
            if (existingLocation != null){
                location.LocationId = existingLocation.LocationId;
            }
        }
        // Check if the inventory already exists by checking the Item_reference (since it's unique)
        var existingInventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.Id == inventory.Id);
        if (existingInventory == null){
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            foreach (var location in inventory.Locations){
                location.InventoryId = inventory.Id;
            }
            await _context.SaveChangesAsync();  
            return inventory;
        }

        return null; 
    }

    public async Task<Inventory> Update_Inventory(int id, Inventory inventory){
        if(id <= 0) return null;
        var existingInventory = await Get_Inventory_By_Id(id);
        if (existingInventory == null) return null;
        existingInventory.Item_id = inventory.Item_id;
        existingInventory.Description = inventory.Description;
        existingInventory.Item_reference = inventory.Item_reference;
        existingInventory.Locations = inventory.Locations;
        existingInventory.Total_on_hand = inventory.Total_on_hand;
        existingInventory.Total_expected = inventory.Total_expected;
        existingInventory.Total_ordered = inventory.Total_ordered;
        existingInventory.Total_allocated = inventory.Total_allocated;
        existingInventory.Total_available = inventory.Total_available;
        existingInventory.Created_at = inventory.Created_at;
        existingInventory.Updated_at = inventory.Updated_at;
        await _context.SaveChangesAsync(); 
        return existingInventory;
    }

    public async Task<bool> Delete_Inventory(int id){
        if (id <= 0) return false;
        var existingInventory = await Get_Inventory_By_Id(id);
        if (existingInventory == null) return false;
        _context.Inventories.Remove(existingInventory);
        await _context.SaveChangesAsync();
        return true;
    }
}