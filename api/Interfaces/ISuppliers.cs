public interface ISuppliers
{
    Task<Supplier> Get(int id);
    Task<IEnumerable<Supplier>> GetAll();
    Task<Supplier> AddSupplier(Supplier supplier);
    Task<Supplier> UpdateSupplier(int id, Supplier supplier);
    Task<bool> DeleteSupplier(int id);
}
