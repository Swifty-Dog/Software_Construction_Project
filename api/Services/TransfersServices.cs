using Microsoft.EntityFrameworkCore;

public class TransfersServices : ITransfers{

    private readonly MyContext _context;
    
    public TransfersServices(MyContext context){
        _context = context;
    }
    
    public async Task<IEnumerable<Transfer>> Get_Transfers(){
        return await _context.Transfers
            .Include(w => w.Items)  // Eagerly load the related Contact entities
            .ToListAsync();
    }

    public async Task<Transfer> Get_Transfer_By_Id(int id){
        if(id <=0)
            return null;
        return await _context.Transfers
                    .Include(t => t.Items)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<Transfers_item>> Get_Transfer_Items(int id){
        if (id <= 0) return null;

        var transfer = await _context.Transfers
                                    .Include(t => t.Items)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (transfer == null)
            return null;

        return transfer.Items;
    }

    public async Task<Transfer> Add_Transfer(Transfer transfer)
    {
        if (transfer == null || transfer.Items == null || !transfer.Items.Any())
        {
            throw new ArgumentNullException("Transfer or its Items list cannot be null.");
        }

        // Get all existing items that match any of the incoming item IDs
        var existingItems = await _context.Transfer_Items
            .Where(i => transfer.Items.Select(ti => ti.Item_Id).Contains(i.Item_Id))
            .ToListAsync();

        // If existing items are found, replace the incoming items with the existing ones
        foreach (var item in transfer.Items)
        {
            var existingItem = existingItems.FirstOrDefault(ei => ei.Item_Id == item.Item_Id);
            if (existingItem != null)
            {
                item.Item_Id = existingItem.Item_Id;
                item.Amount = existingItem.Amount;
            }
        }

        // Check if the transfer already exists by checking the Reference (since it's unique)
        var existingTransfer = await _context.Transfers
            .FirstOrDefaultAsync(t => t.Id == transfer.Id);

        if (existingTransfer == null)
        {
            // If no existing transfer found, add the new transfer
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            // Set the TransferId for each item
            foreach (var item in transfer.Items)
            {
                item.TransferId = transfer.Id;
            }

            await _context.SaveChangesAsync();  // Save the items with the updated TransferId
            return transfer;
        }

        return null;  // Transfer already exists
    }

    public async Task<Transfer> Update_Transfer(int id, Transfer transfer){
        if(id <= 0) return null;
        var existingTransfer = await Get_Transfer_By_Id(id);
        if (existingTransfer == null) return null;
        existingTransfer.Reference = transfer.Reference;
        existingTransfer.Transfer_from = transfer.Transfer_from;
        existingTransfer.Transfer_to = transfer.Transfer_to;
        existingTransfer.Created_at = transfer.Created_at;
        existingTransfer.Updated_at = transfer.Updated_at;
        existingTransfer.Items = transfer.Items;
        await _context.SaveChangesAsync(); 
        return existingTransfer;
    }

    public async Task<bool> Delete_Transfer(int id){
        if (id <= 0) return false;
        var existingTransfer = await Get_Transfer_By_Id(id);
        if (existingTransfer == null) return false;
        _context.Transfers.Remove(existingTransfer);
        await _context.SaveChangesAsync();
        return true;
    }


}