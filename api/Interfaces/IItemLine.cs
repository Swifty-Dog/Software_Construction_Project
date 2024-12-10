public interface IItemLine
{
    public Task<IEnumerable<ItemLine>> GetItemLine();
    public Task<ItemLine> GetItemLineById(int id);
    // public Task<ItemGroup> GetItemGroupItemId(int id);
    
    public Task<ItemLine> UpdateItemLine(ItemLine itemLine, int id);
    public Task<bool> DeleteItemLine(int id);
    
}