using Microsoft.EntityFrameworkCore;

public class Item_lineServices : I_Item_Lines
{
    private readonly MyContext _context;
    public Item_lineServices( MyContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Item_line>> Get_Item_groups()
    {
        return await _context.ItemLines.ToListAsync();
    }
    public async Task<Item_line> Get_Item_group_By_Id(int id)
    {
        return await _context.ItemLines.FindAsync(id);
    }
    public async Task<Item_line> Update_Item_group(Item_line item_group)
    {
        _context.Entry(item_group).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return item_group;
    }
    public async Task<bool> Delete_Item_group(int id)
    {
        var item_group = await _context.ItemLines.FindAsync(id);
        if (item_group == null)
        {
            return false;
        }
        _context.ItemLines.Remove(item_group);
        await _context.SaveChangesAsync();
        return true;
    }
}