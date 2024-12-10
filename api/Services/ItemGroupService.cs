using Microsoft.EntityFrameworkCore;

public class ItemGroupService: IItemGroup
{
    private readonly MyContext _context;
    public ItemGroupService(MyContext context)
    {
         _context = context;
    }

    public async Task<IEnumerable<ItemGroup>> GetItemGroups()
    {
        return await _context.ItemGroups.ToListAsync();
    }
    
    public async Task<ItemGroup> GetItemGroupById(int id)
    {
        if (id <= 0)
            return null;
        return await _context.ItemGroups
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    // public async Task<ItemGroup> GetItemGroupItemId(int id)
    // {
    //     if (id <= 0)
    //         return null;
    //     return await _context.ItemGroup
    //         .FirstOrDefaultAsync(i => i.Item.Id == id);
    // }

    public async Task<ItemGroup> UpdateItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup == null)
            return null;
        ItemGroup itemGroupToUpdate = await _context.ItemGroups.FindAsync(itemGroup.Id);
        if (itemGroupToUpdate == null)
        {
            throw new Exception("ItemGroup not found or has been deleted.");
        }
        itemGroupToUpdate.Name = itemGroup.Name;
        itemGroupToUpdate.Description = itemGroup.Description;
        itemGroupToUpdate.Created_at = itemGroup.Created_at;
        itemGroupToUpdate.Updated_at = itemGroup.Updated_at;

        await _context.SaveChangesAsync();
        return itemGroupToUpdate;
    }

    public async Task<ItemGroup> AddItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup == null)
            return null;

        var existingItemGroup = await _context.ItemGroups
            .FirstOrDefaultAsync(ig => ig.Id == itemGroup.Id);
        if(existingItemGroup == null)
        {
            _context.ItemGroups.Add(itemGroup);
            await _context.SaveChangesAsync();
            return itemGroup;
        }
        return null;

        
    }

    public async Task<bool> DeleteItemGroup(int id){
        if (id <= 0)
            return false;
        ItemGroup itemGroupToDelete = await _context.ItemGroups.FindAsync(id);
        if (itemGroupToDelete == null)
        {
            throw new Exception("ItemGroup not found or has been deleted.");
        }
        _context.ItemGroups.Remove(itemGroupToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

}