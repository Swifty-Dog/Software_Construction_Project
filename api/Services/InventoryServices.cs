using Microsoft.EntityFrameworkCore;

public class InventoryServices : IInventory{

    private readonly MyContext _context;
    
    public InventoryServices(MyContext context){
        _context = context;
    }
    
    public async Task<IEnumerable<Inventory>> GetInventories(){
        return await _context.Inventories
            .Include(w => w.locations)  
            .ToListAsync();
    }

    public async Task<Inventory> GetInventoryById(int id){
        if(id <=0)
            return null;
        return await _context.Inventories
                    .Include(t => t.locations)  
                    .FirstOrDefaultAsync(t => t.id == id);
    }
    public async Task<List<InventoriesLocations>> GetInventoryLocations(int id){
        if (id <= 0) return null;

        var inventory = await _context.Inventories
                                    .Include(t => t.locations)  
                                    .FirstOrDefaultAsync(t => t.id == id);
        if (inventory == null)
            return null;

        return inventory.locations;
    }

    public async Task<Inventory> AddInventory(Inventory inventory){
        if (inventory == null || inventory.locations == null || !inventory.locations.Any()){
            throw new ArgumentNullException("Inventory or its locations list cannot be null.");
        }
        // Get all existing locations that match any of the incoming location IDs
        var checkLocations = await _context.Inventories_Locations
            .Where(i => inventory.locations.Select(il => il.locationId).Contains(i.locationId))
            .ToListAsync();
        // If existing locations are found, replace the incoming locations with the existing ones
        foreach (var location in inventory.locations){
            var existingLocation = checkLocations.FirstOrDefault(el => el.locationId == location.locationId);
            if (existingLocation != null){
                location.locationId = existingLocation.locationId;
            }
        }
        // Check if the inventory already exists by checking the itemReference (since it's unique)
        var existingInventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.id == inventory.id);
        if (existingInventory == null){
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            foreach (var location in inventory.locations){
                location.inventoryId = inventory.id;
            }
            await _context.SaveChangesAsync();  
            return inventory;
        }

        return null; 
    }

    public async Task<Inventory> UpdateInventory(int id, Inventory inventory){
        if(id <= 0) return null;
        var existingInventory = await GetInventoryById(id);
        if (existingInventory == null) return null;
        existingInventory.itemId = inventory.itemId;
        existingInventory.description = inventory.description;
        existingInventory.itemReference = inventory.itemReference;
        existingInventory.locations = inventory.locations;
        existingInventory.totalOnHand = inventory.totalOnHand;
        existingInventory.totalExpected = inventory.totalExpected;
        existingInventory.totalOrdered = inventory.totalOrdered;
        existingInventory.totalAllocated = inventory.totalAllocated;
        existingInventory.totalAvailable = inventory.totalAvailable;
        existingInventory.createdAt = inventory.createdAt;
        existingInventory.updatedAt = inventory.updatedAt;
        await _context.SaveChangesAsync(); 
        return existingInventory;
    }

    public async Task<bool> DeleteInventory(int id){
        if (id <= 0) return false;
        var existingInventory = await GetInventoryById(id);
        if (existingInventory == null) return false;
        _context.Inventories.Remove(existingInventory);
        await _context.SaveChangesAsync();
        return true;
    }
}