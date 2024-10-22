public interface I_Item_group
{
    public Task<IEnumerable<Item_group>> Get_Item_groups();
    public Task<Item_group> Get_Item_group_By_Id(int id);
    // public Task<Item_group> Get_Item_group_Item_Id(int id);
    public Task<Item_group> Update_Item_group(Item_group item_group);
    public Task<bool> Delete_Item_group(int id);
}