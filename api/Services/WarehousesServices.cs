// using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class WarehouseServices: IWarehouse{
    private readonly MyContext _context;
    public WarehouseServices(MyContext context){
        _context = context;
    }

    public async Task<IEnumerable<Warehouse>> Get_Warehouses()
    {
        return await _context.Warehouse
            .Include(w => w.contact)  // Eagerly load the related Contact entities
            .ToListAsync();
    }

    public async Task<Warehouse> Get_Warehouse_By_Id(int id){

        if(id <=0)
            return null;
        return await _context.Warehouse
        .Include(w => w.contact)  
        .FirstOrDefaultAsync(w => w.Id == id);
    }


    public async Task<Warehouse> Add_Warehouse(Warehouse warehouse){
        var existingContact = await _context.Contact
        .FirstOrDefaultAsync(c => c.Email == warehouse.contact.Email
        & c.Phone == warehouse.contact.Phone
        & c.Name == warehouse.contact.Name);
        if (existingContact != null){
            warehouse.contact = existingContact; 
        }   
        Warehouse existingWarehouse = await Get_Warehouse_By_Id(warehouse.Id);
        if (existingWarehouse == null)
        {
            _context.Warehouse.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }
        return null;
    }


    public async Task<Warehouse> Update_Warehouse(int id, Warehouse warehouse){
        if(id == null || warehouse == null) return null;
        if(id < 0) return null;
        var warehouseToUpdate = await _context.Warehouse.FindAsync(id);
        if(warehouseToUpdate != null){
            warehouseToUpdate.code = warehouse.code;
            warehouseToUpdate.name = warehouse.name;
            warehouseToUpdate.address = warehouse.address;
            warehouseToUpdate.zip = warehouse.zip;
            warehouseToUpdate.city = warehouse.city;
            warehouseToUpdate.country = warehouse.country;
            warehouseToUpdate.contact = warehouse.contact;
            warehouseToUpdate.created_at = warehouse.created_at;
            warehouseToUpdate.updated_at = warehouse.updated_at;
            await _context.SaveChangesAsync();
            return warehouseToUpdate;
        }
        return null;
    }

    public async Task<bool> DeleteWarehouse(int id){
        var warehouseToDelete = await _context.Warehouse.FindAsync(id);
        if(warehouseToDelete != null){
            _context.Warehouse.Remove(warehouseToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    
}