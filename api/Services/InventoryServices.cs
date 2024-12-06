using Microsoft.EntityFrameworkCore;

public class InventoryServices : IInventory{

    private readonly MyContext _context;
    
    public InventoryServices(MyContext context){
        _context = context;
    }
    
    public async Task<IEnumerable<Inventory>> GetInventories(){
        return await _context.Inventories
            .Include(w => w.Locations)  
            .ToListAsync();
    }

    public async Task<Inventory> GetInventoryById(int id){
        if(id <=0)
            return null;
        return await _context.Inventories
                    .Include(t => t.Locations)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<InventoriesLocations>> GetInventoryLocations(int id){
        if (id <= 0) return null;

        var inventory = await _context.Inventories
                                    .Include(t => t.Locations)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (inventory == null)
            return null;

        return inventory.Locations;
    }

    public async Task<Inventory> AddInventory(Inventory inventory){
        if (inventory == null || inventory.Locations == null || !inventory.Locations.Any()){
            throw new ArgumentNullException("Inventory or its locations list cannot be null.");
        }
        // Get all existing locations that match any of the incoming location IDs
        var checkLocations = await _context.InventoriesLocations
            .Where(i => inventory.Locations.Select(il => il.LocationId).Contains(i.LocationId))
            .ToListAsync();
        // If existing locations are found, replace the incoming locations with the existing ones
        foreach (var location in inventory.Locations){
            var existingLocation = checkLocations.FirstOrDefault(el => el.LocationId == location.LocationId);
            if (existingLocation != null){
                location.LocationId = existingLocation.LocationId;
            }
        }
        // Check if the inventory already exists by checking the itemReference (since it's unique)
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

    public async Task<Inventory> UpdateInventory(int id, Inventory inventory){
        if(id <= 0) return null;
        var existingInventory = await GetInventoryById(id);
        if (existingInventory == null) return null;
        existingInventory.ItemId = inventory.ItemId;
        existingInventory.Description = inventory.Description;
        existingInventory.ItemReference = inventory.ItemReference;
        existingInventory.Locations = inventory.Locations;
        existingInventory.TotalOnHand = inventory.TotalOnHand;
        existingInventory.TotalExpected = inventory.TotalExpected;
        existingInventory.TotalOrdered = inventory.TotalOrdered;
        existingInventory.TotalAllocated = inventory.TotalAllocated;
        existingInventory.TotalAvailable = inventory.TotalAvailable;
        existingInventory.CreatedAt = inventory.CreatedAt;
        existingInventory.UpdatedAt = inventory.UpdatedAt;
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