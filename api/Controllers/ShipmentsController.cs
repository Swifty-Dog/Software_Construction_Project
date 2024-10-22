using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class ShipmentsController : ControllerBase{
    private readonly ShipmentsServices _shipments;

    public ShipmentsController(ShipmentsServices shipments){
        _shipments = shipments;
    }

    [HttpGet("Shipments")]
    public async Task<IActionResult> Get_Shipments(){
        var shipments = await _shipments.Get_Shipments();
        return Ok(shipments);
    }

    [HttpGet("Shipment/{id}")]
    public async Task<IActionResult> Get_Shipment_By_Id(int id){
        var shipment = await _shipments.Get_Shipment_By_Id(id);
        if(shipment == null){
            return NotFound("No Shipment found with that ID");
        }
        return Ok(shipment);
    }

    [HttpGet("Shipment/{id}/items")]
    public async Task<IActionResult> Get_Shipment_Items(int id){
        var items = await _shipments.Get_Shipment_Items(id);
        if(items == null){
            return NotFound("No items found for that Shipment ID");
        }
        return Ok(items);
    }

    [HttpPost("Shipment")]
   public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
    {
        if(shipment == null)
        {
            return BadRequest("Shipment is null");
        }
        // Call the service to add the shipment
        var addedShipment = await _shipments.Add_Shipment(shipment);

        if (addedShipment != null)
        {
            // If the shipment was successfully added, return success
            return Ok("Shipment and items added successfully");
        }

        // If the shipment already exists, return an appropriate message
        return BadRequest("Shipment with the same id already exists.");
    }
    
    [HttpPut("Shipment/{id}")]
    public async Task<IActionResult> Update_Shipment(int id, [FromBody] Shipment shipment){
        var result = await _shipments.Update_Shipment(id, shipment);
        if(id <= 0 || id != shipment.Id){
            return BadRequest("Shipment ID is invalid or does not match the shipment ID in the request body.");
        }
        if(result == null){
            return BadRequest("Shipment could not be updated or does not exist.");
        }
        return Ok(result);
    }

    [HttpDelete("Shipment/{id}")]
    public async Task<IActionResult> Delete_Shipment(int id){
        var result = await _shipments.Delete_Shipment(id);
        if(result == false){
            return BadRequest("Shipment could not be deleted or does not exist.");
        }
        return Ok("Shipment deleted successfully");
    }
}