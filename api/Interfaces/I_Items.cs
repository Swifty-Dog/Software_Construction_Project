using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface I_Items{
    public Task<IEnumerable<Item>> Get_Items();
    public Task<Item> Get_Item_By_Id(int id);
    public Task<Item> Add_Item(Item client);
    public Task<Item> Update_Item(int id, Item client);
    public Task<bool> Delete_Item(int id);
    }