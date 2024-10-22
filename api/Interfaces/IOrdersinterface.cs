public interface IOrdersInterface
{
    Task<Order> Get(int id);
    Task<IEnumerable<Order>> GetAll();
    Task<Order> AddOrder(Order order);
    Task<Order> UpdateOrder(int id, Order order);
    Task<bool> DeleteOrder(int id);
}
