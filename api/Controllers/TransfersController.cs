using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class TransfersController : ControllerBase{
    private readonly TransfersServices _transfers;

    public TransfersController(TransfersServices transfers){
        _transfers = transfers;
    }

    [HttpGet("Transfers")]
    public async Task<IActionResult> Get_Transfers(){
        var transfers = await _transfers.Get_Transfers();
        return Ok(transfers);
    }

    [HttpGet("Transfer/{id}")]
    public async Task<IActionResult> Get_Transfer_By_Id(int id){
        var transfer = await _transfers.Get_Transfer_By_Id(id);
        if(transfer == null){
            return NotFound("No Transfer found with that ID");
        }
        return Ok(transfer);
    }

    [HttpGet("Transfer/{id}/items")]
    public async Task<IActionResult> Get_Transfer_Items(int id){
        var items = await _transfers.Get_Transfer_Items(id);
        if(items == null){
            return NotFound("No items found for that Transfer ID");
        }
        return Ok(items);
    }

    [HttpPost("Transfer")]
   public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
    {
        if(transfer == null)
        {
            return BadRequest("Transfer is null");
        }
        // Call the service to add the transfer
        var addedTransfer = await _transfers.Add_Transfer(transfer);

        if (addedTransfer != null)
        {
            // If the transfer was successfully added, return success
            return Ok("Transfer and items added successfully");
        }

        // If the transfer already exists, return an appropriate message
        return BadRequest("Transfer with the same id already exists.");
    }
    
    [HttpPut("Transfer/{id}")]
    public async Task<IActionResult> Update_Transfer(int id, [FromBody] Transfer transfer){
        var result = await _transfers.Update_Transfer(id, transfer);
        if(id <= 0 || id != transfer.Id){
            return BadRequest("Transfer ID is invalid or does not match the transfer ID in the request body.");
        }
        if(result == null){
            return BadRequest("Transfer could not be updated or does not exist.");
        }
        return Ok(result);
    }

    [HttpDelete("Transfer/{id}")]
    public async Task<IActionResult> Delete_Transfer(int id){
        var result = await _transfers.Delete_Transfer(id);
        if(result == false){
            return BadRequest("Transfer could not be deleted or does not exist.");
        }
        return Ok("Transfer deleted successfully");
    }
}