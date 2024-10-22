using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface I_Items{
    public Task<IEnumerable<Item>> Get_Items();
    public Task<Item> Get_Item_By_Id(string uid);
    public Task<Item> Add_Item(Item client);
    public Task<Item> Update_Item(string uid, Item item);
    public Task<bool> Delete_Item(string uid);
    }