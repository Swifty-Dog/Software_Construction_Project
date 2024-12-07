using Microsoft.EntityFrameworkCore;

public class SuppliersServices : ISuppliers
{
    private readonly MyContext _context;

    public SuppliersServices(MyContext context)
    {
        _context = context;
    }

    public async Task<Supplier> Get(int id)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(_ => _.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAll()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task<Supplier> AddSupplier(Supplier supplier)
    {
        var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(_ => _.Code == supplier.Code);

        if (existingSupplier != null)
        {
            return null;
        }

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier> UpdateSupplier(int id, Supplier supplier)
    {
        if (id <= 0 || supplier == null)
        {
            return null;
        }

        var existingSupplier = await _context.Suppliers.FindAsync(id);
        if (existingSupplier == null)
        {
            return null;
        }

        existingSupplier.Code = supplier.Code;
        existingSupplier.Name = supplier.Name;
        existingSupplier.Address = supplier.Address;
        existingSupplier.Address_extra = supplier.Address_extra;
        existingSupplier.Zip_code = supplier.Zip_code;
        existingSupplier.Province = supplier.Province;
        existingSupplier.Country = supplier.Country;
        existingSupplier.Contact_name = supplier.Contact_name;
        existingSupplier.Phonenumber = supplier.Phonenumber;
        existingSupplier.Email = supplier.Email;
        existingSupplier.Reference = supplier.Reference;
        existingSupplier.Updated_at = DateTime.Now;

        await _context.SaveChangesAsync();
        return existingSupplier;
    }

    public async Task<bool> DeleteSupplier(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(_ => _.Id == id);
        if (existingSupplier == null)
        {
            return false;
        }

        _context.Suppliers.Remove(existingSupplier);
        await _context.SaveChangesAsync();
        return true;
    }
}
