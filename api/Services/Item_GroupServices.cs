using Microsoft.EntityFrameworkCore;

public class Item_groupService: I_Item_group{
    private readonly MyContext _context;
    public Item_groupService(MyContext context){
         _context = context;
    }

    public async Task<IEnumerable<Item_group>> Get_Item_groups(){
        return await _context.ItemGroups.ToListAsync();
    }
    
    public async Task<Item_group> Get_Item_group_By_Id(int id){
        if (id <= 0)
            return null;
        return await _context.ItemGroups
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    // public async Task<Item_group> Get_Item_group_Item_Id(int id){
    //     if (id <= 0)
    //         return null;
    //     return await _context.Item_group
    //         .FirstOrDefaultAsync(i => i.Item.Id == id);
    // }

    public async Task<Item_group> Update_Item_group(Item_group item_group){
        if (item_group == null)
            return null;
        Item_group item_groupToUpdate = await _context.ItemGroups.FindAsync(item_group.Id);
        if (item_groupToUpdate == null)
        {
            throw new Exception("Item_group not found or has been deleted.");
        }
        item_groupToUpdate.Name = item_group.Name;
        item_groupToUpdate.Description = item_group.Description;
        item_groupToUpdate.Created_at = item_group.Created_at;
        item_groupToUpdate.Updated_at = item_group.Updated_at;

        await _context.SaveChangesAsync();
        return item_groupToUpdate;
    }

    public async Task<bool> Delete_Item_group(int id){
        if (id <= 0)
            return false;
        Item_group item_groupToDelete = await _context.ItemGroups.FindAsync(id);
        if (item_groupToDelete == null)
        {
            throw new Exception("Item_group not found or has been deleted.");
        }
        _context.ItemGroups.Remove(item_groupToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

}