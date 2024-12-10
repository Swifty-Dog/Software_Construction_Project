public interface ISuppliers
{
    Task<Supplier> GetSupplierById(int id);
    Task<IEnumerable<Supplier>> GetSuppliers();
    Task<Supplier> AddSupplier(Supplier supplier);
    Task<Supplier> UpdateSupplier(int id, Supplier supplier);
    Task<bool> DeleteSupplier(int id);
}
