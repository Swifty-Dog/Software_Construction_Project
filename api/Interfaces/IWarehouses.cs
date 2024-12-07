public interface IWarehouse
{
    Task<IEnumerable<Warehouse>> GetWarehouses();
    Task<Warehouse> GetWarehouseById(int id);
    Task<Warehouse> AddWarehouse(Warehouse warehouse);
    Task<Warehouse> UpdateWarehouse(int id, Warehouse warehouse);
    Task<bool> DeleteWarehouse(int id);
}