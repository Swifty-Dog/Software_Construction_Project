public interface IClients
{
    Task<IEnumerable<Client>> GetClients();
    Task<Client> GetClientById(int id);
    Task<Client> AddClient(Client client);
    Task<Client> UpdateClient(int id, Client client);
    Task<bool> DeleteClient(int id);
}