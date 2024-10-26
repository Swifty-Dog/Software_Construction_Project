public interface IWarehouse{
    public Task<IEnumerable<Warehouse>> Get_Warehouses();
    public Task<Warehouse> Get_Warehouse_By_Id(int id);
    public Task<Warehouse> Add_Warehouse(Warehouse warehouse);
    public Task<Warehouse> Update_Warehouse(int id, Warehouse warehouse);
    public Task<bool> Delete_Warehouse(int id);
 
}