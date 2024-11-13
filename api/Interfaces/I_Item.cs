public interface I_Items
{
    public Task<IEnumerable<Item>> Get_Items();
    public Task<Item> Get_Item_By_Id(string uid);
    public Task<Item> Add_Item(Item client);
    public Task<Item> Update_Item(string uid, Item item);
    public Task<bool> Delete_Item(string uid);
    public Task<Inventory> Get_Iventory_Through_Items(string uid);
    public Task<object> Get_Item_Totals_From_Inventory(string uid);

 }