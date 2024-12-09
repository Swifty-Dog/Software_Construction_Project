public interface IItem
{
    public Task<IEnumerable<Item>> GetItems();
    public Task<Item> GetItemById(string uid);
    public Task<Item> AddItem(Item client);
    public Task<Item> UpdateItem(string uid, Item item);
    public Task<bool> DeleteItem(string uid);
    public Task<Inventory> GetIventoryThroughItems(string uid);
    public Task<object> GetItemTotalsFromInventory(string uid);

 }