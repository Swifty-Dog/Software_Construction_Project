public interface IInventory
{
    public Task<IEnumerable<Inventory>> GetInventories();
    public Task<Inventory> GetInventoryById(int id);
    // public Task<List<Locations>> GetInventoryLocations(int id);
    public Task<List<int>> GetInventoryLocations(int id);
    public Task<Inventory> AddInventory(Inventory inventory);
    public Task<Inventory> UpdateInventory(int id, Inventory inventory);
    public Task<bool> DeleteInventory(int id);
}
