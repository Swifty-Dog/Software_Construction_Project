public interface IItemGroup
{
    public Task<IEnumerable<ItemGroup>> GetItemGroups();
    public Task<ItemGroup> GetItemGroupById(int id);
    // public Task<ItemGroup> GetItemGroupItemId(int id);
    public Task<ItemGroup> UpdateItemGroup(ItemGroup itemGroup);
    public Task<bool> DeleteItemGroup(int id);
}