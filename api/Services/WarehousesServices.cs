using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class WarehousesServices{
    private readonly MyContext _context;
    public WarehousesServices(MyContext context){
        _context = context;
    }

    public async Task<IEnumerable<Warehouses>> GetWarehouses(){
        return await _context.Warehouses.ToListAsync();
    }

    public async Task<Warehouses> GetWarehouseById(int id){
        if(id == null)
            return null;
        if(id <0)
            return null;
        return await _context.Warehouses.FindAsync(id);
    }

    public 
    // public async Task<IEnumerable<Warehouses>> GetWarehouses(){
    //     return await _context.Warehouses.ToListAsync();
    // }

    // public async Task<Warehouses> GetWarehouse(int id){
    //     if(id ==null)
    //         return null;
    //     return await _context.Warehouses.FindAsync(id);
    // }
    // badrequest or noConrent 
}
