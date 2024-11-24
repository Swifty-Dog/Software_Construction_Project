public interface IItemLines
{
    public Task<IEnumerable<ItemLine>> GetItemLine();
    public Task<ItemLine> GetItemLineById(int id);
    // public Task<Item_group> Get_Item_group_Item_Id(int id);
    
    public Task<ItemLine> UpdateItemLine(ItemLine itemLine, int id);
    public Task<bool> DeleteItemLine(int id);
    
}