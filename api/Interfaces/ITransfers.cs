public interface ITransfers{
    public Task<IEnumerable<Warehouse>> Get_Transfers();
    public Task<Transfer> Get_Transfer_By_Id(int id);
    public Task<Transfer> Add_Transfer(Warehouse warehouse);
    public Task<Transfer> Update_Transfer(int id, Warehouse warehouse);
    public Task<bool> DeleteWarehouse(int id);
 
}