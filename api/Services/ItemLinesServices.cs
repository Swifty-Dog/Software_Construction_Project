using Microsoft.EntityFrameworkCore;

public class ItemLineServices : IItemLine
{
    private readonly MyContext _context;
    public ItemLineServices( MyContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ItemLine>> GetItemLine()
    {
        return await _context.ItemLines.ToListAsync();
    }
    public async Task<ItemLine> GetItemLineById(int id)
    {
        return await _context.ItemLines.FindAsync(id);
    }
    public async Task<ItemLine> AddItemLine(ItemLine itemLine)
    {
        if(itemLine == null)
        {
            return null;
        }
        var itemLineExists = await _context.ItemLines
            .FirstOrDefaultAsync(ig => ig.Id == itemLine.Id);  
        if(itemLineExists == null)
        {
            _context.ItemLines.Add(itemLine);
            await _context.SaveChangesAsync();
            return itemLine;
        }
        return null;
        
    }
    public async Task<ItemLine> UpdateItemLine(ItemLine itemLine, int id)
    {
        if (itemLine == null)
            return null;
        ItemLine itemLineToUpdate = await _context.ItemLines.FindAsync(id);
        if (itemLineToUpdate == null)
        {
            throw new Exception("Item line not found or has been deleted.");
        }
        itemLineToUpdate.Name = itemLine.Name;
        itemLineToUpdate.Description = itemLine.Description;
        itemLineToUpdate.CreatedAt = itemLine.CreatedAt;
        itemLineToUpdate.UpdatedAt = itemLine.UpdatedAt;

        await _context.SaveChangesAsync();
        return itemLineToUpdate;
    }
/* stappen voor restricten en valideren
    1 kijken of item bij een item in gebruik is.
    2a zo ja allesveranderen naar null
    2b zo nee verwijderen.
*/
    public async Task<Item> GetItemByItemLineId(int id)
    {
        var searchedItem = await _context.Items.FindAsync(id);
        if (searchedItem == null)
        {
            return null;
        }
        return searchedItem;
    }

    public async Task<bool> DeleteItemLine(int id)
    {
        var itemSearch = GetItemByItemLineId(id);
        if (itemSearch == null)
        {
            var itemLine = await _context.ItemLines.FindAsync(id);
            if (itemLine == null)
            {
                return false;
            }
            _context.ItemLines.Remove(itemLine);
            await _context.SaveChangesAsync();
            return true;
        }
        else // nog bezig
        {
            return false;
        }
    }
}