// using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ClientServices: IClients{
    private readonly MyContext _context;
    public ClientServices(MyContext context){
        _context = context;
    }

    public async Task<IEnumerable<Client>> Get_Clients()
    {
        return await _context.Client
            .ToListAsync();
    }

    public async Task<Client> Get_Client_By_Id(int id){
        if(id <=0)
            return null;
        return await _context.Client  
                    .FirstOrDefaultAsync(c => c.Id == id);
}


    public async Task<Client> Add_Client(Client client){
        Client existingclient = await Get_Client_By_Id(client.Id);
        if (existingclient == null)
        {
            _context.Client.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
        return null;
    }

    public async Task<Client> Update_Client(int id, Client client)
    {
        if (id <= 0 || client == null) 
            return null;

        Client clientToUpdate = await _context.Client.FindAsync(id);
        if (clientToUpdate == null){
            throw new Exception("client not found or has been deleted.");
        }
        clientToUpdate.Name = client.Name;
        clientToUpdate.Address = client.Address;
        clientToUpdate.Zip = client.Zip;
        clientToUpdate.City = client.City;
        clientToUpdate.Country = client.Country;
        clientToUpdate.Contact_name = client.Contact_name;
        clientToUpdate.Contact_phone = client.Contact_phone;
        clientToUpdate.Contact_email = client.Contact_email;
        clientToUpdate.Created_at = client.Created_at;
        clientToUpdate.Updated_at = client.Updated_at;

        await _context.SaveChangesAsync();
        return clientToUpdate;
    }


    public async Task<bool> Delete_Client(int id){
        var clientToDelete = await _context.Client.FindAsync(id);
        if(clientToDelete != null){
            _context.Client.Remove(clientToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }


}