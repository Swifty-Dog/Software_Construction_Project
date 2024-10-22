public interface I_Item_Types
{
    Task<List<Item_type>> GetItem_types();
    Task<Item_type> GetItem_types_By_Id(int id);
    // public Task<Item_type> Get_Item_type_Item_Id(int id);
    Task<Item_type> UpdateItem_types(Item_type item_types);
    Task<bool> DeleteItem_types(int id);
}