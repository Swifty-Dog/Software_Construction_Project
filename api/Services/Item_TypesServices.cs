using Microsoft.EntityFrameworkCore;

public class Item_TypeServices : I_Item_Types{
    private readonly MyContext myContext;

    public Item_TypeServices(MyContext myContext){
        this.myContext = myContext;
    }

    public async Task<List<Item_type>> GetItem_types()
    {
        return await myContext.ItemTypes.ToListAsync();
    }

    public async Task<Item_type> GetItem_types_By_Id(int id)
    {
        if(id <= 0) return null;
        return await myContext.ItemTypes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Item_type> UpdateItem_types(Item_type item_types)
    {
        if(item_types == null) return null;
        Item_type item_typesToUpdate = await myContext.ItemTypes.FindAsync(item_types.Id);
        if(item_typesToUpdate == null){
            throw new Exception("Item_types not found or has been deleted.");
        }
        item_typesToUpdate.Name = item_types.Name;
        item_typesToUpdate.Description = item_types.Description;
        item_typesToUpdate.Created_at = item_types.Created_at;
        item_typesToUpdate.Updated_at = item_types.Updated_at;

        await myContext.SaveChangesAsync();
        return item_types;
    }

    public async Task<bool> DeleteItem_types(int id)
    {
        if(id <= 0) return false;
        var item_types = await myContext.ItemTypes.FindAsync(id);
        if(item_types == null){
            return false;
        }
        myContext.ItemTypes.Remove(item_types);
        await myContext.SaveChangesAsync();
        return true;
    }



}