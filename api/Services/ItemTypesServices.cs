using Microsoft.EntityFrameworkCore;

public class ItemTypeServices : IItemTypes
{
    private readonly MyContext _myContext;

    public ItemTypeServices(MyContext _myContext)
    {
        this._myContext = _myContext;
    }

    public async Task<List<ItemType>> GetItemTypes()
    {
        return await _myContext.ItemTypes.ToListAsync();
    }

    public async Task<ItemType> GetItemTypesById(int id)
    {
        if(id <= 0) return null;
        return await _myContext.ItemTypes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<ItemType> AddItemType(ItemType itemTypes)
    {
        if(itemTypes == null) return null;
        var itemTypesExists = await _myContext.ItemTypes.FirstOrDefaultAsync(i => i.Id == itemTypes.Id);
        if(itemTypesExists != null)
        {
            return null;
        }
        await _myContext.ItemTypes.AddAsync(itemTypes);
        await _myContext.SaveChangesAsync();
        return itemTypes;
    }


    public async Task<ItemType> UpdateItemTypes(ItemType itemTypes)
    {
        if(itemTypes == null) return null;
        ItemType itemTypesToUpdate = await _myContext.ItemTypes.FindAsync(itemTypes.Id);
        if(itemTypesToUpdate == null)
        {
            throw new Exception("Item types not found or has been deleted.");
        }
        itemTypesToUpdate.Name = itemTypes.Name;
        itemTypesToUpdate.Description = itemTypes.Description;
        itemTypesToUpdate.CreatedAt = itemTypes.CreatedAt;
        itemTypesToUpdate.UpdatedAt = itemTypes.UpdatedAt;

        await _myContext.SaveChangesAsync();
        return itemTypes;
    }

    public async Task<bool> DeleteItemTypes(int id)
    {
        if(id <= 0) return false;
        var itemTypes = await _myContext.ItemTypes.FindAsync(id);
        if(itemTypes == null)
        {
            return false;
        }
        _myContext.ItemTypes.Remove(itemTypes);
        await _myContext.SaveChangesAsync();
        return true;
    }
}