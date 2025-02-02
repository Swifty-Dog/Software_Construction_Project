// using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class WarehouseServices: IWarehouse
{
    private readonly MyContext _context;
    public WarehouseServices(MyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehouses()
    {
        return await _context.Warehouse
            .Include(w => w.Contact)
            .Include(l => l.Locations)  // Eagerly load the related Contact entities
            .ToListAsync();
    }

    public async Task<Warehouse> GetWarehouseById(int id)
    {
        if (id <=0)
            return null;
        return await _context.Warehouse
                    .Include(w => w.Contact)  
                    .Include(l => l.Locations)
                    .FirstOrDefaultAsync(w => w.Id == id);
                    
    }

    public async Task<Warehouse> AddWarehouse(Warehouse warehouse)
    {
        var existingContact = await _context.Contact
        .FirstOrDefaultAsync(c => c.Email == warehouse.Contact.Email
        & c.Phone == warehouse.Contact.Phone
        & c.Name == warehouse.Contact.Name);
        if (existingContact != null)
        {
            warehouse.Contact = existingContact; 
        }   
        Warehouse existingWarehouse = await GetWarehouseById(warehouse.Id);
        if (existingWarehouse == null)
        {
            _context.Warehouse.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }
        return null;
    }

    public async Task<Warehouse> UpdateWarehouse(int id, Warehouse warehouse)
    {
        if (id <= 0 || warehouse == null) 
            return null;

        Warehouse warehouseToUpdate = await _context.Warehouse.FindAsync(id);
        if (warehouseToUpdate == null)
        {
            throw new Exception("Warehouse not found or has been deleted.");
        }
        warehouseToUpdate.Code = warehouse.Code;
        warehouseToUpdate.Name = warehouse.Name;
        warehouseToUpdate.Address = warehouse.Address;
        warehouseToUpdate.Zip = warehouse.Zip;
        warehouseToUpdate.City = warehouse.City;
        warehouseToUpdate.Province = warehouse.Province;
        warehouseToUpdate.Country = warehouse.Country;
        warehouseToUpdate.Contact = warehouse.Contact;
        warehouseToUpdate.CreatedAt = warehouse.CreatedAt;
        warehouseToUpdate.UpdatedAt = warehouse.UpdatedAt;

        await _context.SaveChangesAsync();
        return warehouseToUpdate;
    }

    public async Task<bool> DeleteWarehouse(int id)
    {
        var warehouseToDelete = await _context.Warehouse.FindAsync(id);
        if (warehouseToDelete != null)
        {
            _context.Warehouse.Remove(warehouseToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<Locations>> GetWarehouseLocations(int id)
    {
        if (id <= 0)
            return null;

        var warehouse = await _context.Warehouse
            .Include(w => w.Locations)
            .Where(w => w.Id == id)
            .Select(w => w.Locations)
            .FirstOrDefaultAsync();

        return warehouse?.ToList() ?? new List<Locations>(); // Return only the locations list
    }
}
