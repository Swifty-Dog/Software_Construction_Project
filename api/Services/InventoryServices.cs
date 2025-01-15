using Microsoft.EntityFrameworkCore;

public class InventoryServices : IInventory
{
    private readonly MyContext _context;
    
    public InventoryServices(MyContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Inventory>> GetInventories()
    {
        return await _context.Inventories.ToListAsync();
    }

    public async Task<Inventory> GetInventoryById(int id)
    {
        if (id <= 0) return null;

        return await _context.Inventories.FirstOrDefaultAsync(t => t.Id == id);
    }

    //public async Task<List<Locations>> GetInventoryLocations(int id)
    public async Task<List<int>> GetInventoryLocations(int id)
    {
        if (id <= 0) return null;

        var inventory = await _context.Inventories.FirstOrDefaultAsync(t => t.Id == id);
        /*
        if (inventory == null) return null;

        // Fetch detailed location information based on the location IDs in the inventory
        return await _context.Locations
            .Where(l => inventory.Locations.Contains(l.Id))
            .ToListAsync();
        */
        return inventory?.Locations;
    }

    public async Task<Inventory> AddInventory(Inventory inventory)
    {
        if (inventory == null || !inventory.Locations.Any())
        {
            throw new ArgumentNullException("Inventory or its locations list cannot be null.");
        }

        // Check if the inventory already exists
        var existingInventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventory.Id);
        if (existingInventory != null) return null;

        // Ensure all location IDs exist in the database
        var validLocations = await _context.Locations
            .Where(l => inventory.Locations.Contains(l.Id))
            .Select(l => l.Id)
            .ToListAsync();

        if (validLocations.Count != inventory.Locations.Count)
        {
            throw new ArgumentException("One or more location IDs are invalid.");
        }

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();

        return inventory; 
    }

    public async Task<Inventory> UpdateInventory(int id, Inventory inventory)
    {
        if (id <= 0) return null;

        var existingInventory = await GetInventoryById(id);
        if (existingInventory == null) return null;

        existingInventory.ItemId = inventory.ItemId;
        existingInventory.Description = inventory.Description;
        existingInventory.ItemReference = inventory.ItemReference;
        existingInventory.TotalOnHand = inventory.TotalOnHand;
        existingInventory.TotalExpected = inventory.TotalExpected;
        existingInventory.TotalOrdered = inventory.TotalOrdered;
        existingInventory.TotalAllocated = inventory.TotalAllocated;
        existingInventory.TotalAvailable = inventory.TotalAvailable;
        existingInventory.CreatedAt = inventory.CreatedAt;
        existingInventory.UpdatedAt = inventory.UpdatedAt;

        // Validate and update locations
        var validLocations = await _context.Locations
            .Where(l => inventory.Locations.Contains(l.Id))
            .Select(l => l.Id)
            .ToListAsync();

        if (validLocations.Count != inventory.Locations.Count)
        {
            throw new ArgumentException("One or more location IDs are invalid.");
        }

        existingInventory.Locations = validLocations;
        await _context.SaveChangesAsync();

        return existingInventory;
    }

    public async Task<bool> DeleteInventory(int id)
    {
        if (id <= 0) return false;

        var existingInventory = await GetInventoryById(id);
        if (existingInventory == null) return false;

        _context.Inventories.Remove(existingInventory);
        await _context.SaveChangesAsync();

        return true;
    }
}