public interface ITransfers{
    public Task<IEnumerable<Transfer>> Get_Transfers();
    public Task<Transfer> Get_Transfer_By_Id(int id);
    public Task<List<Transfers_item>> Get_Transfer_Items(int id);
    public Task<Transfer> Add_Transfer(Transfer transfer);
    public Task<Transfer> Update_Transfer(int id, Transfer transfer);
    public Task<bool> Delete_Transfer(int id);
 
}