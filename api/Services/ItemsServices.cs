using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ItemsServices : I_Items
{
    private readonly MyContext _context;
    public ItemsServices(MyContext context){
        _context = context;
    }

    public async Task<IEnumerable<Item>> Get_Items()
    {
        return await _context.Items
            .ToListAsync();
    }

    public async Task<Item> Get_Item_By_Id(string uid){
        //if(id <= 0) return null. en dan id als int hebben voor een makkelijkere check.
        if(uid == null)
            return null;
        return await _context.Items  
                    .FirstOrDefaultAsync(i => i.Uid == uid);
    }

    public async Task<Item> Add_Item(Item item){
        Item existingitem = await Get_Item_By_Id(item.Uid);
        if (existingitem == null)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        return null;
    }

    public async Task<Item> Update_Item(string uid, Item item)
    {
        // if (id <= 0 || item == null) 
        //     return null;
        if (uid == null || uid.Length == 0)
            return null;
        Item itemToUpdate = await _context.Items.FindAsync(uid);
        if (itemToUpdate == null){
            throw new Exception("item not found or has been deleted.");
        }
        itemToUpdate.Uid = item.Uid;
        itemToUpdate.Code = item.Code;
        itemToUpdate.Description = item.Description;
        itemToUpdate.Short_Description = item.Short_Description;
        itemToUpdate.Upc_code = item.Upc_code;
        itemToUpdate.Model_number = item.Model_number;
        itemToUpdate.Commodity_code = item.Commodity_code;
        itemToUpdate.Item_line = item.Item_line;
        itemToUpdate.Item_group =  item.Item_group;
        itemToUpdate.item_type = item.item_type;
        itemToUpdate.unit_purchase_quantity = item.unit_purchase_quantity;
        itemToUpdate.unit_order_quantity = item.unit_order_quantity;
        itemToUpdate.pack_order_quantity = item.pack_order_quantity;
        itemToUpdate.supplier_id = item.supplier_id;
        itemToUpdate.supplier_code = item.supplier_code;
        itemToUpdate.supplier_part_number = item.supplier_part_number;
        itemToUpdate.Created_at = item.Created_at;
        itemToUpdate.Updated_at = item.Updated_at;

        await _context.SaveChangesAsync();
        return itemToUpdate;
    }

    public async Task<bool> Delete_Item(string Uid){
        var itemToDelete = await _context.Items.FindAsync(Uid);
        if(itemToDelete != null){
            _context.Items.Remove(itemToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}