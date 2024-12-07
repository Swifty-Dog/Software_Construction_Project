public interface ITransfers
{
    public Task<IEnumerable<Transfer>> GetTransfers();
    public Task<Transfer> GetTransferById(int id);
    public Task<List<TransfersItem>> GetTransferItems(int id);
    public Task<Transfer> AddTransfer(Transfer transfer);
    public Task<Transfer> UpdateTransfer(int id, Transfer transfer);
    public Task<bool> DeleteTransfer(int id);
}