using Microsoft.EntityFrameworkCore;

public class ItemTypeServices : IItemTypes
{
    private readonly MyContext _context;

    public ItemTypeServices(MyContext _context)
    {
        this._context = _context;
    }

    public async Task<List<ItemType>> GetItemTypes()
    {
        return await _context.ItemTypes.ToListAsync();
    }

    public async Task<ItemType> GetItemTypesById(int id)
    {
        if(id <= 0) return null;
        return await _context.ItemTypes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<ItemType> AddItemType(ItemType itemTypes)
    {
        if(itemTypes == null) return null;
        var itemTypesExists = await _context.ItemTypes.FirstOrDefaultAsync(i => i.Id == itemTypes.Id);
        if(itemTypesExists != null)
        {
            return null;
        }
        await _context.ItemTypes.AddAsync(itemTypes);
        await _context.SaveChangesAsync();
        return itemTypes;
    }


    public async Task<ItemType> UpdateItemTypes(ItemType itemTypes)
    {
        if(itemTypes == null) return null;
        ItemType itemTypesToUpdate = await _context.ItemTypes.FindAsync(itemTypes.Id);
        if(itemTypesToUpdate == null)
        {
            throw new Exception("Item types not found or has been deleted.");
        }
        itemTypesToUpdate.Name = itemTypes.Name;
        itemTypesToUpdate.Description = itemTypes.Description;
        itemTypesToUpdate.CreatedAt = itemTypes.CreatedAt;
        itemTypesToUpdate.UpdatedAt = itemTypes.UpdatedAt;

        await _context.SaveChangesAsync();
        return itemTypes;
    }

    public async Task<bool> DeleteItemTypes(int id)
    {
        if(id <= 0) return false;
        var itemTypes = await _context.ItemTypes.FindAsync(id);
        if(itemTypes == null)
        {
            return false;
        }

        var itemsWithThisItemType = await _context.Items.Where(i => i.ItemType == id).ToListAsync();
        foreach (var item in itemsWithThisItemType)
        {
            item.ItemType = null;
        }

        await _context.SaveChangesAsync();
        _context.ItemTypes.Remove(itemTypes);
        await _context.SaveChangesAsync();
        return true;
    }
}