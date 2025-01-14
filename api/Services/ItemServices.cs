using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ItemServices : IItem
{
    private readonly MyContext _context;
    public ItemServices(MyContext context){
        _context = context;
    }

    public async Task<IEnumerable<Item>> GetItems()
    {
        return await _context.Items
            .ToListAsync();
    }

    public async Task<Item> GetItemById(string uid)
    {
        //if(id <= 0) return null. en dan id als int hebben voor een makkelijkere check.
        if(uid == null)
            return null;
        return await _context.Items  
                    .FirstOrDefaultAsync(i => i.Uid == uid);
    }

    public async Task<Item> AddItem(Item item)
    {
        if (!_context.ItemGroups.Any(ig => ig.Id == item.ItemGroup))
        {
            item.ItemGroup = null;
        }
        if (!_context.ItemLines.Any(il => il.Id == item.ItemLine))
        {
            item.ItemLine = null;
        }
        if (!_context.ItemTypes.Any(it => it.Id == item.ItemType))
        {
            item.ItemType = null;
        }
        if (!_context.Suppliers.Any(s => s.Id == item.SupplierId))
        {
            item.SupplierId = null;
        }

        Item existingItem = await GetItemById(item.Uid);
        if (existingItem == null)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        return null;
    }

    public async Task<Item> UpdateItem(string uid, Item item)
    {
        if (uid == null || uid.Length == 0 || uid != item.Uid)
            return null;

        Item itemToUpdate = await _context.Items.FindAsync(uid);
        if (itemToUpdate == null){
            throw new Exception("item not found or has been deleted.");
        }
        itemToUpdate.Uid = item.Uid;
        itemToUpdate.Code = item.Code;
        itemToUpdate.Description = item.Description;
        itemToUpdate.ShortDescription = item.ShortDescription;
        itemToUpdate.UpcCode = item.UpcCode;
        itemToUpdate.ModelNumber = item.ModelNumber;
        itemToUpdate.CommodityCode = item.CommodityCode;
        itemToUpdate.ItemLine = item.ItemLine;
        itemToUpdate.ItemGroup =  item.ItemGroup;
        itemToUpdate.ItemType = item.ItemType;
        itemToUpdate.UnitPurchaseQuantity = item.UnitPurchaseQuantity;
        itemToUpdate.UnitOrderQuantity = item.UnitOrderQuantity;
        itemToUpdate.PackOrderQuantity = item.PackOrderQuantity;
        itemToUpdate.SupplierId = item.SupplierId;
        itemToUpdate.SupplierCode = item.SupplierCode;
        itemToUpdate.SupplierPartNumber = item.SupplierPartNumber;
        itemToUpdate.CreatedAt = item.CreatedAt;
        itemToUpdate.UpdatedAt = item.UpdatedAt;

        await _context.SaveChangesAsync();
        return itemToUpdate;
    }

    public async Task<bool> DeleteItem(string Uid)
    {
        var itemToDelete = await _context.Items.FindAsync(Uid);
        if(itemToDelete != null)
        {
            var transferItemsWithThisItem = await _context.TransferItems.Where(ti => ti.ItemId == Uid).ToListAsync();
            foreach (var transferItem in transferItemsWithThisItem)
            {
                transferItem.ItemId = "null";
            }
            await _context.SaveChangesAsync();
            
            _context.Items.Remove(itemToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Inventory> GetIventoryThroughItems(string uid)
    {
        return await _context.Inventories
                    .Include(t => t.Locations)
                    .FirstOrDefaultAsync(t => t.ItemId == uid);
    }

    public async Task<object> GetItemTotalsFromInventory(string uid)
    {
        if (uid == null)
            return null;

        return await _context.Inventories
                    .Where(i => i.ItemId == uid)
                    .Select(i => new 
                    {
                        i.TotalExpected,
                        i.TotalOrdered,
                        i.TotalAllocated,
                        i.TotalAvailable
                    })
                    .FirstOrDefaultAsync();
    }
}