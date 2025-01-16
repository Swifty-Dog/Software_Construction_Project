using Microsoft.EntityFrameworkCore;

public class TransfersServices : ITransfers
{
    private readonly MyContext _context;
    
    public TransfersServices(MyContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Transfer>> GetTransfers()
    {
        return await _context.Transfers
            .Include(w => w.Items)  
            .ToListAsync();
    }

    public async Task<Transfer> GetTransferById(int id)
    {
        if(id <=0)
            return null;
        return await _context.Transfers
                    .Include(t => t.Items)  
                    .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<List<TransfersItem>> GetTransferItems(int id)
    {
        if (id <= 0) return null;

        var transfer = await _context.Transfers
                                    .Include(t => t.Items)  
                                    .FirstOrDefaultAsync(t => t.Id == id);
        if (transfer == null)
            return null;

        return transfer.Items;
    }

    public async Task<Transfer> AddTransfer(Transfer transfer)
    {
        if (transfer == null || transfer.Items == null || !transfer.Items.Any())
        {
            throw new ArgumentNullException("Transfer or its Items list cannot be null.");
        }
        // Get all existing items that match any of the incoming item IDs
        var checkItems = await _context.TransferItems
            .Where(i => transfer.Items.Select(ti => ti.ItemId).Contains(i.ItemId))
            .ToListAsync();
        // If existing items are found, replace the incoming items with the existing ones
        foreach (var item in transfer.Items)
        {
            var existingItem = checkItems.FirstOrDefault(ei => ei.ItemId == item.ItemId);
            if (existingItem != null)
            {
                item.ItemId = existingItem.ItemId;
                item.Amount = existingItem.Amount;
            }
            // if (!_context.Items.Any(i => i.Uid == item.ItemId)) // if item does not exist, set ItemId in trnasfersitems to null
            // {
            //     item.ItemId = "null";
            // }
        }
        
        // Check if the transfer already exists by checking the Reference (since it's unique)
        var existingTransfer = await _context.Transfers
            .FirstOrDefaultAsync(t => t.Id == transfer.Id);
        if (existingTransfer == null)
        {
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();
            foreach (var item in transfer.Items)
            {
                item.TransferId = transfer.Id;
            }
            await _context.SaveChangesAsync();  
            return transfer;
        }

        return null; 
    }

    public async Task<Transfer> UpdateTransfer(int id, Transfer transfer)
    {
        if(id <= 0) return null;
        var existingTransfer = await GetTransferById(id);
        if (existingTransfer == null) return null;
        existingTransfer.Reference = transfer.Reference;
        existingTransfer.TransferFrom = transfer.TransferFrom;
        existingTransfer.TransferTo = transfer.TransferTo;
        existingTransfer.CreatedAt = transfer.CreatedAt;
        existingTransfer.UpdatedAt = transfer.UpdatedAt;
        existingTransfer.Items = transfer.Items;
        await _context.SaveChangesAsync(); 
        return existingTransfer;
    }

    public async Task<bool> DeleteTransfer(int id)
    {
        if (id <= 0) return false;
        var existingTransfer = await GetTransferById(id);
        var isTransferUsed = await _context.TransferItems.AnyAsync(ti => ti.TransferId == id); // tijdelijk. moet nog aangepast worden op juiste class
        if (isTransferUsed)
        {
            throw new ArgumentNullException("Transfer is in use and cannot be deleted.");
        }
        if (existingTransfer == null) return false;
        _context.Transfers.Remove(existingTransfer);
        await _context.SaveChangesAsync();
        return true;
    }
}
