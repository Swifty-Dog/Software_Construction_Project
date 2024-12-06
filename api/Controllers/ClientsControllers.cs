using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ClientController : Controller
{
    private readonly ClientServices _clientServices;

    public ClientController(ClientServices clientServices)
    {
        _clientServices = clientServices;
    }

    // GET /Clients: Returns all Clients.
    [HttpGet("clients")]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _clientServices.GetClients();
        return Ok(clients);
    }

    // GET /Clients/{id}: Returns the details of a specific client by its ID.
    [HttpGet("clients/{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await _clientServices.GetClientById(id);
        if (client == null)
        {
            return NotFound("No client found with that ID");
        }
        return Ok(client);
    }

    // POST /Clients: Adds a new client.
    [HttpPost("clients")]
    public async Task<IActionResult> AddClient([FromBody] Client client)
    {
        try
        {
            var result = await _clientServices.AddClient(client);
            if (result == null)
            {
                return BadRequest("Client could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT /Clients/{id}: Updates an existing client.
    [HttpPut("clients/{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
    {
        var result = await _clientServices.UpdateClient(id, client);
        if (result == null)
        {
            return NotFound("No client found with that ID");
        }
        return Ok(result);
    }

    // DELETE /Clients/{id}: Deletes a client by its ID.
    [HttpDelete("clients/{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _clientServices.DeleteClient(id);
        if (!result)
        {
            return NotFound("No client found with that ID");
        }
        return Ok("Client deleted successfully");
    }
}