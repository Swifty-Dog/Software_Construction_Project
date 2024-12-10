public interface IItemTypes
{
    Task<List<ItemType>> GetItemTypes();
    Task<ItemType> GetItemTypesById(int id);
    // public Task<ItemType> Get_Item_type_Item_Id(int id);
    Task<ItemType> AddItemType(ItemType itemTypes);
    Task<ItemType> UpdateItemTypes(ItemType itemTypes);
    Task<bool> DeleteItemTypes(int id);
}