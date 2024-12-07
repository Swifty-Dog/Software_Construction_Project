using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class ShipmentsController : ControllerBase
{
    private readonly ShipmentsServices _shipments;

    public ShipmentsController(ShipmentsServices shipments)
    {
        _shipments = shipments;
    }

    [HttpGet("Shipments")]
    public async Task<IActionResult> GetShipments()
    {
        var shipments = await _shipments.GetShipments();
        return Ok(shipments);
    }

    [HttpGet("Shipment/{id}")]
    public async Task<IActionResult> GetShipmentById(int id)
    {
        var shipment = await _shipments.GetShipmentById(id);
        if (shipment == null)
        {
            return NotFound("No Shipment found with that ID");
        }
        return Ok(shipment);
    }

    [HttpGet("Shipment/{id}/items")]
    public async Task<IActionResult> GetShipmentItems(int id)
    {
        var items = await _shipments.GetShipmentItems(id);
        if (items == null)
        {
            return NotFound("No items found for that Shipment ID");
        }
        return Ok(items);
    }

    [HttpPost("Shipment")]
   public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
    {
        if (shipment == null)
        {
            return BadRequest("Shipment is null");
        }
        // Call the service to add the shipment
        var addedShipment = await _shipments.AddShipment(shipment);

        if (addedShipment != null)
        {
            // If the shipment was successfully added, return success
            return Ok("Shipment and items added successfully");
        }

        // If the shipment already exists, return an appropriate message
        return BadRequest("Shipment with the same id already exists.");
    }
    
    [HttpPut("Shipment/{id}")]
    public async Task<IActionResult> UpdateShipment(int id, [FromBody] Shipment shipment)
    {
        var result = await _shipments.UpdateShipment(id, shipment);
        if (id <= 0 || id != shipment.Id)
        {
            return BadRequest("Shipment ID is invalid or does not match the shipment ID in the request body.");
        }
        if (result == null)
        {
            return BadRequest("Shipment could not be updated or does not exist.");
        }
        return Ok(result);
    }

    [HttpDelete("Shipment/{id}")]
    public async Task<IActionResult> DeleteShipment(int id)
    {
        var result = await _shipments.DeleteShipment(id);
        if (result == false)
        {
            return BadRequest("Shipment could not be deleted or does not exist.");
        }
        return Ok("Shipment deleted successfully");
    }
}