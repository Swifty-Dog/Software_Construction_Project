using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class TransfersController : ControllerBase
{
    private readonly TransfersServices _transfers;

    public TransfersController(TransfersServices transfers)
    {
        _transfers = transfers;
    }

    // GET /Transfers: Returns all transfers.
    [HttpGet("Transfers")]
    public async Task<IActionResult> Get_Transfers()
    {
        var transfers = await _transfers.Get_Transfers();
        if (transfers == null || !transfers.Any())
        {
            return NotFound("No transfers found.");
        }
        return Ok(transfers);
    }

    // GET /Transfer/{id}: Returns a specific transfer by its ID.
    [HttpGet("Transfer/{id}")]
    public async Task<IActionResult> Get_Transfer_By_Id(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid transfer ID.");
        }

        var transfer = await _transfers.Get_Transfer_By_Id(id);
        if (transfer == null)
        {
            return NotFound($"No transfer found with ID {id}.");
        }
        return Ok(transfer);
    }

    // GET /Transfer/{id}/items: Returns all items for a specific transfer.
    [HttpGet("Transfer/{id}/items")]
    public async Task<IActionResult> Get_Transfer_Items(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid transfer ID.");
        }

        var items = await _transfers.Get_Transfer_Items(id);
        if (items == null || !items.Any())
        {
            return NotFound($"No items found for transfer ID {id}.");
        }
        return Ok(items);
    }

    // POST /Transfer: Adds a new transfer.
    [HttpPost("Transfer")]
    public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
    {
        if (transfer == null)
        {
            return BadRequest("Transfer cannot be null.");
        }

        var result = await _transfers.Add_Transfer(transfer);
        if (result == null)
        {
            return BadRequest("Transfer could not be added or already exists.");
        }
        return CreatedAtAction(nameof(Get_Transfer_By_Id), new { id = transfer.Id }, result);
    }

    // PUT /Transfer/{id}: Updates transfer information.
    [HttpPut("Transfer/{id}")]
    public async Task<IActionResult> Update_Transfer(int id, [FromBody] Transfer transfer)
    {
        if (id <= 0 || id != transfer.Id)
        {
            return BadRequest("Transfer ID is invalid or does not match the request body.");
        }

        var result = await _transfers.Update_Transfer(id, transfer);
        if (result == null)
        {
            return NotFound($"No transfer found with ID {id}.");
        }
        return Ok(result);
    }

    // DELETE /Transfer/{id}: Deletes a transfer.
    [HttpDelete("Transfer/{id}")]
    public async Task<IActionResult> Delete_Transfer(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid transfer ID.");
        }

        var deleted = await _transfers.Delete_Transfer(id);
        if (!deleted)
        {
            return NotFound($"No transfer found with ID {id}.");
        }
        return NoContent(); // 204 No Content
    }
}
