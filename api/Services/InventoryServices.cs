using Microsoft.EntityFrameworkCore;

public class InventoryServices
{
    private readonly MyContext _context;

    public InventoryServices(MyContext context)
    {
        _context = context;
    }

    public async Task<Inventory> AddInventory(Inventory inventory)
    {
        var checkLocations = await _context.InventoriesLocations
            .Where(il => il.InventoryId == inventory.Id)
            .ToListAsync();

        // If existing locations are found, replace the incoming locations with the existing ones
        foreach (var location in inventory.Locations)
        {
            var existingLocation = checkLocations.FirstOrDefault(el => el.LocationId == location.LocationId);
            if (existingLocation != null)
            {
                location.LocationId = existingLocation.LocationId;
            }
        }

        // Check if the inventory already exists by checking the ItemReference (since it's unique)
        var existingInventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.Id == inventory.Id);
        if (existingInventory == null)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            foreach (var location in inventory.Locations)
            {
                location.InventoryId = inventory.Id;
            }
            await _context.SaveChangesAsync();
            return inventory;
        }

        return null;
    }

    public async Task<Inventory> UpdateInventory(int id, Inventory inventory)
    {
        if (id <= 0) return null;
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

    public async Task<bool> DeleteInventory(int id)
    {
        var inventory = await GetInventoryById(id);
        if (inventory == null) return false;
        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Inventory> GetInventoryById(int id)
    {
        return await _context.Inventories
            .Include(i => i.Locations)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}