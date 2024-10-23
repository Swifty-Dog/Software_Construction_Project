public interface IOrdersInterface
{
    Task<Orders> Get(int id);
    Task<IEnumerable<Orders>> GetAll();
    Task<Orders> AddOrder(Orders order);
    Task<Orders> UpdateOrder(int id, Orders order);
    Task<bool> DeleteOrder(int id);
}
