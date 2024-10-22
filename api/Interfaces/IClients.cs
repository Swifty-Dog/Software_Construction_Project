public interface IClients{
    public Task<IEnumerable<Client>> Get_Clients();
    public Task<Client> Get_Client_By_Id(int id);
    public Task<Client> Add_Client(Client client);
    public Task<Client> Update_Client(int id, Client client);
    public Task<bool> Delete_Client(int id);
 
}