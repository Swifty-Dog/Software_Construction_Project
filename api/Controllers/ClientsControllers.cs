using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ClientController : Controller{
    private readonly ClientServices _client;
    public ClientController(ClientServices client)
    {
        _client = client;
    }

    // GET /Client: Returns all Client. 
    [HttpGet("Clients")]
    public async Task<IActionResult> Get_Clients()
    {
        var clients = await _client.Get_Clients(); 
        return Ok(clients); 
    }

    // GET /Client/{id}: Returns the details of a specific client by its ID. 
    [HttpGet("Client/{id}")]
    public async Task<IActionResult> Get_Client_By_Id(int id){    
        var client = await _client.Get_Client_By_Id(id);
        if(client == null){
            return NotFound("No Client found with that ID");
        }
        return Ok(client);
    }
   

    [HttpPost("Client")]
    public async Task<IActionResult> Add_Client([FromBody] Client client)
    {
        try{
            var result = await _client.Add_Client(client);
            if (result == null){
                return BadRequest("Client could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }

    // PUT /Client/{id}: Updates client information. 
    [HttpPut("Client/{id}")]
    public async Task<IActionResult> Update_Client([FromRoute]int id, [FromBody] Client client){
        try{
            if(id <= 0 || id != client.Id){
                return BadRequest("Client ID is invalid or does not match the client ID in the request body.");
            }
            var result = await _client.Update_Client(id, client);
            if (result == null) {
                return BadRequest("Client could not be updated.");
            }
            // if(id >=0)
            //     return BadRequest("ID can not be a negative number");
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }


    // DELETE /Client/ {id}: Deletes a client. 
    [HttpDelete("Client/{id}")]
    public async Task<IActionResult> Delete_Client(int id){
        bool ClientToDeleted = await _client.Delete_Client(id);
        if(ClientToDeleted == false)
            return BadRequest("Client could not be deleted.");
        return Ok("Client deleted successfully.");
    }
}