public interface I_Inventories{
    public Task<IEnumerable<Inventory>> Get_Inventories();
    public Task<Inventory> Get_Inventory_By_Id(int id);
    public Task<List<Inventories_locations>> Get_Inventory_Locations(int id);
    public Task<Inventory> Add_Inventory(Inventory inventory);
    public Task<Inventory> Update_Inventory(int id, Inventory inventory);
    public Task<bool> Delete_Inventory(int id);
}