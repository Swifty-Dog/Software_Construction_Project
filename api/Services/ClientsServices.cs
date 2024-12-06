using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ClientServices : IClients
{
    private readonly MyContext _context;

    public ClientServices(MyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetClients()
    {
        return await _context.Client
            .ToListAsync();
    }

    public async Task<Client> GetClientById(int id)
    {
        if (id <= 0)
            return null;

        return await _context.Client
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client> AddClient(Client client)
    {
        Client existingClient = await GetClientById(client.Id);
        if (existingClient == null)
        {
            _context.Client.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
        return null;
    }

    public async Task<Client> UpdateClient(int id, Client client)
    {
        if (id <= 0 || client == null)
            return null;

        var existingClient = await GetClientById(id);
        if (existingClient != null)
        {
            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.City = client.City;
            existingClient.Zip = client.Zip;
            existingClient.Province = client.Province;
            existingClient.Country = client.Country;
            existingClient.ContactName = client.ContactName;
            existingClient.ContactPhone = client.ContactPhone;
            existingClient.ContactEmail = client.ContactEmail;
            existingClient.CreatedAt = client.CreatedAt;
            existingClient.UpdatedAt = client.UpdatedAt;

            await _context.SaveChangesAsync();
            return existingClient;
        }
        return null;
    }

    public async Task<bool> DeleteClient(int id)
    {
        var client = await GetClientById(id);
        if (client != null)
        {
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}